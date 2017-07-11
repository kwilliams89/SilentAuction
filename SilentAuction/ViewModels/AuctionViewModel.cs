using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class AuctionViewModel
    {
        public IList<Listing> Listings { get; set; } = new List<Listing>();

        public IList<Item> Items { get; set; } = new List<Item>();
    }
}
