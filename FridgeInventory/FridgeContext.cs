using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class FridgeContext : DbContext
    {
        public DbSet<Fridge> Fridge { get; set; }
        public DbSet<FridgeItem> FridgeItem { get; set; }
        public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\palko\\source\\repos\\FridgeInventoryManagement\\FridgeDatabaseLocal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");
        }
    }
}
