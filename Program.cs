using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable 414
namespace std;



class Program
{
    static void Main()
    {
        // Path to save player data and inventory
        string playerDataFilePath = "gameData.txt";
        string inventoryFilePath = "inventory.txt";

        // Load player data
        string playerData = LoadPlayerData(playerDataFilePath);

        if (playerData == null)
        {
            // Collect player information if it doesn't exist
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            Console.Write("Enter your score: ");
            string score = Console.ReadLine();

            Console.Write("Enter the level you're on: ");
            string level = Console.ReadLine();

            // Data to save
            playerData = $"Player Name: {playerName}\nScore: {score}\nLevel: {level}";
            
            // Save player data
            SaveData(playerDataFilePath, playerData);
        }
        else
        {
            Console.WriteLine("\nLoaded Player Data:\n" + playerData);
        }

        // Create an empty inventory
        List<string> inventory = new List<string>();

        // Load the inventory from file if it exists
        if (File.Exists(inventoryFilePath))
        {
            inventory = LoadInventory(inventoryFilePath);
            Console.WriteLine("\nInventory loaded from file.");
        }

        // Inventory management
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Inventory Menu ---");
            Console.WriteLine("1. Add item");
            Console.WriteLine("2. Remove item");
            Console.WriteLine("3. View inventory");
            Console.WriteLine("4. Save and Exit");

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
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    // Save player data
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

    // Load player data
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

    // Add an item to the inventory
    static void AddItem(List<string> inventory)
    {
        Console.Write("Enter the name of the item to add: ");
        string item = Console.ReadLine();
        inventory.Add(item);
        Console.WriteLine($"{item} added to inventory.");
    }

    // Remove an item from the inventory
    static void RemoveItem(List<string> inventory)
    {
        Console.Write("Enter the name of the item to remove: ");
        string item = Console.ReadLine();
        if (inventory.Remove(item))
        {
            Console.WriteLine($"{item} removed from inventory.");
        }
        else
        {
            Console.WriteLine($"{item} not found in inventory.");
        }
    }

    // View the current inventory
    static void ViewInventory(List<string> inventory)
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
                Console.WriteLine("- " + item);
            }
        }
    }

    // Save the inventory to a file
    static void SaveInventory(string path, List<string> inventory)
    {
        try
        {
            File.WriteAllLines(path, inventory);
            Console.WriteLine("Inventory saved to file.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while saving: " + e.Message);
        }
    }

    // Load the inventory from a file
    static List<string> LoadInventory(string path)
    {
        try
        {
            return new List<string>(File.ReadAllLines(path));
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while loading: " + e.Message);
            return new List<string>();
        }
    }
}