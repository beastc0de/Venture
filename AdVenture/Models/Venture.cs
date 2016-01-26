using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdVenture.Models
{
    public class Venture
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }
        [Display(Name = "Business Description")]
        [Required]
        public string CompanyDescription { get; set; }
        [Display(Name = "Owner Name" )]
        [Required]
        public string OwnerName { get; set; }
       
        [Display(Name = "Capital Raised")]
        [Required]
        [UIHint("Currency")]
        public decimal CapitalRaised { get; set; }
        [Required]
        public decimal Ask { get; set; }
        [Display(Name = "Created On")]
        [Required]
        
        public DateTime createdOn { get; set; }
        [Required]
        public string Sector { get; set; }
        [Required]
        public bool Verified { get; set; }

    }

}