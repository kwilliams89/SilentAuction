using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class BidHistoryViewModel
    {
        public int ListingId { get; set; }

        public Listing MyListing { get; set; }
    }
}
