using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/*Add category: [ac]
Print all categories: [pac]
Print one category: [poc]
Add product: [ap]
Print all products: [pap]
Print one product: [pop]
Export products to csv: [epc]
Export products to xml: [epx]
Export categories to xml: [ecx]
Import products from csv: [ipc]
Import products from xml: [ipx]
Import categories from csv: [icc]
Import categories from xml: [icx]
Delete product: [dp]
Delete category: [dc]*/

namespace SemestralProject
{
    static class Commands 
    {
        public const string AddCategory = "ac";
        public const string DeleteCategory = "dc";
        public const string PrintAllCategories = "pac";
        public const string PrintOneCategory = "poc";

        public const string AddProduct = "ap";
        public const string DeleteProduct = "dp";
        public const string PrintAllProducts = "pap";
        public const string PrintOneProduct = "pop";

        public const string ExportProductsToCsv = "epc";
        public const string ExportProductsToXml = "epx";
        public const string ExportCategoriesToCsv = "ecc";
        public const string ExportCategoriesToXml = "ecx";

        public const string ImportProductsFromCsv = "ipc";
        public const string ImportProductsFromXml = "ipx";
        public const string ImportCategoriesFromCsv = "icc";
        public const string ImportCategoriesFromXml = "icx";

        public const string Back = "back";



    }

    internal class App
    {
        List<Category> categories = new List<Category>();
        List<Product> products = new List<Product>();
        int lastCategoryId = 0;
        int lastProductId = 0;

        string answer;


        void AddCategory()
        {
            Console.WriteLine("You are creating new category");
            Category category = new Category();
            Console.Write("Name it: ");
            category.Name = Console.ReadLine();
            category.Id = lastCategoryId++;
            categories.Add(category);
            Console.Write("Category is created: " + category.ToString());
            Console.ReadKey();
            return;
        }

