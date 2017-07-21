using Microsoft.AspNetCore.Mvc.Rendering;
using SilentAuction.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }

        public IList<Listing> Listings { get; set; } = new List<Listing>();

        public string SearchQuery { get; set; }

        public string AuctionName { get; set; }

        public string AuctionEndDate { get; set; }

        public IEnumerable<SelectListItem> PageList { get; set; }

    }
}


