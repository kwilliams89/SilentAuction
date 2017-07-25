using Microsoft.AspNetCore.Mvc.Rendering;
using SilentAuction.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }

        public int CategoryID { get; set; }

        public IList<Catagory> Categories { get; set; }

        public PaginatedList<Listing> Listings { get; set; }

        public string SearchQuery { get; set; }

        public string AuctionName { get; set; }

        public string AuctionEndDate { get; set; }

        public int? PageSize { get; set; }
    }
}


