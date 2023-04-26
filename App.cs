using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;

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
        public const string EditCategory = "ec";
        public const string PrintAllCategories = "pac";
        public const string PrintOneCategory = "poc";

        public const string AddProduct = "ap";
        public const string DeleteProduct = "dp";
        public const string EditProduct = "ep";
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

        void PressAnyKeyMsg()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        void UpdateLastCategoryId()
        {
            if (categories.Count > 0)
            {
                int biggestCategoryId = 0;
                foreach (Category category in categories)
                {
                    if (category.Id > biggestCategoryId)
                        biggestCategoryId = category.Id;
                }
                lastCategoryId = biggestCategoryId;
            }
            else
            {
                lastCategoryId = 0;
            }
        }

        void UpdateLastProductId()
        {
            if (products.Count > 0)
            {
                int biggestProductId = 0;
                foreach (Product product in products)
                {
                    if (product.Id > biggestProductId)
                        biggestProductId = product.Id;
                }
                lastProductId = biggestProductId;
            }
            else
            {
                lastProductId = 0;
            }
        }

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
                Console.Clear();
                Console.WriteLine("You are deleting category");
                BackToMenuMessage();
                PrintAvailableCategories();
                Console.Write("Write category Id that will be deleted: ");
                answer = Console.ReadLine();
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
                            if (product.Category != null && product.Category.Id == category.Id)
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
                            Console.WriteLine("Delete them first.");
                            Console.ReadKey();
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
                
                PrintAvailableCategories();

                Console.Write($"Choose category (by entering category ID) or create new category ([{Commands.AddCategory}] command): ");
                answer = Console.ReadLine().ToLower();
                if (answer == Commands.AddCategory)
                {
                    this.AddCategory();
                    product.Category = this.categories.Last();
                } else
                {
                    int categoryId;
                    bool isNumeric = int.TryParse(answer, out categoryId);
                    if (isNumeric)
                    {
                        Category category = categories.Find(category => category.Id == categoryId);
                        if (category != null)
                        {
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
            } while (answer != Commands.Back);
            return;
        }

        void DeleteProduct()
        {

            string answer = "";
            do
            {
                Console.WriteLine("You are deleting a product");
                Console.Write("Chooise product id to delete");
                BackToMenuMessage();
                answer = Console.ReadLine().ToLower();

                if (IsValidId(answer))
                {
                    int productId = int.Parse(answer);
                    int productIndex = products.FindIndex(product => product.Id == productId);
                    Product product = products.Find(product => product.Id == productId);
                    if (productIndex != -1)
                    {
                        products.Remove(product);
                        Console.WriteLine($"Product is deleted: {product.ToString()}");
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
            if (product != null)
            {
                Console.WriteLine(product.ToString());
            } else
            {
                Console.WriteLine($"Product with id {productId} wasn't found");
            }
            return;
        }

        void PrintOneProduct()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are printing a product");

                Console.Write("Chooise product id to print");
                BackToMenuMessage();
                answer = Console.ReadLine().ToLower();

                if (IsValidId(answer))
                {
                    int productId = int.Parse(answer);
                    int productIndex = products.FindIndex(product => product.Id == productId);
                    Product product = products.Find(product => product.Id == productId);
                    if (productIndex != -1)
                    {
                        Console.WriteLine($"{product.ToString()}");
                        if(product.Category != null)
                        {
                            Console.WriteLine("Product in category:");
                            Console.WriteLine(product.Category.ToString());
                        } else
                        {
                            Console.WriteLine("Product doesn't has category");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Product doesn't exists");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong product id. Product id must be numeric");
                }
            } while (answer != Commands.Back);
            return;
        }

        void PrintAvailableCategories()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Available categories: ");
            PrintAllCategories();
            Console.WriteLine("-------------------------------");
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
                Console.WriteLine("There is no categories");
            }
            return;
        }

        bool IsValidId(string id)
        {
            return int.TryParse(id, out _);
        }

        void PrintOneCategory()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are printing the category");
                PrintAllCategories();
                Console.Write("Chooise category id to display category");
                BackToMenuMessage();
                answer = Console.ReadLine().ToLower();

                if (IsValidId(answer))
                {
                    int categoryId = int.Parse(answer);
                    int categoryIndex = categories.FindIndex(category => category.Id == categoryId);
                    Category category = categories.Find(category => category.Id == categoryId);
                    if (categoryIndex != -1)
                    {
                        Console.WriteLine($"{category.ToString()}");
                        Console.WriteLine("Products in category: ");
                        foreach(Product product in products)
                        {
                            if(product.Category != null && product.Category.Id == categoryId)
                            {
                                Console.WriteLine($"{product.ToString()}");
                            }
                        }
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
                    Console.WriteLine("Wrong category id. Category id must be numeric");
                }
            } while (answer != Commands.Back);
            return;
        }

        /// <summary>
        /// Provide generic and specific handling of fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object GetCsvFieldasedOnValue<ShopItemType>(PropertyInfo p, ShopItemType item)
        {
            string value = "";

            try
            {
                value = p.GetValue(item, null)?.ToString();
                if (value == null) return "NULL";  // Deal with nulls
                if (value.Trim().Length == 0) return ""; // Deal with spaces and blanks

                // Guard strings with "s, they may contain the delimiter!
                if (p.PropertyType == typeof(string))
                {
                    value = string.Format("\"{0}\"", value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        string ExportToCsv<ShopItemType>(string savePath, List<ShopItemType> items)
        {
            if (File.Exists(savePath))
            {
                throw new FileAlreadyExists(savePath);
                return "";
            }
            else
            {
                string delimiter = ";";
                Type itemType = typeof(ShopItemType);
                var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

                var csv = new StringBuilder();

                // Write Headers
                csv.AppendLine(string.Join(delimiter, props.Select(p => p.Name)));

                // Write Rows
                foreach (var item in items)
                {
                    // Write Fields
                    csv.AppendLine(string.Join(delimiter, props.Select(p => GetCsvFieldasedOnValue(p, item))));
                }

                FileStream file = File.Create(savePath);
                StreamWriter writer = new StreamWriter(file);
                Console.WriteLine(csv.ToString());
                writer.Write(csv.ToString());
                writer.Close();
                file.Close();

                return file.Name;

            }
        }

        string ExportToXml<ShopItemType>(string savePath, List<ShopItemType> items)
        {
            if (File.Exists(savePath))
            {
                throw new FileAlreadyExists(savePath);
                return "";
            } else
            {
                XmlSerializer writer = new XmlSerializer(typeof(List<ShopItemType>));
                FileStream file = File.Create(savePath);
                writer.Serialize(file, items);
                file.Close();

                string pathToFile = file.Name;

                return pathToFile;
            }
        }

        void ExportProductsToXml() {
            string answer = "";
            do {
                Console.WriteLine("Import file");
                BackToMenuMessage();
                Console.Write("Write path to folder where you wanna save your file: ");
                answer = Console.ReadLine();
                string pathToFolder = "";
                pathToFolder = answer;

                if (!Directory.Exists(pathToFolder))
                {
                    Console.WriteLine("Directory doesn't exists");
                    PressAnyKeyMsg();
                    continue;
                }

                Console.Write("Write file name: ");
                answer = Console.ReadLine();
                string fileName = answer;

                try
                {
                    Console.WriteLine("Importing...");
                    string pathToFile = ExportToXml<Product>($"{pathToFolder}\\{fileName}.xml", products);
                    Console.WriteLine("Imported completed. Path to file is: " + pathToFile);
                    PressAnyKeyMsg();
                    break;
                } catch(FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                
            } while (answer != Commands.Back);
            return;
            /*XmlSerializer writer = new XmlSerializer(typeof(List<Product>));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializedProducts.xml";
            FileStream file = File.Create(path);
            writer.Serialize(file, products);
            file.Close();*/
        }

        void ExportProductsToCsv()
        {
            string answer = "";
            do
            {
                Console.WriteLine("Export file");
                BackToMenuMessage();
                Console.Write("Write path to folder where you wanna save your file: ");
                answer = Console.ReadLine();
                string pathToFolder = "";
                pathToFolder = answer;

                if (!Directory.Exists(pathToFolder))
                {
                    Console.WriteLine("Directory doesn't exists");
                    PressAnyKeyMsg();
                    continue;
                }

                Console.Write("Write file name: ");
                answer = Console.ReadLine();
                string fileName = answer;

                try
                {
                    Console.WriteLine("Exporting...");
                    string pathToFile = ExportToCsv<Product>($"{pathToFolder}\\{fileName}.csv", products);
                    Console.WriteLine("Imported completed. Path to file is: " + pathToFile);
                    PressAnyKeyMsg();
                    break;
                }
                catch (FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }

            } while (answer != Commands.Back);
            return;
            /*XmlSerializer writer = new XmlSerializer(typeof(List<Product>));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializedProducts.xml";
            FileStream file = File.Create(path);
            writer.Serialize(file, products);
            file.Close();*/
        }

        void ImportProductsFromXml()
        {
            BackToMenuMessage();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
            Console.WriteLine("test import");
            using (FileStream stream = File.OpenRead(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializedProducts.xml"))
            {
                List<Product> dezerializedList = (List<Product>)serializer.Deserialize(stream);
                products = dezerializedList;
                foreach(Product product in products)
                {
                    if (!IsCategoryExists(product.Category))
                    {
                        categories.Add(product.Category);
                        Console.WriteLine("Product category was created");
                    }   
                }
                UpdateLastCategoryId();
                UpdateLastProductId();
            }
            return;
        }

        bool IsCategoryExists(int id)
        {
            bool isExists = false;
            foreach(Category category in categories)
            {
                if (category.Id == id) isExists = true;
            }
            return isExists;
        }

        bool IsCategoryExists(Category category)
        {
            return categories.Contains(category);
        }

        void BackToMenuMessage()
        {
            Console.WriteLine("return to main: [back]");
        }

        void EditCategory()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are editing the product");
                PrintAllCategories();
                Console.Write("Chooise category id to display modify(edit)");
                BackToMenuMessage();
                answer = Console.ReadLine().ToLower();

                if (IsValidId(answer))
                {
                    int categoryId = int.Parse(answer);
                    int categoryIndex = categories.FindIndex(category => category.Id == categoryId);
                    Category category = categories.Find(category => category.Id == categoryId);
                    if (categoryIndex != -1)
                    {
                        Console.WriteLine("Current category:");
                        Console.WriteLine($"{category.ToString()}");
                        Console.WriteLine("Products in category: ");
                        Console.Write("Write new category name: ");
                        category.Name = Console.ReadLine();
                        Console.WriteLine("Category is updated:");
                        Console.WriteLine($"{category.ToString()}");
                        foreach (Product product in products)
                        {
                            if (product.Category != null && product.Category.Id == categoryId)
                            {
                                product.Category = category;
                            }
                        }
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
                    Console.WriteLine("Wrong category id. Category id must be numeric");
                }
            } while (answer != Commands.Back);
            return;
        }

        public void run()
        {

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome To The Shop");
                Console.WriteLine("-------------------------------");

                Console.WriteLine($"LOAD TEST DATA: [init]");

                Console.WriteLine($"Add category: [{Commands.AddCategory}]");
                Console.WriteLine($"Delete category: [{Commands.DeleteCategory}]");
                Console.WriteLine($"Edit category: [{Commands.EditCategory}]");
                Console.WriteLine($"Print all categories: [{Commands.PrintAllCategories}]");
                Console.WriteLine($"Print one category: [{Commands.PrintOneCategory}]");

                Console.WriteLine($"Add product: [{Commands.AddProduct}]");
                Console.WriteLine($"Delete product: [{Commands.DeleteProduct}]");
                Console.WriteLine($"Edit product: [{Commands.EditProduct}]");
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
                    // TEST DATA, todo: delete
                    case "init":
                        Category category1 = new Category(1, "Test cat 1");
                        Category category2 = new Category(2, "Test cat 2");
                        Category category3 = new Category(3, "Test cat 3");
                        Category category4 = new Category(4, "Test cat 4");
                        Category category5 = new Category(5, "Test cat 5");
                        categories.Add(category1);
                        categories.Add(category2);
                        categories.Add(category3);
                        categories.Add(category4);
                        categories.Add(category5);

                        Product product1 = new Product(1, "Test prod 1", "666", category1);
                        Product product2 = new Product(2, "Test prod 2", "1345", category2);
                        Product product3 = new Product(3, "Test prod 3", "346", category3);
                        Product product4 = new Product(4, "Test prod 4", "754", category4);
                        Product product5 = new Product(5, "Test prod 5", "965", null);
                        Product product6 = new Product(5, "Test prod 6", "315", category3);
                        Product product7 = new Product(5, "Test prod 7", "618", category3);
                        Product product8 = new Product(5, "Test prod 8", "234", category3);
                        products.Add(product1);
                        products.Add(product2);
                        products.Add(product3);
                        products.Add(product4);
                        products.Add(product5);
                        products.Add(product6);
                        products.Add(product7);
                        products.Add(product8);

                        break;
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
                    //Edit category
                    case Commands.EditCategory:
                        Console.Clear();
                        EditCategory();
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
                        Console.Clear();
                        PrintOneCategory();
                        Console.ReadKey();
                        break;
                    //Add product
                    case Commands.AddProduct:
                        Console.Clear();
                        AddProduct();
                        Console.WriteLine("New product was created. Press any key to return to main menu");
                        Console.ReadKey();
                        break;
                    //Delete product
                    case Commands.DeleteProduct:
                        DeleteProduct();
                        break;
                    //Edit product
                    case Commands.EditProduct:
                        Console.Clear();
                        //EditProduct();
                        Console.ReadKey();
                        break;
                    //Print all products
                    case Commands.PrintAllProducts:
                        Console.Clear();
                        PrintAllProducts();
                        Console.ReadKey();
                        break;
                    //Print one product
                    case Commands.PrintOneProduct:
                        Console.Clear();
                        PrintOneProduct();
                        Console.ReadKey();
                        break;
                    //Export products to csv file
                    case Commands.ExportProductsToCsv:
                        Console.Clear();
                        ExportProductsToCsv();
                        Console.ReadKey();
                        break;
                    //Export products to xml file
                    case Commands.ExportProductsToXml:
                        Console.Clear();
                        ExportProductsToXml();
                        Console.ReadKey();
                        break;
                    //Export categories to csv file
                    case Commands.ExportCategoriesToCsv:
                        break;
                    //Export categories to xml file
                    case Commands.ExportCategoriesToXml:
                        break;
                    //Import products to csv file
                    case Commands.ImportProductsFromCsv:
                        break;
                    //Import products to xml file
                    case Commands.ImportProductsFromXml:
                        Console.Clear();
                        ImportProductsFromXml();
                        Console.ReadKey();
                        break;
                    //Import categories to csv file
                    case Commands.ImportCategoriesFromCsv:
                        break;
                    //Import categories to xml file
                    case Commands.ImportCategoriesFromXml:
                        break;
                }
            } while (answer != "q");
        }
    }
}
