using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class InternalAccount
    {
        [Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime LastTransactionDate { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
