using Microsoft.AspNetCore.Mvc.Rendering;
using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }

        public PaginatedList<Listing> Listings { get; set; }

        public string SearchQuery { get; set; }

        public string TotalItems { get; set; }

        public List<SelectListGroup> Pages { get; set; }

        public string NumberOfItems { get; set; }
    }
}
