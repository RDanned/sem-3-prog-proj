using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    internal class Category: ShopItem
    {
        public string ToString()
        {
            return $"#{this.Id}: {this.Name}";
        }
    }
}
