using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodstvenik
{
    public partial class Form1 : Form
    {
        private string _roleFormat;

        public Form1()
        {
            InitializeComponent();
            _roleFormat = roleLabel.Text;
            UpdateRoleLabel();
        }

        private int? _userId = null;
        private bool IsAuthorized => _userId != null && _userId > 0;

        private void button1_Click(object sender, EventArgs e)
        {
            (string login, string password) = (loginInput.Text, passwordInput.Text);

            using (var dbContext = new UsersEntities())
            {
                var userData = dbContext.Users
                    .FirstOrDefault(user => user.Пароль == password
                    && user.Логин == login);

                if (userData != null)
                    _userId = userData.ID;
            }

            UpdateRoleLabel();
        }

        private void UpdateRoleLabel()
        {
            string roleName = "Неавторизован";

            if (IsAuthorized)
            {
                using (var dbContext = new UsersEntities())
                {
                    var roleId = dbContext.Users.First(user => user.ID == _userId).ID_role;
                    roleName = dbContext.Roles.First(role => role.ID == roleId).Роль;
                }
            }

            roleLabel.Text = string.Format(_roleFormat, roleName);
        }
    }
}
