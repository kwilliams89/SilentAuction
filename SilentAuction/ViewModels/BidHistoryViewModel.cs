using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class BidHistoryViewModel
    {
        public int ListingId { get; set; }

        public Listing MyListing { get; set; }

        public decimal CurrentBid { get; set; }

        public decimal MinimumBid { get; set; }

        public decimal Increment { get; set; }

        public string MySponsor { get; set; }

        public BidDetailsViewModel MyBidDetails {get; set;}

    }
}
