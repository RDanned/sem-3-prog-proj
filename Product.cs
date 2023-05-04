using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    public class Product: ShopItem
    {
        private string price = "";
        private Category category = new Category();

        public Product()
        {

        }

        public Product(int id, string name, string price, Category category)
        {
            this.id = id;
            this.price = price;
            this.category = category;   
            this.name = name;
        }

        public string Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public Category Category
        {
            get { return this.category; }
            set { this.category = value; }
        }

        public string ToString()
        {
            return $"#{this.Id}: {this.Name}";
        }
    }
}
