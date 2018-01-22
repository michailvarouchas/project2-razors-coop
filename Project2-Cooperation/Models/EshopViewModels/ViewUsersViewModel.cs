using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.EshopViewModels
{
    public class ViewUsersViewModel
    {
        public string User { get; set; }
        public List<string> Roles { get; set; }
    }
}
