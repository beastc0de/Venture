using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdVenture.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        public virtual string investorID { get; set; }
        public virtual int ventureID { get; set; }
        public decimal bid { get; set; }
        public DateTime createdOn { get; set; }
        public string status { get; set; }
    }
}