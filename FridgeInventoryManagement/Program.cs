
using System.Collections.ObjectModel;
using FridgeInventory;

Fridge fridge1/* = new Fridge(new Person("Pavol", "Žigo", 20))*/;
//fridge1.AddItem(new FridgeItem("Milk", new DateTime(2025, 12, 24)));
//fridge1.AddItem(new FridgeItem("Eggs", new DateTime(2025, 11, 20)));
//fridge1.AddItem(new FridgeItem("Bread", new DateTime(2025, 6, 20)));
//fridge1.SaveToJson();
fridge1 = Fridge.LoadFromJson("fridge.json");
Console.WriteLine(fridge1);

