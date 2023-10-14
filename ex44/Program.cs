using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex44
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Player
    {
        private List<Product> _inventory = new List<Product>();
        private int _money;
        private int _moneyToPay;

        public Player()
        {
            Random random = new Random();
            _money = random.Next(40, 101);
        }

        public bool CheckSolnevcy(Product product)
        {
            _moneyToPay = product.Count * product.Cost;

            if (_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }

        public int Pay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
        }

        public void GetProduct(Customer customer)
        {
            if (_money >= _moneyToPay)
            {
                _inventory.Add(customer.GiveProduct());
            }
        }
    }

    class Customer
    {
        private List<Product> _products = new List<Product>();

        public Customer()
        {
            _products.Add(new Product("Капуста", 2, 10));
            _products.Add(new Product("Картофель", 15, 5));
            _products.Add(new Product("Свекла", 12, 2));
            _products.Add(new Product("Лук", 9, 1));
        }

        public void ShowProducts()
        {
            foreach (Product product in _products)
            {
                product.ShowInfo();
            }
        }

        public Product GiveProduct()
        {
            Console.Write("Какой товар вы хотите купить? ");

            if (TryGetProduct(out Product product))
            {
                Console.Write("Введите кол-во которое хотите купить: ");
                int count = GetRightCount();

                if (TryGetCount(product))
                {
                    product.Count -= count;
                    return product;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public int GetRightCount()
        {
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                return count;
            }
            else
            {
                return 0;
            }
        }

        private bool TryGetProduct(out Product product)
        {
            if (int.TryParse(Console.ReadLine(), out int productNumber))
            {
                if (productNumber - 1 <= _products.Count && productNumber > 0)
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

        private bool TryGetCount(Product product)
        {
            if (int.TryParse(Console.ReadLine(), out int productCount))
            {
                if (productCount <= product.Count && productCount > 0)
                {
                    Console.WriteLine("Продукт успешно куплен...");
                    return true;
                }
                else
                {
                    Console.WriteLine("Столько у меня нет...");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод...");
                return false;
            }
        }
    }

    class Product
    {
        public Product(string name, int count, int cost)
        {
            Name = name;
            Count = count;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Count { get; private set; }
        public int Cost { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Товар: {Name}\nКоличество - {Count} шт.\nЦена: {Cost} монет\n");
        }
    }
}
