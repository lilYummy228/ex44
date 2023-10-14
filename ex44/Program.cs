using System;
using System.Collections.Generic;

namespace ex44
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandBuyWeapon = "1";
            const string CommandShowProducts = "2";
            const string CommandShowInventory = "3";
            const string CommandExit = "4";

            Seller customer = new Seller();
            Player player = new Player();
            Shop shop = new Shop();
            bool isOpen = true;

            while (isOpen)
            {
                Console.SetCursorPosition(40, 0);
                player.ShowMoney();
                Console.SetCursorPosition(0, 0);
                Console.Write($"Оружейная\n\n" +
                    $"{CommandBuyWeapon} - купить оружие\n" +
                    $"{CommandShowProducts} - показать товары\n" +
                    $"{CommandShowInventory} - показать инвентарь\n" +
                    $"{CommandExit} - выйти из программы\n" +
                    $"\nЧто желаете сделать? ");

                switch (Console.ReadLine())
                {
                    case CommandBuyWeapon:
                        shop.MakeTransaction(customer, player);
                        break;

                    case CommandShowInventory:
                        player.ShowInventory();
                        break;

                    case CommandShowProducts:
                        customer.ShowProducts();
                        break;

                    case CommandExit:
                        isOpen = false;
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Player : Person
    {
        public Player()
        {
            Random random = new Random();
            Money = random.Next(150, 501);
        }

        public int Money { get; private set; }
        public int MoneyToPay { get; private set; }

        public void ShowMoney()
        {
            Console.WriteLine($"Денег в кошельке: {Money} монет");
        }

        public void ShowInventory()
        {
            if (_inventory.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("Инвентарь\n");

                foreach (Product product in _inventory)
                {
                    Console.WriteLine($"|{product.Name}|");
                }
            }
            else
            {
                Console.WriteLine("Инвентарь пуст...");
            }
        }

        public void Pay()
        {
            Money -= MoneyToPay;
        }

        public bool CheckSolnevcy(Product product)
        {
            MoneyToPay = product.Cost;

            if (Money >= MoneyToPay)
            {
                return true;
            }
            else
            {
                MoneyToPay = 0;
                return false;
            }
        }
    }

    class Seller : Person
    {
        public Seller()
        {
            _inventory.Add(new Product("Меч", 100));
            _inventory.Add(new Product("Молот", 130));
            _inventory.Add(new Product("Секира", 170));
            _inventory.Add(new Product("Длинный лук", 150));
            _inventory.Add(new Product("Кинжал", 40));
        }

        public void ShowProducts()
        {
            int numberOfProduct = 1;

            foreach (Product product in _inventory)
            {
                Console.Write($"{numberOfProduct}. ");
                product.ShowInfo();
                numberOfProduct++;
            }
        }

        public Product TryGetProduct(out Product product)
        {
            ShowProducts();

            Console.Write("Какой товар вы хотите купить? ");

            if (int.TryParse(Console.ReadLine(), out int productNumber))
            {
                if (productNumber - 1 < _inventory.Count && productNumber > 0)
                {
                    return product = _inventory[productNumber - 1];
                }
                else
                {
                    Console.WriteLine("Такого продукта нет...");
                    return product = null;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод...");
                return product = null;
            }
        }
    }

    class Product
    {
        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{Name}\nЦена: {Cost} монет\n");
        }
    }

    class Person
    {
        protected List<Product> _inventory = new List<Product>();

        public void AddProductToInventory(Product product)
        {
            _inventory.Add(product);
        }

        public void RemoveProductFromInventory(Product product)
        {
            _inventory.Remove(product);
        }
    }

    class Shop
    {
        public void MakeTransaction(Seller customer, Player player)
        {
            Product product = customer.TryGetProduct(out product);

            if (product != null)
            {
                if (player.CheckSolnevcy(product))
                {
                    player.Pay();
                    player.AddProductToInventory(product);
                    customer.RemoveProductFromInventory(product);
                    Console.WriteLine("Товар успешно куплен...");
                }
                else
                {
                    Console.WriteLine("Не хватает монет...");
                }
            }
        }
    }
}
