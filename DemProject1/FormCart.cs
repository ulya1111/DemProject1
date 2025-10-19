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
    public partial class FormCart : Form
    {
        private List<Product> cartItems; // список товаров в корзине

        public FormCart(List<Product> items)
        {
            InitializeComponent();
            cartItems = items;
            DisplayCartItems();
        }

        private void DisplayCartItems()
        {
            listBoxCart.Items.Clear();

            decimal total = 0;
            foreach (var item in cartItems)
            {
                string line = $"{item.Name} — {item.Price} руб.";
                listBoxCart.Items.Add(line);
                total += item.Price;
            }

            labelTotal.Text = $"Итого: {total} руб.";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
