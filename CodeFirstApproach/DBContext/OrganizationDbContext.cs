using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFirstApproach.DBContext
{
    public class OrganizationDbContext:DbContext
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Children> Children { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(LocalDB)\\TejaYelagonda;Database=Family;Trusted_Connection=True;");
        }
    }
}
