using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface ITransactionRepository
    {
        void AdminBuy(string adminId, string memberId, decimal ammount);
        bool TransactionCheckout(string adminId, string userId, IEnumerable<ApplicationUser> members, decimal ammount);
    }
}
