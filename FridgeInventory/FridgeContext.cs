using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FridgeInventory
{
    public class FridgeContext : DbContext
    {
        public DbSet<Fridge> Fridge { get; set; }
        public DbSet<FridgeItem> FridgeItem { get; set; }
        public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\FridgeInventory\FridgeDatabaseLocal.mdf"));
            optionsBuilder.UseSqlServer($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;Encrypt=True");
        }


        public static bool RemovePerson(int ownerId)
        {
            try
            {
                using var db = new FridgeContext();
                var person = db.Person.Find(ownerId);
                if (person == null) return false;
                var fridges = db.Fridge.Where(f => f.OwnerId == ownerId);
                foreach (var fridge in fridges)
                {
                    RemoveFridge(fridge.Id);
                }
                db.Person.Remove(person);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFridge(int? fridgeId)
        {
            try
            {
                using var db = new FridgeContext();
                var fridge = db.Fridge.Find(fridgeId);
                if (fridge == null) return false;
                var items = db.FridgeItem.Where(i => i.FridgeId == fridgeId);
                foreach (var fridgeItem in items)
                {
                    RemoveItem(fridgeItem.Id);
                }
                db.SaveChanges();
                db.Fridge.Remove(fridge);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveItem(int? itemId)
        {
            try
            {
                using var db = new FridgeContext();
                var item = db.FridgeItem.Find(itemId);
                if (item == null) return false;
                db.FridgeItem.Remove(item);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static string HashPassword(string password, string seed)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(seed), 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateSeed()
        {
            byte[] seed = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(seed);
            }
            return Convert.ToBase64String(seed);
        }
        public static bool VerifyPassword(string enteredPassword, string storedSeed, string storedHash)
        {
            var enteredHash = FridgeContext.HashPassword(enteredPassword, storedSeed);
            return enteredHash == storedHash;
        }
    }
}
