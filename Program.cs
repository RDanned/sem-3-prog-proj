using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace SemestralProject
{
    internal class Program
    {
        List<Category> categories = new List<Category>();

        static Category CreateCategory(int id)
        {
            Category category = new Category();
            Console.Write("Name it: ");
            category.Name = Console.ReadLine();
            category.Id = id;
            return category;
        }

        static void CreateProduct(int id, ref List<Product> products)
        {
            string answer;
            do
            {
                Product product = new Product();
                Console.Write("Name it: ");
                product.Name = Console.ReadLine();
                Console.Write("Write price: ");
                product.Price = Console.ReadLine();
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Available categories: ");
                Console.WriteLine("-------------------------------");
                Console.Write("Choose category (by entering category ID) or create new category ([cc] command): ");
                answer = Console.ReadLine();
                if(answer == "cc")
                {
                    product.Categories.Add(new Category());
                }
                product.Categories.Add(new Category());
                products.Add(product);
                return;
            } while (answer != "ok" || answer != "q");
            
        }

        static void Main(string[] args)
        {
            App app = new App();

            app.run();
            //List<Category> categories = new List<Category>();
            /*List<Product> products = new List<Product>();

            Console.WriteLine("Welcome To The Shop");
            Console.WriteLine("Welcome To The Shop");

            string answer;
            int lastCategoryId = 0;
            int lastProductId = 0;

            do
            {
                Console.WriteLine("Choose action");
                Console.WriteLine("Add category: [cc]");
                Console.WriteLine("Print categories: [cl]");
                Console.WriteLine("Add product: [pc]");
                Console.WriteLine("Print products: [pl]");
                answer = Console.ReadLine().ToLower();

                switch (answer)
                {
                    case "cc":
                        Console.Clear();
                        //this.categories.Add(CreateCategory(lastCategoryId++));
                        Console.ReadKey();
                        break;
                    case "cl":
                        Console.Clear();
                        *//*if (categories.Count > 0)
                            foreach (Category category in categories)
                            {
                                Console.WriteLine(category.toString());
                            }
                        else
                            Console.WriteLine("No categories");*//*
                        Console.ReadKey();
                        break;
                    case "pc":
                        Console.Clear();
                        CreateProduct(lastProductId++, ref products);
                        //products.Add(CreateProduct(lastProductId++));
                        Console.ReadKey();
                        break;

                }
            } while (answer != "q");*/
        }
    }
}
