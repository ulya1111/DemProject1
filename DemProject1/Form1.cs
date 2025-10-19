using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace DemProject1
{
    public partial class FormLogin : Form
    {
        private Dictionary<string, (string Password, string Role)> users = new Dictionary<string, (string, string)>
        {
            { "admin", ("123", "Admin") },
            { "manager", ("123", "Manager") },
            { "user", ("123", "User") }
        };
        private string role;

        public FormLogin()
        {
            InitializeComponent();
        }

        public void FormMain(string userRole)
        {
            InitializeComponent();
            role = userRole;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            if (users.ContainsKey(login) && users[login].Password == password)
            {
                string role = users[login].Role;
                FormMain mainForm = new FormMain(role);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            string role = comboBoxRole.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (users.ContainsKey(login))
            {
                MessageBox.Show("Пользователь уже существует");
                return;
            }

            users.Add(login, (password, role));
            MessageBox.Show("Регистрация успешна!");
        }
    }
}
