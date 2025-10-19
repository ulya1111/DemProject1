using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemProject1
{
    public partial class FormMain : Form
    {
        private string role;
        private List<Product> products = new List<Product>();
        private List<Product> cart = new List<Product>();
        

        public FormMain(string userRole)
        {
            InitializeComponent();
            role = userRole;
            SetupInterface();
            LoadProducts();
            DisplayProducts();
        }

        private void SetupInterface()
        {
            if (role == "Manager" || role == "Admin")
                buttonEditProducts.Visible = true;
            else
                buttonEditProducts.Visible = false;
           

            if (role != "Admin")
                panelTools.Visible = false; // Панель сортировки только для Admin
            if (role == "Admin") label2.Text = "Вы зашли как Admin";
            else if (role == "Manager") label2.Text = "Вы зашли как Manager";
            else if (role == "User") label2.Text = "Вы зашли как User";
        }

        private void LoadProducts()
        {
            products = new List<Product>
            {
                new Product("Nike Air Zoom", 2000, 42, "nike.jpg"),
                new Product("Adidas Run", 1800, 43, "adidas.jpg"),
                new Product("Converse Classic", 1200, 41, "converse.jpg"),
                new Product("Puma SoftFoam", 2300, 44, "puma.jpg"),
            };
        }

        private void DisplayProducts(IEnumerable<Product> list = null)
        {
            var data = list ?? products;
            flowLayoutPanelProducts.Controls.Clear();

            foreach (var p in data)
            {
                Panel card = new Panel
                {
                    Width = 280,
                    Height = 200,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = p.Price > 1500 ? Color.MistyRose : Color.WhiteSmoke,
                    Margin = new Padding(10)
                };

                // Фото
                PictureBox pic = new PictureBox
                {
                    Width = 80,
                    Height = 80,
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                try { pic.Load(p.ImagePath); } catch { } // если файла нет

                // Название
                Label lblName = new Label
                {
                    Text = p.Name,
                    Location = new Point(100, 10),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true
                };

                // Цена
                Label lblPrice = new Label { Location = new Point(100, 40), AutoSize = true };

                if (p.Price > 1500)
                {
                    decimal newPrice = p.Price * 0.85m;
                    lblPrice.Text = $"Цена: {newPrice} руб.";

                    Label lblOld = new Label
                    {
                        Text = $"{p.Price} руб.",
                        Font = new Font("Segoe UI", 9, FontStyle.Strikeout),
                        ForeColor = Color.Gray,
                        Location = new Point(100, 60),
                        AutoSize = true
                    };
                    card.Controls.Add(lblOld);
                }
                else lblPrice.Text = $"Цена: {p.Price} руб.";

                // Размер
                Label lblSize = new Label
                {
                    Text = $"Размер: {p.Size}",
                    Location = new Point(100, 80),
                    AutoSize = true
                };

                // Кнопки
                if (role == "User")
                {
                    Button btnAdd = new Button
                    {
                        Text = "Добавить в корзину",
                        Location = new Point(100, 120),
                        Width = 150
                    };
                    btnAdd.Click += (s, e) => cart.Add(p);
                    card.Controls.Add(btnAdd);
                }

                if (role == "Manager" || role == "Admin")
                {
                    Button btnEdit = new Button
                    {
                        Text = "Редактировать",
                        Location = new Point(100, 120),
                        Width = 150
                    };
                    btnEdit.Click += (s, e) => MessageBox.Show($"Редактирование {p.Name}");
                    card.Controls.Add(btnEdit);
                }

                // Добавляем всё
                card.Controls.Add(pic);
                card.Controls.Add(lblName);
                card.Controls.Add(lblPrice);
                card.Controls.Add(lblSize);
                flowLayoutPanelProducts.Controls.Add(card);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCart cartForm = new FormCart(cart);
            cartForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();   
        }

        private void buttonApplySort_Click(object sender, EventArgs e)
        {
            string selected = comboBoxSort.Text;
            IEnumerable<Product> sorted = products;

            if (selected == "По цене ↑") sorted = products.OrderBy(p => p.Price);
            else if (selected == "По цене ↓") sorted = products.OrderByDescending(p => p.Price);

            DisplayProducts(sorted);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string query = textBoxSearch.Text.ToLower();
            var filtered = products.Where(p => p.Name.ToLower().Contains(query));
            DisplayProducts(filtered);
        }

        public class Product
        {
            public string Name;
            public decimal Price;
            public int Size;
            public string ImagePath;

            public Product(string name, decimal price, int size, string image)
            {
                Name = name;
                Price = price;
                Size = size;
                ImagePath = image;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormEditProducts form = new FormEditProducts(products, role);
            form.ShowDialog();
            DisplayProducts(); // обновляем витрину после изменений
        }
    }
}
