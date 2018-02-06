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

        public decimal UserBalance(string userId)
        {
            return _db.InternalAccounts.SingleOrDefault(u => u.ApplicationUserId == userId).Balance;
        }

        public void CreateNewAccount(string userId)
        {
            var account = new InternalAccount
            {
                ApplicationUserId = userId,
                LastTransactionDate = DateTime.Now,
                Balance = 150
            };
            _db.InternalAccounts.Add(account);
            _db.SaveChanges();
        }


        public bool AdminBuy(string adminId, string memberId, decimal ammount)
        {           
            var adminAccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == adminId);

            if (adminAccount.Balance >= ammount)
            {
                var memberAccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == memberId);
                memberAccount.Balance += ammount;
                adminAccount.Balance -= ammount;
                _db.SaveChanges();
                return true;
            }
            return false;
            
        }
        
        public bool TransactionCheckout(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount)
        {
            
            var userAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == userId);
           
            if (userAccount.Balance >= ammount)
            {
                var membersAccounts = new List<InternalAccount>();
                userAccount.Balance -= ammount;
                var adminAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == adminId);
                adminAccount.Balance += ammount / 2;
                foreach (var item in members)
                {
                    var membersAccount = _db.InternalAccounts.FirstOrDefault(i => i.ApplicationUserId == item.Id);
                    membersAccounts.Add(membersAccount);
                }

                foreach (var item in membersAccounts)
                {
                    item.Balance += (ammount / 2) / membersAccounts.Count;
                }

                _db.SaveChanges();
                return true;

            }
            return false;    
        }

        public void ReturnMoney(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount)
        {
            var userAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == userId);
            var adminAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == adminId);
            var membersAccounts = new List<InternalAccount>();

            foreach (var item in members)
            {
                var membersAccount = _db.InternalAccounts.FirstOrDefault(i => i.ApplicationUserId == item.Id);
                membersAccounts.Add(membersAccount);
            }

            userAccount.Balance += ammount;
            adminAccount.Balance -= ammount / 2;
            foreach (var item in membersAccounts)
            {
                item.Balance -= (ammount / 2) / membersAccounts.Count;
            }

            _db.SaveChanges();

        }


    }
}
