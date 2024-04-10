using AuroraBricksIntex.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace AuroraBricksIntex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;


            var connectionString = configuration["ConnectionStrings:DefaultConnection"]
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<Team410DbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Team410DbContext>(); // Use Team410DbContext for Identity

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

            builder.Services.AddRazorPages();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Enable HSTS middleware
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.MapControllerRoute("pagenumandcategory", "{productCategoryType}/{pageNum}", new { Controller = "Home", action = "Index" });
            app.MapControllerRoute("productCategoryType", "{productCategoryType}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapControllerRoute("pagination", "{pageNum}", new { Controller = "Home", action = "Index", pageNum = 1 });

            app.MapDefaultControllerRoute();

            app.MapRazorPages();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Run();
        }
    }
}
