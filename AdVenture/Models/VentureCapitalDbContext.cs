using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AdVenture.Models
{
    public class VentureCapitalDbContext : DbContext
    {
        // if changing models, clear tables, delete migrations, and start fresh
        public DbSet<Venture> Ventures { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Bids> Bids { get; set; }
    }
}