using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kiemtra_31_10
{
    internal class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }  

        public Product(string name, decimal price, int quantity, string imagePath)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            ImagePath = imagePath;
        }

        public override string ToString()
        {
            return $"{Name} - Giá: {Price:C} - Số lượng: {Quantity}";
        }
    }
}