        void DeleteCategory()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are deleting category");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Available categories: ");
                PrintAllCategories();
                Console.WriteLine("Write category Id that will be deleted: ");
                int categoryId;
                bool isNumeric = int.TryParse(answer, out categoryId);
                if (isNumeric)
                {
                    int categoryIndex = categories.FindIndex(category => category.Id == categoryId);
                    Category category = categories.Find(category => category.Id == categoryId);
                    if (categoryIndex != -1 && category != null)
                    {
                        bool productHasCategory = false;
                        List<Product> productsWithCategory = new List<Product>();
                        foreach (Product product in products)
                        {
                            if(product.Category.Id == category.Id)
                            {
                                productHasCategory = true;
                                productsWithCategory.Add(product);
                                break;
                            }
                        }

                        if (productHasCategory)
                        {
                            Console.WriteLine("Category is assigned to theese products:");
                            foreach (Product product in productsWithCategory)
                                PrintOneProduct(product.Id);
                            Console.WriteLine("Delete them first:");
                            continue;
                        }
                        categories.Remove(category);
                        Console.WriteLine($"Category is deleted: {category.ToString()}");
                        //product.Categories.Add(new Category());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Category doesn't exists");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong product id. Product id must be numeric");
                }

            } while (answer != Commands.Back);
        }

        void AddProduct()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are creating product");
                Product product = new Product();
                product.Id = lastProductId++;
                Console.Write("Name it: ");
                product.Name = Console.ReadLine();
                Console.Write("Write price: ");
                product.Price = Console.ReadLine();
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Available categories: ");
                if (categories.Count > 0)
                {
                    foreach(Category category in categories)
                    {
                        Console.WriteLine(category.ToString());
                    }
                } else
                {
                    Console.WriteLine("There is no categories");
                }
                Console.WriteLine("-------------------------------");

                Console.Write($"Choose category (by entering category ID) or create new category ([{Commands.AddCategory}] command): ");
                answer = Console.ReadLine().ToLower();
                if (answer == Commands.AddCategory)
                {
                    this.AddCategory();
                    //product.Categories.Add();
                    product.Category = this.categories.Last();
                } else
                {
                    int categoryId;
                    bool isNumeric = int.TryParse(answer, out categoryId);
                    if (isNumeric)
                    {   
                        Category category = categories.Find(category => category.Id == categoryId);
                        if(category != null)
                        {
                            //product.Categories.Add(new Category());
                            product.Category = category;
                        } else
                        {
                            Console.WriteLine("Category doesn't exists");
                        }
                    }
                }

                products.Add(product);
                Console.WriteLine($"New product was created: {product.ToString()}");
                break;
                return;
            } while (answer != "ok" || answer != "q");
            return;
        }

        void DeleteProduct()
        {
            
            string answer = "";
            do
            {
                Console.WriteLine("You are deleting a product");
                Console.Write("Chooise product id to delete");
                answer = Console.ReadLine().ToLower();

                int productId;
                bool isNumeric = int.TryParse(answer, out productId);
                if (isNumeric)
                {
                    int productIndex = products.FindIndex(product => product.Id == productId);
                    Product product = products.Find(product => product.Id == productId);
                    if (productIndex != -1)
                    {
                        products.Remove(product);
                        Console.WriteLine($"Product is deleted: {product.ToString()}");
                        //product.Categories.Add(new Category());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Category doesn't exists");
                        continue;
                    }
                } else
                {
                    Console.WriteLine("Wrong product id. Product id must be numeric");
                }
            } while (answer != Commands.Back);
            return;
        }

        void PrintAllProducts()
        {
            if (products.Count > 0)
            {
                foreach (Product product in products)
                {
                    Console.WriteLine(product.ToString());
                }
            }
            else
            {
                Console.WriteLine("No products");
            }
            return;
        }

        void PrintOneProduct(int? productId)
        {
            Product product = products.Find(product => product.Id == productId);
            if(product != null)
            {
                Console.WriteLine(product.ToString());
            } else
            {
                Console.WriteLine($"Product with id {productId} wasn't found");
            }
            return;
        }

        void PrintAllCategories()
        {
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    Console.WriteLine(category.ToString());
                }
            }
            else
            {
                Console.WriteLine("No categories");
            }
            return;
        }
        
        public void run()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome To The Shop");
                Console.WriteLine("-------------------------------");

                Console.WriteLine($"Add category: [{Commands.AddCategory}]");
                Console.WriteLine($"Delete category: [{Commands.DeleteCategory}]");
                Console.WriteLine($"Print all categories: [{Commands.PrintAllCategories}]");
                Console.WriteLine($"Print one category: [{Commands.PrintOneCategory}]");

                Console.WriteLine($"Add product: [{Commands.AddProduct}]");
                Console.WriteLine($"Delete product: [{Commands.DeleteProduct}]");
                Console.WriteLine($"Print all products: [{Commands.PrintAllProducts}]");
                Console.WriteLine($"Print one product: [{Commands.PrintOneProduct}]");

                Console.WriteLine($"Export products to csv: [{Commands.ExportProductsToCsv}]");
                Console.WriteLine($"Export products to xml: [{Commands.ExportProductsToXml}]");
                Console.WriteLine($"Export categories to csv: [{Commands.ExportCategoriesToCsv}]");
                Console.WriteLine($"Export categories to xml: [{Commands.ExportCategoriesToXml}]");

                Console.WriteLine($"Import products from csv: [{Commands.ImportProductsFromCsv}]");
                Console.WriteLine($"Import products from xml: [{Commands.ImportProductsFromXml}]");
                Console.WriteLine($"Import categories from csv: [{Commands.ImportCategoriesFromCsv}]");
                Console.WriteLine($"Import categories from xml: [{Commands.ImportProductsFromXml}]");

                Console.Write("Choose action: ");
                answer = Console.ReadLine().ToLower();

                switch (answer)
                {
                    //Add category
                    case Commands.AddCategory:
                        Console.Clear();
                        AddCategory();
                        Console.WriteLine("New category was created. Press any key to return to main menu");
                        Console.ReadKey();
                        break;
                    //Delete category
                    case Commands.DeleteCategory:
                        Console.Clear();
                        DeleteCategory();
                        Console.ReadKey();
                        break;
                    //Print all categories
                    case Commands.PrintAllCategories:
                        Console.Clear();
                        PrintAllCategories();
                        Console.ReadKey();
                        break;
                    //Print one category
                    case Commands.PrintOneCategory:
                        break;
                    //Add product
                    case Commands.AddProduct:
                        Console.Clear();
                        AddProduct();
                        Console.WriteLine("New product was created. Press any key to return to main menu");
                        Console.ReadKey();
                        break;
                    case Commands.DeleteProduct:
                        DeleteProduct();
                        break;
                    //Print all products
                    case Commands.PrintAllProducts:
                        Console.Clear();
                        PrintAllProducts();
                        Console.ReadKey();
                        break;
                    //Print one product
                    case Commands.PrintOneProduct:
                        break;
                    //Export products to csv
                    case Commands.ExportProductsToCsv:
                        break;
                    case Commands.ExportProductsToXml:
                        break;
                    case Commands.ExportCategoriesToCsv:
                        break;
                    case Commands.ExportCategoriesToXml:
                        break;
                    case Commands.ImportProductsFromCsv:
                        break;
                    case Commands.ImportProductsFromXml:
                        break;
                    case Commands.ImportCategoriesFromCsv:
                        break;
                    case Commands.ImportCategoriesFromXml:
                        break;
                }
            } while (answer != "q");
        }
    }
}
