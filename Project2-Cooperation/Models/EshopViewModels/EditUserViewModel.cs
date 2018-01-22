using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.EshopViewModels
{
    public class EditUserViewModel
    {
        public string User { get; set; }
        public List<string> Roles { get; set; }

        public List<SelectListItem> AllRoles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "SuperAdmin", Text = "Admin" },
            new SelectListItem { Value = "Member", Text = "Cooperation Member" },
            new SelectListItem { Value = "User", Text = "Client"  },
        };
    }
}
