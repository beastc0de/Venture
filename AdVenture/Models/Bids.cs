using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdVenture.Models
{
    public class Bids
    {
        [Key]
        public int Id { get; set; }
        public virtual string investorID { get; set; }
        public virtual int ventureID { get; set; }
        public string Company { get; set; }

        //[Range(0, 792281625142643375,ErrorMessage ="Bid must be a positive number")]
        [Required]
        [Display(Name = "Bid")]
        public int bid { get; set; }

        [Required]
        //[Range(0.001,100,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name ="% Stake")]
        public int bidStake { get; set; }
        [Display(Name ="Created On")]
        public DateTime createdOn { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
    }
}