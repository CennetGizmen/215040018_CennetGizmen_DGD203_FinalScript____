using System;

namespace SimpleRPG
{
    internal class RPGGame
    {
        private static Character _playerCharacter;
        private static Map _gameMap;

        private static void Main()
        {
            Console.WriteLine("Welcome to the RPG Adventure!");

            InitializePlayer();
            InitializeMap();

            Console.WriteLine($"Welcome, {_playerCharacter.Name}! You find yourself in {_gameMap.CurrentLocation.Name}.");

            while (true)
            {
                DisplayOptions();
                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "M":
                        Move();
                        break;
                    case "A":
                        Attack();
                        break;
                    case "T":
                        Talk();
                        break;
                    case "P":
                        PickupItem();
                        break;
                    case "Q":
                        Console.WriteLine("Exiting the game. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid command. Try again.");
                        break;
                }
            }
        }

        private static void InitializePlayer()
        {
            Console.Write("Enter your character's name: ");
            string playerName = Console.ReadLine();
            _playerCharacter = new Character(playerName);
        }

        private static void InitializeMap()
        {
            _gameMap = new Map();
        }

        private static void DisplayOptions()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("M - Move to a new location");
            Console.WriteLine("A - Attack enemies");
            Console.WriteLine("T - Talk to NPCs");
            Console.WriteLine("P - Pickup items");
            Console.WriteLine("Q - Quit the game");
            Console.Write("Enter your choice: ");
        }

        private static void Move()
        {
            Console.WriteLine("Select a direction to move:");
            Console.WriteLine("1. North");
            Console.WriteLine("2. East");
            Console.WriteLine("3. South");
            Console.WriteLine("4. West");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 4)
            {
                _gameMap.MoveToNewLocation(choice);
                Console.WriteLine($"You moved to {_gameMap.CurrentLocation.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid choice. Try again.");
            }
        }

        private static void Attack()
        {
            if (_gameMap.CurrentLocation.HasEnemy())
            {
                Enemy enemy = _gameMap.CurrentLocation.Enemy;
                _playerCharacter.Attack(enemy);

                if (!enemy.IsAlive)
                {
                    Console.WriteLine($"You defeated the {enemy.Name}!");
                    _gameMap.CurrentLocation.RemoveEnemy();
                }
            }
            else
            {
                Console.WriteLine("No enemy to attack here.");
            }
        }

        private static void Talk()
        {
            if (_gameMap.CurrentLocation.HasNPC())
            {
                NPC npc = _gameMap.CurrentLocation.NPC;
                Console.WriteLine($"{npc.Name} says: '{npc.Message}'");
            }
            else
            {
                Console.WriteLine("No NPC to talk to here.");
            }
        }

        private static void PickupItem()
        {
            if (_gameMap.CurrentLocation.HasItem())
            {
                Item item = _gameMap.CurrentLocation.Item;
                _playerCharacter.PickupItem(item);
                Console.WriteLine($"You picked up {item.Name}: {item.Message}");
                _gameMap.CurrentLocation.RemoveItem();
            }
            else
            {
                Console.WriteLine("No item to pick up here.");
            }
        }
    }

    public class Character
    {
        public string Name { get; }
        public int Health { get; private set; }

        public Character(string name)
        {
            Name = name;
            Health = 100;
        }

        public void Attack(Enemy enemy)
        {
            Console.WriteLine($"You attacked the {enemy.Name}!");
            enemy.TakeDamage();
        }

        public void TakeDamage(int damage = 10)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Console.WriteLine("You have been defeated. Game over!");
                Environment.Exit(0);
            }
        }

        public void PickupItem(Item item)
        {
        }
    }

    public class Map
    {
        public Location CurrentLocation { get; private set; }

        public Map()
        {
            CurrentLocation = new Location("Starting Area");
        }

        public void MoveToNewLocation(int direction)
        {
            CurrentLocation = new Location($"New Location {direction}");
        }
    }

    public class Location
    {
        public string Name { get; }
        public NPC NPC { get; set; }
        public Item Item { get; set; }
        public Enemy Enemy { get; set; }

        public Location(string name)
        {
            Name = name;
        }

        public bool HasNPC()
        {
            return NPC != null;
        }

        public bool HasItem()
        {
            return Item != null;
        }

        public bool HasEnemy()
        {
            return Enemy != null;
        }

        public void RemoveEnemy()
        {
            Enemy = null;
        }

        public void RemoveItem()
        {
            Item = null;
        }
    }

    public class NPC
    {
        public string Name { get; }
        public string Message { get; }

        public NPC(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }

    public class Item
    {
        public string Name { get; }
        public string Message { get; }

        public Item(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }

    public class Enemy
    {
        public string Name { get; }
        public bool IsAlive { get; private set; }

        public Enemy(string name)
        {
            Name = name;
            IsAlive = true;
        }

        public void TakeDamage()
        {
            IsAlive = false;
        }
    }
}

class Map
{
    private int _width;
    private int _height;
    private int _currentLocationX;
    private int _currentLocationY;

    private string[,] _npcMessages;


    public int CurrentLocationX { get { return _currentLocationX; } }
    public int CurrentLocationY { get { return _currentLocationY; } }

    public Map(int width, int height)
    {
        _width = width;
        _height = height;
        _currentLocationX = 0;
        _currentLocationY = 0;

        _npcMessages = new string[width, height];
        _npcMessages[2, 2] = "Merhaba, ben NPC1!";
        _npcMessages[4, 3] = "Selam, ben NPC2!";
    }

}