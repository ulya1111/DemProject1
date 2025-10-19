using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DemProject1.FormMain;

namespace DemProject1
{
    public partial class FormEditProducts : Form
    {
        private List<Product> products;
        private string role;
        public FormEditProducts()
        {
            InitializeComponent();
        }

        public FormEditProducts(List<Product> productList, string userRole)
        {
            InitializeComponent();
            products = productList;
            role = userRole;
            SetupForm();
            RefreshGrid();
        }

        private void SetupForm()
        {
            if (role == "Manager")
            {
                buttonAdd.Enabled = false;  // менеджер не может добавлять
                buttonDelete.Enabled = false; // и удалять
            }

            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.ReadOnly = true;
        }

        private void RefreshGrid()
        {
            dataGridViewProducts.DataSource = null;
            dataGridViewProducts.DataSource = products.Select(p => new
            {
                Название = p.Name,
                Цена = p.Price,
                Размер = p.Size,
                Изображение = p.ImagePath
            }).ToList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text) || numericPrice.Value == 0)
            {
                MessageBox.Show("Введите корректные данные");
                return;
            }

            products.Add(new Product(
                textBoxName.Text,
                numericPrice.Value,
                (int)numericSize.Value,
                textBoxImagePath.Text
            ));

            RefreshGrid();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для изменения");
                return;
            }

            int index = dataGridViewProducts.SelectedRows[0].Index;

            products[index].Name = textBoxName.Text;
            products[index].Price = numericPrice.Value;
            products[index].Size = (int)numericSize.Value;
            products[index].ImagePath = textBoxImagePath.Text;

            RefreshGrid();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }

            int index = dataGridViewProducts.SelectedRows[0].Index;
            products.RemoveAt(index);
            RefreshGrid();
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var row = dataGridViewProducts.SelectedRows[0];
                textBoxName.Text = row.Cells[0].Value.ToString();
                numericPrice.Value = Convert.ToDecimal(row.Cells[1].Value);
                numericSize.Value = Convert.ToDecimal(row.Cells[2].Value);
                textBoxImagePath.Text = row.Cells[3].Value.ToString();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
