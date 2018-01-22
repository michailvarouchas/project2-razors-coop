using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models;

namespace Project2_Cooperation.Services
{
    public class EFUserDetailsRepository : IUserDetailsRepository
    {
        private ApplicationDbContext _db;

        public EFUserDetailsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<UserDetails> UserDetails => _db.UserDetails;

        public void UpdateUserDetails(UserDetails details)
        {
            _db.UserDetails.Update(details);
            _db.SaveChanges();
        }

        public void CreateUserDetails(UserDetails details)
        {
            _db.UserDetails.Add(details);
            _db.SaveChanges();
        }
    }
}
