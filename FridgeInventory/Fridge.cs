using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
        //public Fridge(Person? owner, FridgeInventory items)
        //{
        //    OwnerId = owner?.Id;
        //    Items = items;
        //    FridgeInventoryId = Items.Id;
        //    Owner = owner;
        //}

        public void AddItem(FridgeItem item)
        {
            //Items.Add(item);
            ItemsList?.Add(item);
            using var db = new FridgeContext();
            item.FridgeId = Id;
            db.FridgeItem.Add(item);
            db.SaveChanges();
        }

        public void RemoveItem(FridgeItem item)
        {
            //Items.Remove(item);
        }

        //public override string ToString()
        //{
        //    //return $"Owner: {Owner}, Items: {Items}";
        //}
    }

    
}
