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

public class Planet
{
    public string Name { get; set; }
    public List<Resource> Resources { get; set; } = new List<Resource>();

    public void DisplayResources()
    {
        Console.WriteLine($"Planet: {Name}");
        Console.WriteLine("Resources:");
        foreach (var resource in Resources)
        {
            Console.WriteLine($"- {resource.Name} ({resource.Symbol}): {resource.Description}");
        }
    }
}

class Program
{
    static void Main()
    {
        // Generate resources and planets
        List<Resource> resources = GenerateResources();
        List<Planet> planets = GeneratePlanets(resources);
        string planetsFilePath = "planets.txt";

        // Write planets to a file
        WritePlanetsToFile(planets, planetsFilePath);
        
        Player player = new Player("Connor");
        bool running = true;
        
        while (running)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. Inventory");
            Console.WriteLine("2. Planet Information");
            Console.WriteLine("3. Save and Exit");
            Console.WriteLine("4. Earn Credits");
            Console.WriteLine("5. Spend Credits");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Inventory management
                    break;
                case "2":
                    Console.WriteLine("Enter planet number (1-5): ");
                    int planetNumber = int.Parse(Console.ReadLine()) - 1;
                    if (planetNumber >= 0 && planetNumber < planets.Count)
                    {
                        planets[planetNumber].DisplayResources();
                    }
                    else
                    {
                        Console.WriteLine("Invalid planet number.");
                    }
                    break;
                case "3":
                    running = false;
                    break;
                case "4":
                    Console.Write("Enter amount to earn: ");
                    decimal earnAmount = Convert.ToDecimal(Console.ReadLine());
                    player.Earn(earnAmount);
                    break;
                case "5":
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

    static List<Resource> GenerateResources()
    {
        return new List<Resource>
        {
            new Resource { Name = "Water", Symbol = "H₂O", Description = "Essential for life." },
            new Resource { Name = "Iron", Symbol = "Fe", Description = "Crucial for the planet's core and magnetic field." },
            new Resource { Name = "Silicon", Symbol = "Si", Description = "Key for making rocky surfaces and tech components." },
            new Resource { Name = "Oxygen", Symbol = "O₂", Description = "Vital for breathable atmospheres." },
            new Resource { Name = "Carbon", Symbol = "C", Description = "Forms the basis of organic life." },
            // Add the rest of the 80 resources here...
        };
    }

    static List<Planet> GeneratePlanets(List<Resource> resources)
    {
        Random random = new Random();
        List<Planet> planets = new List<Planet>();

        for (int i = 1; i <= 5; i++) // Generate 5 planets for example
        {
            Planet planet = new Planet { Name = $"Planet {i}" };

            int numberOfResources = random.Next(10, 20); // Each planet has 10-20 random resources
            for (int j = 0; j < numberOfResources; j++)
            {
                Resource resource = resources[random.Next(resources.Count)];
                if (!planet.Resources.Contains(resource)) // Avoid duplicates
                {
                    planet.Resources.Add(resource);
                }
            }

            planets.Add(planet);
        }

        return planets;
    }

    static void WritePlanetsToFile(List<Planet> planets, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var planet in planets)
            {
                writer.WriteLine($"Planet: {planet.Name}");
                writer.WriteLine("Resources:");
                foreach (var resource in planet.Resources)
                {
                    writer.WriteLine($"- {resource.Name} ({resource.Symbol}): {resource.Description}");
                }
                writer.WriteLine();
            }
        }
    }
}
