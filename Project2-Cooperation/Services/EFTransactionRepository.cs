﻿using Project2_Cooperation.Services;
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

        public void AdminBuy(string adminId, string memberId, decimal ammount)
        {           
            var adminAccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == adminId);
            var memberAccount = _db.InternalAccounts.SingleOrDefault(i => i.ApplicationUserId == memberId);
            memberAccount.Balance += ammount;
            adminAccount.Balance -= ammount;
            _db.SaveChanges();
        }
        
        public void TransactionCheckout(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount)
        {
            
            var userAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == userId);
            userAccount.Balance -= ammount;
            var adminAccount = _db.InternalAccounts.SingleOrDefault(a => a.ApplicationUserId == adminId);
            adminAccount.Balance += ammount / 2;
            
            var membersAccounts = new List<InternalAccount>();

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
        }


    }
}
