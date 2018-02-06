using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface ITransactionRepository
    {
        bool AdminBuy(string adminId, string memberId, decimal ammount);
        bool TransactionCheckout(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount);
        decimal UserBalance(string userId);
        void ReturnMoney(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount);
       
    }
}
