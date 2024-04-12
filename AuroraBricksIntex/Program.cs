using AuroraBricksIntex.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NWebsec.AspNetCore.Middleware;

namespace AuroraBricksIntex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            // Take the connection string out of the program and get it from environment variables
            var connectionString = configuration["ConnectionStrings:DefaultConnection"]
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<Team410DbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)); // Add ApplicationDbContext to the services

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>(); // Use ApplicationDbContext for Identity

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

            builder.Services.AddRazorPages();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Better password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 14;
                options.Password.RequiredUniqueChars = 3;
            });

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline and add HSTS
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint(); // Provide endpoint for database migration in development environment
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Handle exceptions with error page
                app.UseHsts(); // Enable HSTS middleware for enhanced security in production
            }

            // Add Https Redirection
            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
            app.UseStaticFiles(); // Enable static file serving

            app.UseSession(); // Enable session state

            app.UseRouting(); // Enable routing

            //// Content Security Policy
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; base-uri 'self'; img-src 'self' data: https:; object-src 'none'; script-src 'self' https://stackpath.bootstrapcdn.com https://code.jquery.com/jquery-3.6.0.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js; style-src 'self' https://stackpath.bootstrapcdn.com; connect-src 'self' http://localhost:54310 https://localhost:44329 https://localhost:44389; upgrade-insecure-requests;");
            //    await next();
            //});


            // Content Security Policy
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; base-uri 'self'; img-src 'self' data: https:; object-src 'none'; script-src 'self' https://stackpath.bootstrapcdn.com https://code.jquery.com/jquery-3.6.0.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js 'unsafe-inline'; style-src 'self' https://stackpath.bootstrapcdn.com 'unsafe-inline'; connect-src 'self' http://localhost:54310 https://localhost:44329; upgrade-insecure-requests;");
                await next();
            });

            // Upgrade URLs to look better
            app.MapControllerRoute("pagenumandcategory", "{productCategoryType}/Page{pageNum}", new { Controller = "Home", action = "Index" });
            app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
            app.MapControllerRoute("productCategoryType", "{productCategoryType}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapControllerRoute("pagination", "Products/Page{pageNum}", new { Controller = "Home", action = "Index", pageNum = 1 });

            app.MapDefaultControllerRoute(); // Map default controller route

            app.MapRazorPages(); // Map Razor pages

            // Add authentication and authorization
            app.UseAuthentication(); // Enable authentication
            app.UseAuthorization(); // Enable authorization

            app.Run(); // Execute the application
        }
    }
}
