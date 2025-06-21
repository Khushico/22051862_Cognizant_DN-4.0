using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSearch
{
    // Step 1: Product class
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }

        public Product(string name, string category, double price)
        {
            Name = name;
            Category = category;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name} | Category: {Category} | Price: ₹{Price}";
        }
    }

    // Step 2: Search logic
    public class ProductSearch
    {
        public static List<Product> Search(List<Product> products, string keyword)
        {
            return products
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            p.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Name)
                .ToList();
        }
    }

    // Step 3: Test class
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> productList = new List<Product>
            {
                new Product("iPhone 15", "Electronics", 79999),
                new Product("Samsung Galaxy S24", "Electronics", 69999),
                new Product("Dell Laptop", "Computers", 58999),
                new Product("Nike Shoes", "Footwear", 4999),
                new Product("Sony Headphones", "Electronics", 2999),
                new Product("Apple Watch", "Wearables", 30999)
            };

            Console.WriteLine("Enter keyword to search:");
            string input = Console.ReadLine();

            var results = ProductSearch.Search(productList, input);

            Console.WriteLine($"\nSearch Results for '{input}':");
            if (results.Count > 0)
            {
                foreach (var product in results)
                    Console.WriteLine(product);
            }
            else
            {
                Console.WriteLine("No products found matching your search.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
