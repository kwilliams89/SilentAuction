using Microsoft.AspNetCore.Mvc.Rendering;
using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public class ItemViewModel
    {
        public Item Item { get; set; }

        public IList<SelectListItem> Catagories { get; set; }
    }
}
