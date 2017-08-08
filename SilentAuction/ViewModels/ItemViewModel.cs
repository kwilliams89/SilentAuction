using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sponsor { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string RetailPrice { get; set; }

        public IEnumerable<SelectListItem> Sponsors { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public List<int> MediaIds { get; set; }
    }
}
