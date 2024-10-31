using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiemtra_31_10
{
    internal class ShoppingCart
    {
        // Danh sách các sản phẩm trong giỏ hàng
        public List<Product> Products { get; set; } = new List<Product>();

        // Thêm sản phẩm vào giỏ hàng
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        // Tính tổng số lượng sản phẩm trong giỏ hàng
        public int GetTotalQuantity()
        {
            return Products.Sum(p => p.Quantity);
        }

        // Tính tổng giá trị của giỏ hàng
        public decimal GetTotalAmount()
        {
            return Products.Sum(p => p.Price * p.Quantity);
        }

        // Xóa tất cả sản phẩm trong giỏ hàng
        public void ClearCart()
        {
            Products.Clear();
        }
    }
}
