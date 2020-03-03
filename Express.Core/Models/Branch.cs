using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public enum BranchStatus
    {
        Closed, Open
    }
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Branch Name")]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        public long SortCode { get; set; }

        public BranchStatus Status { get; set; }
    }
    
}
