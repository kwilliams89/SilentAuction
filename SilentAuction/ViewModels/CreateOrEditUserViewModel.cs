using Microsoft.AspNetCore.Mvc.Rendering;
using SilentAuction.Models;
using System.Collections.Generic;

namespace SilentAuction.ViewModels
{
    public sealed class CreateOrEditUserViewModel
    {
        public User User { get; set; }

        public IList<SelectListItem> Roles { get; set; }
    }
}