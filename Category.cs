using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    public class Category: ShopItem
    {
        public Category()
        {

        }
        public Category (int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public string ToString()
        {
            return $"#{this.Id}: {this.Name}";
        }
    }
}
