using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kiemtra_31_10
{
    public partial class Form1 : Form
    {
        private ShoppingCart shoppingCart = new ShoppingCart();
        public Form1()
        {
            InitializeComponent();
            InitializeProductList();
        }
        private void InitializeProductList()
        {
            dgvview.Rows.Add("Sản phẩm A", "1000", "10");
            dgvview.Rows.Add("Sản phẩm B", "2000", "15");
            dgvview.Rows.Add("Sản phẩm C", "1500", "20");
        }
        private List<Product> productsList;

        private void LoadProducts()
        {
            var product1 = new Product("Sản phẩm A", 1000, 9, "Images/product1.jpg");
            var product2 = new Product("Sản phẩm B", 2000, 15, "Images/product2.jpg");
            var product3 = new Product("Sản phẩm C", 1500, 20, "Images/product3.jpg");

            AddProductToDataGridView(product1);
            AddProductToDataGridView(product2);
            AddProductToDataGridView(product3);
        }

        private void AddProductToDataGridView(Product product)
        {
            dgvview.Rows.Add(product.Name, product.Price, product.Quantity, product.ImagePath);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn trong danh sách sản phẩm không
            if (dgvview.SelectedRows.Count > 0)
            {
                var selectedRow = dgvview.SelectedRows[0];
                var productName = selectedRow.Cells[0].Value?.ToString() ?? "";
                var productPrice = selectedRow.Cells[1].Value?.ToString() ?? "";
                int availableQuantity = int.Parse(selectedRow.Cells[2].Value?.ToString() ?? "0");
                int quantityToAdd = 1; // Số lượng mặc định khi thêm vào giỏ hàng

                // Kiểm tra nếu số lượng còn đủ để thêm vào giỏ hàng
                if (availableQuantity >= quantityToAdd)
                {
                    bool productExists = false;

                    // Giảm số lượng trong dgvview
                    selectedRow.Cells[2].Value = availableQuantity - quantityToAdd;

                    // Thêm hoặc cập nhật sản phẩm trong dgvGioHang
                    foreach (DataGridViewRow row in dgvGioHang.Rows)
                    {
                        if (row.Cells["colProductName"].Value != null && row.Cells["colProductName"].Value.ToString() == productName)
                        {
                            int currentQuantity = int.Parse(row.Cells["colQuantity"].Value?.ToString() ?? "0");
                            row.Cells["colQuantity"].Value = currentQuantity + quantityToAdd;
                            productExists = true;
                            break;
                        }
                    }

                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm dòng mới
                    if (!productExists)
                    {
                        dgvGioHang.Rows.Add(productName, productPrice, quantityToAdd);
                    }
                }
                else
                {
                    MessageBox.Show("Không đủ số lượng sản phẩm trong kho!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn trong giỏ hàng không
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                // Lấy thông tin sản phẩm được chọn trong giỏ hàng
                var selectedRow = dgvGioHang.SelectedRows[0];
                var productName = selectedRow.Cells["colProductName"].Value?.ToString() ?? "";
                int currentQuantityInCart = int.Parse(selectedRow.Cells["colQuantity"].Value?.ToString() ?? "0");

                // Giảm số lượng trong giỏ hàng
                if (currentQuantityInCart > 1)
                {
                    // Giảm số lượng đi 1
                    selectedRow.Cells["colQuantity"].Value = currentQuantityInCart - 1;
                }
                else
                {
                    // Nếu số lượng là 1, xóa dòng khỏi giỏ hàng
                    dgvGioHang.Rows.Remove(selectedRow);
                }

                // Cập nhật lại số lượng trong danh sách sản phẩm (dgvview)
                foreach (DataGridViewRow row in dgvview.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == productName)
                    {
                        // Cộng lại số lượng vào dgvview
                        int currentQuantityInStock = int.Parse(row.Cells[2].Value?.ToString() ?? "0");
                        row.Cells[2].Value = currentQuantityInStock + 1;
                        break;
                    }
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem giỏ hàng có sản phẩm nào không
            if (dgvGioHang.Rows.Count > 0)
            {
                // Tính tổng giá trị đơn hàng
                decimal totalAmount = 0;
                foreach (DataGridViewRow row in dgvGioHang.Rows)
                {
                    string priceText = row.Cells["colPrice"].Value?.ToString() ?? "0";
                    string quantityText = row.Cells["colQuantity"].Value?.ToString() ?? "0";

                    decimal price = decimal.Parse(priceText);
                    int quantity = int.Parse(quantityText);
                    totalAmount += price * quantity;
                }

                // Hiển thị thông báo xác nhận và tổng giá trị đơn hàng
                DialogResult result = MessageBox.Show($"Tổng giá trị đơn hàng là {totalAmount:C}. Bạn có muốn thanh toán không?",
                                                      "Xác nhận thanh toán",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                // Nếu người dùng chọn Yes, tiến hành thanh toán
                if (result == DialogResult.Yes)
                {
                    // Xóa giỏ hàng
                    dgvGioHang.Rows.Clear();

                    // Cập nhật lại giao diện tổng số lượng và tổng giá trị
                    lblTongSoLuong.Text = "Tổng số lượng: 0";
                    lblTongGiaTri.Text = "Tổng giá trị: 0 đ";

                    // Thông báo hoàn tất
                    MessageBox.Show("Đơn hàng của bạn đã được thanh toán thành công!", "Thanh toán thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Giỏ hàng hiện đang trống, không có sản phẩm để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void UpdateTotalValues(DataGridView dataGridView)
        {
            int totalQuantity = 0;
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Số lượng"].Value != null && row.Cells["Giá"].Value != null)
                {
                    int quantity = int.Parse(row.Cells["Số lượng"].Value.ToString());
                    decimal price = decimal.Parse(row.Cells["Giá"].Value.ToString());
                    totalQuantity += quantity;
                    totalAmount += quantity * price;
                }
            }

            lblTongSoLuong.Text = $"Tổng số lượng: {totalQuantity}";
            lblTongGiaTri.Text = $"Tổng giá trị: {totalAmount:C}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Liên kết sự kiện SelectionChanged cho dgvview và dgvGioHang
            dgvview.SelectionChanged += DataGridView_SelectionChanged;
            dgvGioHang.SelectionChanged += DataGridView_SelectionChanged;
        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = sender as DataGridView;

            if (selectedDataGridView != null && selectedDataGridView.SelectedRows.Count > 0)
            {
                // Lấy dòng đầu tiên được chọn
                DataGridViewRow selectedRow = selectedDataGridView.SelectedRows[0];

                // Lấy đường dẫn hình ảnh từ sản phẩm và hiển thị trong PictureBox
                string imagePath = selectedRow.Cells["ImagePath"].Value?.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    picProductImage.Image = Image.FromFile(imagePath);
                }
                else
                {
                    picProductImage.Image = null; // Nếu không có hình ảnh, xóa hình hiện tại
                }

                // Hiển thị thông tin số lượng và giá trị trên các Label
                int quantity = int.Parse(selectedRow.Cells["Số lượng"].Value?.ToString() ?? "0");
                decimal price = decimal.Parse(selectedRow.Cells["Giá"].Value?.ToString() ?? "0");

                lblTongSoLuong.Text = $"Số lượng: {quantity}";
                lblTongGiaTri.Text = $"Giá trị: {price * quantity:C}";
            }
        }
        private void dgvview_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvview.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn
                var selectedRow = dgvview.SelectedRows[0];

                // Lấy tên sản phẩm từ dòng đã chọn
                string productName = selectedRow.Cells["Tên sản phẩm"].Value.ToString();

                // Tìm sản phẩm trong danh sách sản phẩm dựa trên tên
                var product = productsList.FirstOrDefault(p => p.Name == productName);

                // Hiển thị hình ảnh của sản phẩm nếu tìm thấy
                if (product != null && !string.IsNullOrEmpty(product.ImagePath))
                {
                    picProductImage.Image = Image.FromFile(product.ImagePath);
                }
            }
        }



        private void dgvGioHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTongSoLuong_Click(object sender, EventArgs e)
        {

        }
    }
}
