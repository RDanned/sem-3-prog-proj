using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    internal class Product: ShopItem
    {
        private string price = "";
        private List<Category> categories = new List<Category>();
        private Category category = new Category();

        public string Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public List<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public Category Category
        {
            get { return this.category; }
            set { this.category = value; }
        }

        public string toString()
        {
            return $"#{this.Id}: {this.Name}";
        }
    }
}
