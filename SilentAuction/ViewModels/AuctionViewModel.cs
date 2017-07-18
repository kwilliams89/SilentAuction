using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }

        public IList<Listing> Listings { get; set; } = new List<Listing>();

        public string SearchQuery { get; set; }
    }
}
