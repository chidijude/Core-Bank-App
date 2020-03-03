using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    

        public enum TellerPostingType
        {
            Deposit, Withdrawal
        }

        public class TellerPosting
        {
            public int ID { get; set; }

            [Required(ErrorMessage = "Enter Amount")]
            [Display(Name = "Amount")]
            [DataType(DataType.Currency)]
            [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Invalid amount"), Range(1, (double)Decimal.MaxValue, ErrorMessage = ("Amount must be between 1 and a reasonable maximum value"))]
            public decimal Amount { get; set; }

            [DataType(DataType.MultilineText)]
            public string Narration { get; set; }

            [DataType(DataType.Date)]
            public DateTime Date { get; set; }

            [Required(ErrorMessage = "Posting type required")]
            public TellerPostingType PostingType { get; set; }

            [Display(Name = "Account")]
            public int CustomerAccountID { get; set; }
            public virtual CustomerAccount CustomerAccount { get; set; }

            [Display(Name = "Post Initiator")]
            public string PostInitiatorId { get; set; }

            [Display(Name = "Till Account")]
            public int? TillAccountID { get; set; }
            public virtual GlAccount TillAccount { get; set; }

            [Display(Name = "Post Status")]
            public PostStatus Status { get; set; }
        }
    
}
