using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using NCForm.Models;

namespace NCForm.DAL
{
    public class NonConformanceContext : DbContext
    {
        public NonConformanceContext() : base("NonConformanceContext")
        {
        }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<History> Histories { get; set; }
      //  public DbSet<SalesOePo> SalesOePos { get; set; }
      //  public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}