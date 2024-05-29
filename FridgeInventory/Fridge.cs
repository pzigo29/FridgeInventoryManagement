using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeInventory
{
    public class Fridge
    {
        [Key]
        public int? Id { get; set; }
        [NotMapped]
        public Person? Owner { get; set; }
        [ForeignKey("OwnerId")]
        public int? OwnerId { get; set; }
        [NotMapped]
        public virtual ICollection<FridgeItem>? ItemsList { get; set; } = [];
        public string Name { get; set; }
        public Fridge(int? ownerId, string name,int? id = null)
        {
            Id = id;
            OwnerId = ownerId;
            Owner = null;
            Name = name;
        }

        public Fridge(string name, int? ownerId = null)
        {
            OwnerId = ownerId;
            Owner = null;
            Name = name;
        }

        public void AddItem(FridgeItem item)
        {
            ItemsList?.Add(item);
            using var db = new FridgeContext();
            item.FridgeId = Id;
            db.FridgeItem.Add(item);
            db.SaveChanges();
        }
    }

    
}
