using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class ListingViewModel
    {
        public int Id { get; set; }

        public string Auction { get; set; }

        public string Item { get; set; }

        public string MinimumBid { get; set; }

        public string Increment { get; set; }

        public IEnumerable<SelectListItem> Auctions { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
