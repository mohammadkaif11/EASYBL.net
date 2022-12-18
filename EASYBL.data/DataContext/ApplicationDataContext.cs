using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EASYBL.model.Model;

namespace EASYBL.data.DataContext
{
    public partial class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext()
            : base("name=dbconnection")
        {
        }

        public virtual DbSet<Bills> Bills { get; set; }
        public virtual DbSet<ShopkeeperUsers> ShopkeeperUsers { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }

    }
}
