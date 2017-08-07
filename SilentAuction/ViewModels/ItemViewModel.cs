using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilentAuction.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sponsor { get; set; }

        public string Description { get; set; }

        public string category { get; set; }

        public string RetailPrice { get; set; }
    }
}
