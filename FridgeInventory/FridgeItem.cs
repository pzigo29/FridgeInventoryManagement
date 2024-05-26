using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class FridgeItem(string name, DateTime? expiryDate = null)
    {
    public string Name { get; set; } = name;
    public DateTime? ExpiryDate { get; set; } = expiryDate;

    public override string ToString()
    {
        return $"{Name} (expires on {ExpiryDate})";
    }

    }
}
