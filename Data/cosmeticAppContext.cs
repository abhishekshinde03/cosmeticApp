using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cosmeticApp.Models;

namespace cosmeticApp.Data
{
    public class cosmeticAppContext : DbContext
    {
        public cosmeticAppContext (DbContextOptions<cosmeticAppContext> options)
            : base(options)
        {
        }

        public DbSet<cosmeticApp.Models.product> product { get; set; }

        public DbSet<cosmeticApp.Models.users> users { get; set; }

        public DbSet<cosmeticApp.Models.orders> orders { get; set; }

        public DbSet<cosmeticApp.Models.analysis> analysis { get; set; }
    }
}
