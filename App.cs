using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;


namespace SemestralProject
{
    //Commands of available actions in app
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
        //List with categories
        List<Category> categories = new List<Category>();
        //List with products
        List<Product> products = new List<Product>();
        int lastCategoryId = 0;
        int lastProductId = 0;

        string answer;

        //Prints press any key message
        void PressAnyKeyMsg()
        {
            Console.WriteLine("Press any key to continue...");
        }

        //Updates last category id to set it in later added products
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

        //Updates last product id to set it in later added products
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

        //Starts dialog to add category
        void AddCategory()
        {
            Console.WriteLine("You are creating new category");
            Category category = new Category();
            Console.Write("Name it: ");
            category.Name = Console.ReadLine();
            category.Id = lastCategoryId++;
            categories.Add(category);
            Console.WriteLine("Category is created: " + category.ToString());
            return;
        }

        //Starts dialog to delete category
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
                if (answer == Commands.Back) break;

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


        //Starts dialog to add product
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
                        Console.WriteLine(category);
                        if (category != null)
                        {
                            product.Category = category;
                        } else
                        {
                            Console.WriteLine("Category doesn't exists");
                            continue;
                        }
                    } else
                    {
                        Console.WriteLine("Category doesn't exists");
                        continue;
                    }
                }

                products.Add(product);
                Console.WriteLine($"New product was created: {product.ToString()}");
                break;
                return;
            } while (answer != Commands.Back);
            return;
        }

        //Starts dialog to delete product
        void DeleteProduct()
        {

            string answer = "";
            do
            {
                Console.WriteLine("You are deleting a product");
                BackToMenuMessage();
                Console.WriteLine("Available products:");
                PrintAllProducts();
                Console.Write("Chooise product id to delete: ");
                answer = Console.ReadLine().ToLower();

                if (answer == Commands.Back) break;

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
                        Console.WriteLine("Product doesn't exists");
                        continue;
                    }
                } else
                {
                    Console.WriteLine("Wrong product id. Product id must be numeric");
                }
            } while (answer != Commands.Back);
            return;
        }

        //Prints all products
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

        //PRints one product by id
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

        //PRints one product by id
        void PrintOneProduct()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are printing a product");
                BackToMenuMessage();
                Console.Write("Chooise product id to print: ");

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

        //Prints all categories with availability message
        void PrintAvailableCategories()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Available categories: ");
            PrintAllCategories();
            Console.WriteLine("-------------------------------");
        }

        //Prints all categories
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

        //Check, if string id is number
        bool IsValidId(string id)
        {
            return int.TryParse(id, out _);
        }


        //Prints one category by id
        void PrintOneCategory()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are printing the category");
                BackToMenuMessage();
                PrintAllCategories();
                Console.Write("Chooise category id to display category: ");

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

        //Command to help to translate object to csv string
        private static object GetCsvFieldasedOnValue<ShopItemType>(PropertyInfo p, ShopItemType item)
        {
            string value = "";

            try
            {
                value = p.GetValue(item, null)?.ToString();
                if (value == null) return "NULL";  // Deal with nulls
                if (value.Trim().Length == 0) return ""; // Deal with spaces and blanks

                // Guard strings with "s, they may contain the delimiter!
                /*if (p.PropertyType == typeof(string))
                {
                    value = string.Format("\"{0}\"", value);
                }*/

                if (p.PropertyType == typeof(Category))
                {
                    Category category = p.GetValue(item) as Category;
                    value = $"{category.Id}|{category.Name}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        //Starts dialog to export file to csv
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

        //Additional command that exports categories or products to xml
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

        //Starts dialog export products to xml
        void ExportProductsToXml() {
            string answer = "";
            do {
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
                    string pathToFile = ExportToXml<Product>($"{pathToFolder}\\{fileName}.xml", products);
                    Console.WriteLine("Export completed. Path to file is: " + pathToFile);
                    break;
                } catch(FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                
            } while (answer != Commands.Back);
            return;
        }

        //Starts dialog export categories to xml
        void ExportCategoriesToXml()
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
                    string pathToFile = ExportToXml<Category>($"{pathToFolder}\\{fileName}.xml", categories);
                    Console.WriteLine("Export completed. Path to file is: " + pathToFile);
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
        }

        //Starts dialog export products or categories to csv
        void ExportShopItemToCsvDialog<ShopItemType>(List<ShopItemType> items)
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
                    string pathToFile = ExportToCsv<ShopItemType>($"{pathToFolder}\\{fileName}.csv", items);
                    Console.WriteLine("Export completed. Path to file is: " + pathToFile);
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
        }

        /*void ExportProductsToCsv()
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
                    Console.WriteLine("Export completed. Path to file is: " + pathToFile);
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
        }

        void ExportCategoriesToCsv()
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
                    string pathToFile = ExportToCsv<Category>($"{pathToFolder}\\{fileName}.csv", categories);
                    Console.WriteLine("Export completed. Path to file is: " + pathToFile);
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
        }*/

        //Starts dialog to import products from xml file
        void ImportProductsFromXml()
        {
            string answer = "";
            do
            {
                Console.WriteLine("Import file");
                BackToMenuMessage();
                Console.Write("Write path to folder where is located your file: ");
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
                    BackToMenuMessage();
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
                    Console.WriteLine("Importing...");
                    using (FileStream stream = File.OpenRead($"{pathToFolder}\\{fileName}.xml"))
                    {
                        List<Product> dezerializedList = (List<Product>)serializer.Deserialize(stream);
                        products = dezerializedList;
                        foreach (Product product in products)
                        {
                            if (!IsCategoryExists(product.Category))
                            {
                                categories.Add(product.Category);
                                Console.WriteLine("Product category was created");
                            }
                        }
                        Console.WriteLine("Imported completed");
                        UpdateLastCategoryId();
                        UpdateLastProductId();
                    }
                    return;
                }
                catch (FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }

            } while (answer != Commands.Back);
            return;
        }

        //Starts dialog to import categories from xml file
        void ImportCategoriessFromXml()
        {
            string answer = "";
            do
            {
                Console.WriteLine("Import file");
                BackToMenuMessage();
                Console.Write("Write path to folder where is located your file: ");
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
                    BackToMenuMessage();
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Category>));
                    Console.WriteLine("Importing...");
                    using (FileStream stream = File.OpenRead($"{pathToFolder}\\{fileName}.xml"))
                    {
                        List<Category> dezerializedList = (List<Category>)serializer.Deserialize(stream);
                        categories = dezerializedList;
                        Console.WriteLine("Imported completed");
                        UpdateLastCategoryId();
                        UpdateLastProductId();
                    }
                    return;
                }
                catch (FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }

            } while (answer != Commands.Back);
            return;
        }

        //Starts dialog to import products from csv file
        void ImportProductsFromCsv()
        {
            string answer = "";
            do
            {
                Console.WriteLine("Import file");
                BackToMenuMessage();
                Console.Write("Write path to folder where is located your file: ");
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
                    BackToMenuMessage();
                    Console.WriteLine("Importing...");
                    List<Product> newProducts = new List<Product>();
                    List<Category> newCategories = new List<Category>();
                    using (FileStream fileStream = File.OpenRead($"{pathToFolder}\\{fileName}.csv"))
                    {
                        const Int32 BufferSize = 128;
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                        {
                            String line;
                            int lineNumber = 0;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                if (lineNumber == 0)
                                {
                                    lineNumber++;
                                    continue;
                                }

                                if(line.Split(";").Length != 4)
                                {
                                    throw new WrongFormat("Wrong delimiter or columns count");
                                    break;
                                    return;
                                }

                                string[] parts = new string[4];
                                parts = line.Split(";");

                                int productId;
                                if (!int.TryParse(parts[1], out productId))
                                {
                                    throw new WrongFormat("Cannot convert column with product id to integer");
                                    break;
                                    return;
                                }

                                Product product = new Product();
                                product.Id = productId;
                                product.Name = parts[2];
                                product.Price = parts[3];

                                if (parts[0] != "" && parts[0] != "NULL")
                                {
                                    if (!parts[0].Contains("|"))
                                    {
                                        throw new WrongFormat("Category columns doesnt contain delimiter");
                                        break;
                                        return;
                                    }

                                    if (parts[0].Split("|").Length != 2)
                                    {
                                        throw new WrongFormat("Category column contains too much or less fields");
                                        break;
                                        return;
                                    }

                                    string[] categoryColumn = new string[2];
                                    categoryColumn = parts[0].Split("|");
                                    int categoryId;

                                    if (!int.TryParse(categoryColumn[0], out categoryId))
                                    {
                                        throw new WrongFormat("Cannot convert category id to integer");
                                        break;
                                        return;
                                    }

                                    Category category = new Category();
                                    category.Id = categoryId;
                                    category.Name = categoryColumn[1];
                                    product.Category = category;
                                    newCategories.Add(category);
                                }

                                newProducts.Add(product);
                                Console.WriteLine(line);
                                lineNumber++;
                            }
                        }
                        products = newProducts;
                        categories = newCategories;
                        UpdateLastCategoryId();
                        UpdateLastProductId();
                    }
                    return;
                }
                catch (FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }

            } while (answer != Commands.Back);
            return;
        }

        //Starts dialog to import categories from csv file
        void ImportCategoriesFromCsv()
        {
            string answer = "";
            do
            {
                Console.WriteLine("Import file");
                BackToMenuMessage();
                Console.Write("Write path to folder where is located your file: ");
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
                    BackToMenuMessage();
                    Console.WriteLine("Importing...");
                    List<Category> newCategories = new List<Category>();
                    using (FileStream fileStream = File.OpenRead($"{pathToFolder}\\{fileName}.csv"))
                    {
                        const Int32 BufferSize = 128;
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                        {
                            String line;
                            int lineNumber = 0;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                if (lineNumber == 0)
                                {
                                    lineNumber++;
                                    continue;
                                }

                                if (line.Split(";").Length != 2)
                                {
                                    throw new WrongFormat("Wrong delimiter or columns count");
                                    break;
                                    return;
                                }

                                string[] parts = new string[4];
                                parts = line.Split(";");

                                int categoryId;
                                if (!int.TryParse(parts[0], out categoryId))
                                {
                                    throw new WrongFormat("Cannot convert column with category id to integer");
                                    break;
                                    return;
                                }

                                Category category = new Category();
                                category.Id = categoryId;
                                category.Name = parts[1];

                                Console.WriteLine(line);
                                lineNumber++;
                            }
                        }
                        categories = newCategories;
                        UpdateLastCategoryId();
                        UpdateLastProductId();
                    }
                    return;
                }
                catch (FileAlreadyExists e)
                {
                    Console.WriteLine($"An error was occured: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    PressAnyKeyMsg();
                    continue;
                }

            } while (answer != Commands.Back);
            return;
        }

        //Command to check if category exists
        bool IsCategoryExists(int id)
        {
            bool isExists = false;
            foreach(Category category in categories)
            {
                if (category.Id == id) isExists = true;
            }
            return isExists;
        }

        //Command to check if category exists
        bool IsCategoryExists(Category category)
        {
            return categories.Contains(category);
        }

        //Command to display information message about returing to main menu
        void BackToMenuMessage()
        {
            Console.WriteLine("return to main: [back]");
        }

        //Starts dialog to edit category
        void EditCategory()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are editing the category");
                BackToMenuMessage();
                PrintAllCategories();
                Console.Write("Chooise category id to modify(edit): ");
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

        //Starts dialog to edit product
        void EditProduct()
        {
            string answer = "";
            do
            {
                Console.WriteLine("You are editing the product");
                BackToMenuMessage();
                PrintAllProducts();
                Console.Write("Chooise product id to modify(edit): ");
                answer = Console.ReadLine().ToLower();

                if (IsValidId(answer))
                {
                    int productId = int.Parse(answer);
                    int productIndex = products.FindIndex(product => product.Id == productId);
                    Product product = products.Find(product => product.Id == productId);
                    if (productIndex != -1)
                    {
                        Console.WriteLine("Current product:");
                        Console.WriteLine($"{product.ToString()}");

                        Console.Write("Write new product name: ");
                        product.Name = Console.ReadLine();
                        Console.Write("Write new product price: ");
                        product.Price = Console.ReadLine();

                        
                        string categoryChangeAnswer = "";
                        do
                        {
                            Console.Write("Do you want to change category?[yes][no]");
                            categoryChangeAnswer = Console.ReadLine();

                            if(categoryChangeAnswer == "yes") {
                                Console.WriteLine("Available categories: ");
                                PrintAllCategories();
                                Console.Write("Choose category id: ");
                                answer = Console.ReadLine().ToLower();
                                int categoryId;
                                bool isNumeric = int.TryParse(answer, out categoryId);
                                if (isNumeric)
                                {
                                    Category category = categories.Find(category => category.Id == categoryId);
                                    if (category != null)
                                    {
                                        product.Category = category;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Category doesn't exists");
                                        continue;
                                    }
                                }
                                Console.WriteLine("Category is updated");
                                break;
                            } if (categoryChangeAnswer == "no") {
                                break;
                            } else {
                                continue;
                            }
                        }
                        while (categoryChangeAnswer != "yes" || categoryChangeAnswer != "noe") ;


                        Console.WriteLine("Product is updated:");
                        Console.WriteLine($"{product.ToString()}");

                        products[productIndex] = product;
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

        //Run app
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
                Console.WriteLine($"Import categories from xml: [{Commands.ImportCategoriesFromXml}]");

                Console.WriteLine($"Quit app: [q]");

                Console.Write("Choose action: ");
                answer = Console.ReadLine().ToLower();

                switch (answer)
                {
                    // TEST DATA
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
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Delete category
                    case Commands.DeleteCategory:
                        Console.Clear();
                        DeleteCategory();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Edit category
                    case Commands.EditCategory:
                        Console.Clear();
                        EditCategory();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Print all categories
                    case Commands.PrintAllCategories:
                        Console.Clear();
                        PrintAllCategories();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Print one category
                    case Commands.PrintOneCategory:
                        Console.Clear();
                        PrintOneCategory();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Add product
                    case Commands.AddProduct:
                        Console.Clear();
                        AddProduct();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Delete product
                    case Commands.DeleteProduct:
                        Console.Clear();
                        DeleteProduct();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Edit product
                    case Commands.EditProduct:
                        Console.Clear();
                        EditProduct();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Print all products
                    case Commands.PrintAllProducts:
                        Console.Clear();
                        PrintAllProducts();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Print one product
                    case Commands.PrintOneProduct:
                        Console.Clear();
                        PrintOneProduct();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Export products to csv file
                    case Commands.ExportProductsToCsv:
                        Console.Clear();
                        ExportShopItemToCsvDialog<Product>(products);
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Export products to xml file
                    case Commands.ExportProductsToXml:
                        Console.Clear();
                        ExportProductsToXml();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Export categories to csv file
                    case Commands.ExportCategoriesToCsv:
                        Console.Clear();
                        ExportShopItemToCsvDialog<Category>(categories);
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Export categories to xml file
                    case Commands.ExportCategoriesToXml:
                        Console.Clear();
                        ExportCategoriesToXml();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Import products to csv file
                    case Commands.ImportProductsFromCsv:
                        Console.Clear();
                        ImportProductsFromCsv();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Import products to xml file
                    case Commands.ImportProductsFromXml:
                        Console.Clear();
                        ImportProductsFromXml();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Import categories to csv file
                    case Commands.ImportCategoriesFromCsv:
                        Console.Clear();
                        ImportCategoriesFromCsv();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    //Import categories to xml file
                    case Commands.ImportCategoriesFromXml:
                        Console.Clear();
                        ImportCategoriessFromXml();
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Action doesn't exist");
                        PressAnyKeyMsg();
                        Console.ReadKey();
                        break;
                }
            } while (answer != "q");
        }
    }
}
