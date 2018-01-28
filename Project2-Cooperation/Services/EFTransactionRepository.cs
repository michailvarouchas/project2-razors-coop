using Project2_Cooperation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project2_Cooperation.Models;
using Project2_Cooperation.Data;

namespace Project_Cooperation.Services
{
    public class EFTransactionRepository : ITransactionRepository
    {
        private ApplicationDbContext _db;

        public EFTransactionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AdminBuy(ApplicationUser member, decimal ammount)
        {
            var admin = _db.Users.SingleOrDefault(i => i.Email == "admin@afdemp.gr");
            var adminaccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == admin.Id);
            var memberaccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == member.Id);
            memberaccount.Balance += ammount;
            adminaccount.Balance -= ammount;
            _db.SaveChanges();

        }
        
        public void TransactionCheckout(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount)
        {
            
            var userAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == userId);
            userAccount.Balance -= ammount;
            var adminAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == adminId);
            adminAccount.Balance += ammount / 2;

            var membersaccounts = new List<InternalAccount>();
            var rest = new List<InternalAccount>();

            foreach (var item in members)
            {
                var memberaccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == item.Id);
                if (memberaccount != null)
                {
                    membersaccounts.Add(memberaccount);
                }
                else
                {
                    rest.Add(memberaccount);
                }
            }

            foreach (var item in membersaccounts)
            {
                item.Balance += (ammount / 2) / membersaccounts.Count;
            }
           

            _db.SaveChanges();
        }


    }
}
