using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdVenture.Models
{
    public class Investment
    {
        [Key]
        public int Id { get; set; }
        //public virtual int bidId { get; set; }
        public virtual string InvestorID { get; set; }
        public virtual int VentureID { get; set; }

    }
}