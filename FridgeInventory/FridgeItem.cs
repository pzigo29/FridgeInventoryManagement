using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class FridgeItem
    {
        public FridgeItem(string name, int quantity = 1, FridgeItemType type = FridgeItemType.Other, DateTime? expiryDate = null, bool? inShoppingList = null)
        {
            Name = name;
            ExpiryDate = expiryDate;
            Type = type;
            Quantity = quantity;
            InShoppingList = inShoppingList;
        }

        public FridgeItem(int? id, string name, int quantity, DateTime? expiryDate, FridgeItemType type, bool? inShoppingList)
        {
            Id = id;
            Name = name;
            ExpiryDate = expiryDate;
            Type = type;
            Quantity = quantity;
            InShoppingList = inShoppingList;
        }

        [Key]
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public FridgeItemType Type { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("FridgeId")]
        public int? FridgeId { get; set; }
        public bool? InShoppingList { get; set; }
        [NotMapped]
        public bool IsExpired => ExpiryDate < DateTime.Now;

        public override string ToString()
        {
            return $"{Name} (expires on {DateWithoutTime(ExpiryDate)})";
        }

        private static string DateWithoutTime(DateTime? date)
        {
            return date != null ? ((DateTime)date).ToString("dd/MM/yyyy") : "";
        }

    }
}
