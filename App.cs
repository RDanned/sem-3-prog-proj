using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    internal class App
    {
        List<Category> categories = new List<Category>();
        List<Product> products = new List<Product>();
        int lastCategoryId = 0;
        int lastProductId = 0;

        string answer;


        void CreateCategory()
        {
            Category category = new Category();
            Console.Write("Name it: ");
            category.Name = Console.ReadLine();
            category.Id = lastCategoryId++;
            categories.Add(category);
            Console.Write("Category is created: " + category.toString());
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            return;
        }
        void CreateProduct()
        {
            string answer = "";
            do
            {
                Product product = new Product();
                product.Id = lastProductId++;
                Console.Write("Name it: ");
                product.Name = Console.ReadLine();
                Console.Write("Write price: ");
                product.Price = Console.ReadLine();
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Available categories: ");
                Console.WriteLine("-------------------------------");
                Console.Write("Choose category (by entering category ID) or create new category ([cc] command): ");
                answer = Console.ReadLine().ToLower();
                if (answer == "cc")
                {
                    product.Categories.Add(new Category());
                } else
                {
                    int categoryId;
                    bool isNumeric = int.TryParse(answer, out categoryId);
                    if (isNumeric)
                    {   
                        Category category = categories.Find(category => category.Id == categoryId);
                        if(category != null)
                        {
                            product.Categories.Add(new Category());
                        }
                    }
                }
                product.Categories.Add(new Category());
                products.Add(product);
                return;
            } while (answer != "ok" || answer != "q");

        }
        public void run()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome To The Shop");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Choose action");

                Console.WriteLine("Add category: [ac]"); //+
                Console.WriteLine("Print all categories: [pac]"); //+
                Console.WriteLine("Print one category: [poc]");

                Console.WriteLine("Add product: [ap]"); //+
                Console.WriteLine("Print all products: [pap]"); //+
                Console.WriteLine("Print one product: [pop]");

                Console.WriteLine("Export products to csv: [epc]");
                Console.WriteLine("Export categories to csv: [ecc]");

                Console.WriteLine("Import products from csv: [ipc]");
                Console.WriteLine("Import categories from csv: [icc]");

                answer = Console.ReadLine().ToLower();

                switch (answer)
                {
                    //Add category
                    case "ac":
                        Console.Clear();
                        CreateCategory();
                        Console.ReadKey();
                        break;

                    
                    //Print all categories
                    case "pac":
                        Console.Clear();
                        if (categories.Count > 0)
                        {
                            foreach (Category category in categories)
                            {
                                Console.WriteLine(category.toString());
                            }
                        } else {
                            Console.WriteLine("No categories");
                        }
                            
                        Console.ReadKey();
                        break;
                    //Print one category
                    case "poc":
                    
                    case "ap":
                        Console.Clear();
                        CreateProduct();
                        Console.ReadKey();
                        break;
                    case "pap":
                        Console.Clear();
                        if (categories.Count > 0)
                        {
                            foreach (Product product in products)
                            {
                                Console.WriteLine(product.toString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("No products");
                        }
                        Console.ReadKey();
                        break;
                }
            } while (answer != "q");
        }
    }
}
