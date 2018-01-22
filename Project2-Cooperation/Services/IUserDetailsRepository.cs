using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IUserDetailsRepository
    {
        IQueryable<UserDetails> UserDetails { get; }
        void UpdateUserDetails(UserDetails details);
        void CreateUserDetails(UserDetails details);
    }
}
