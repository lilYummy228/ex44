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

            Customer customer = new Customer();
            Player player = new Player();
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
                        player.MakeTransaction(customer);
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

    class Player
    {
        private List<Product> _inventory = new List<Product>();

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

        public void MakeTransaction(Customer customer)
        {
            Product product = customer.FindProduct();

            if (product != null)
            {
                if (CheckSolnevcy(product))
                {
                    Pay();
                    _inventory.Add(product);
                    customer.SellProduct(product);
                    Console.WriteLine("Товар успешно куплен...");
                }
                else
                {
                    Console.WriteLine("Не хватает монет...");
                }
            }
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

        private bool CheckSolnevcy(Product product)
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

        private void Pay()
        {
            Money -= MoneyToPay;
        }
    }

    class Customer
    {
        private List<Product> _products = new List<Product>();

        public Customer()
        {
            _products.Add(new Product("Меч", 100));
            _products.Add(new Product("Молот", 130));
            _products.Add(new Product("Секира", 170));
            _products.Add(new Product("Длинный лук", 150));
            _products.Add(new Product("Кинжал", 40));
        }

        public void ShowProducts()
        {
            int numberOfProduct = 1;

            foreach (Product product in _products)
            {
                Console.Write($"{numberOfProduct}. ");
                product.ShowInfo();
                numberOfProduct++;
            }
        }

        public Product FindProduct()
        {
            if (TryGetProduct(out Product product))
                return product;
            else
                return null;
        }

        public void SellProduct(Product product)
        {
            _products.Remove(product);
        }

        private bool TryGetProduct(out Product product)
        {
            ShowProducts();

            Console.Write("Какой товар вы хотите купить? ");

            if (int.TryParse(Console.ReadLine(), out int productNumber))
            {
                if (productNumber - 1 < _products.Count && productNumber > 0)
                {
                    product = _products[productNumber - 1];
                    return true;
                }
                else
                {
                    Console.WriteLine("Такого продукта нет...");
                    product = null;
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод...");
                product = null;
                return false;
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
}
