using System;
using System.Collections.Generic;
using System.IO;

public class BatteryItem
{
    public string ItemName { get; set; }
    public string Type { get; set; }
    public string Manufacturer { get; set; }
    public string ItemID { get; set; }
    public string ManufacturerCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Dictionary<int, int> ManufacturingAmounts { get; set; } = new Dictionary<int, int>();
    public Dictionary<int, decimal> Prices { get; set; } = new Dictionary<int, decimal>();
    public int Defects { get; set; }
    public int EnergyCapacity { get; set; }
    public string SizeClass { get; set; }
}

public class Player
{
    public string Name { get; set; }
    public decimal Balance { get; set; } = 1000.0m; // Starting balance

    public Player(string name)
    {
        Name = name;
    }

    public void Earn(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"{Name} earned {amount} IGC. New balance: {Balance} IGC.");
    }

    public void Spend(decimal amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
            Console.WriteLine($"{Name} spent {amount} IGC. New balance: {Balance} IGC.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough IGC to spend {amount}.");
        }
    }

    public void Transfer(Player recipient, decimal amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
            recipient.Balance += amount;
            Console.WriteLine($"{Name} transferred {amount} IGC to {recipient.Name}. {Name}'s new balance: {Balance} IGC. {recipient.Name}'s new balance: {recipient.Balance} IGC.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough IGC to transfer {amount}.");
        }
    }
}

class Program
{
    static void Main()
    {
        string playerDataFilePath = "gameData.txt";
        string inventoryFilePath = "inventory.txt";

        string playerData = LoadPlayerData(playerDataFilePath);
        if (playerData == null)
        {
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();
            Console.Write("Enter your score: ");
            string score = Console.ReadLine();
            Console.Write("Enter the level you're on: ");
            string level = Console.ReadLine();

            playerData = $"Player Name: {playerName}\nScore: {score}\nLevel: {level}";
            SaveData(playerDataFilePath, playerData);
        }
        else
        {
            Console.WriteLine("\nLoaded Player Data:\n" + playerData);
        }

        List<BatteryItem> inventory = new List<BatteryItem>();
        
        for (int i = 1; i <= 12; i++)
        {
            BatteryItem item = new BatteryItem();
            item.ItemName = $"5 Volt Battery X{i}";
            item.Type = "Battery";
            item.Manufacturer = "Voltage XYZ";
            item.ItemID = $"Item5V00{i}";
            item.ManufacturerCode = $"5VX{i}";
            item.StartDate = DateTime.Parse("8166-01-01").AddYears(i - 1); // Shifts start date each year
            item.EndDate = item.StartDate.AddYears(4); // 4-year manufacturing period
            item.ManufacturingAmounts = new Dictionary<int, int>
            {
                { item.StartDate.Year, 10000 }, { item.StartDate.Year + 1, 10000 },
                { item.StartDate.Year + 2, 10000 }, { item.StartDate.Year + 3, 10000 }
            };
            item.Prices = new Dictionary<int, decimal>
            {
                { item.StartDate.Year, 1.00m }, { item.StartDate.Year + 1, 1.00m },
                { item.StartDate.Year + 2, 1.00m }, { item.StartDate.Year + 3, 1.00m }
            };
            item.Defects = 100;
            item.EnergyCapacity = 500;
            item.SizeClass = "2 (5 cm by 7 cm)";
            
            inventory.Add(item);
            Console.WriteLine($"{item.ItemName} added to inventory.");
        }
        
        Player player = new Player("Connor");
        
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Inventory Menu ---");
            Console.WriteLine("1. Add item");
            Console.WriteLine("2. Remove item");
            Console.WriteLine("3. View inventory");
            Console.WriteLine("4. Save and Exit");
            Console.WriteLine("5. Earn Credits");
            Console.WriteLine("6. Spend Credits");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddItem(inventory);
                    break;
                case "2":
                    RemoveItem(inventory);
                    break;
                case "3":
                    ViewInventory(inventory);
                    break;
                case "4":
                    SaveInventory(inventoryFilePath, inventory);
                    running = false;
                    break;
                case "5":
                    Console.Write("Enter amount to earn: ");
                    decimal earnAmount = Convert.ToDecimal(Console.ReadLine());
                    player.Earn(earnAmount);
                    break;
                case "6":
                    Console.Write("Enter amount to spend: ");
                    decimal spendAmount = Convert.ToDecimal(Console.ReadLine());
                    player.Spend(spendAmount);
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    static void SaveData(string path, string data)
    {
        try
        {
            File.WriteAllText(path, data);
            Console.WriteLine("\nPlayer data successfully saved.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    static string LoadPlayerData(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            return null;
        }
    }

    static void AddItem(List<BatteryItem> inventory)
    {
        BatteryItem item = new BatteryItem();
        Console.Write("Enter the name of the item to add: ");
        item.ItemName = Console.ReadLine();
        // Set other properties similarly...
        inventory.Add(item);
        Console.WriteLine($"{item.ItemName} added to inventory.");
    }

    static void RemoveItem(List<BatteryItem> inventory)
    {
        Console.Write("Enter the name of the item to remove: ");
        string item = Console.ReadLine();
        var batteryItem = inventory.Find(i => i.ItemName.Equals(item, StringComparison.OrdinalIgnoreCase));
        if (batteryItem != null)
        {
            inventory.Remove(batteryItem);
            Console.WriteLine($"{item} removed from inventory.");
        }
        else
        {
            Console.WriteLine($"{item} not found in inventory.");
        }
    }

    static void ViewInventory(List<BatteryItem> inventory)
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
        }
        else
        {
            Console.WriteLine("Your inventory:");
            foreach (var item in inventory)
            {
                Console.WriteLine($"- {item.ItemName}");
                // Display other item properties here...
            }
        }
    }

    static void SaveInventory(string path, List<BatteryItem> inventory)
    {
        try
        {
            // Format and save inventory items to file
            File.WriteAllLines(path, new string[] { /* formatted inventory details here */ });
            Console.WriteLine("Inventory saved to file.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while saving: " + e.Message);
        }
    }
}
