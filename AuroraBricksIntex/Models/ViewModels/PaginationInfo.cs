using Microsoft.Identity.Client;

namespace AuroraBricksIntex.Models.ViewModels
{
    public class PaginationInfo
    {

        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalNumPages => (int)(Math.Ceiling((decimal)TotalItems / ItemsPerPage));  // need to turn to decimal so that it will return remainder in fraction then Math.ceiling to round up always then cast back into int

    }
}
