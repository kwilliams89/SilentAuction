using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilentAuction.Models
{
    public class Item
    {
        public int Id { get; set; }
        public String ItemName { get; set; }
        public String Description { get; set; }
        public String ItemType { get; set; }
        public double RetailPrice { get; set; }
        public double StartingBid { get; set; }

    }
}
