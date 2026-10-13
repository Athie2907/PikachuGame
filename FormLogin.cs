using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pikachu
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public string UserName { get; set; }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string userName = tbDangNhap.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = CONNECT.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM [dbo].[Table] WHERE UserName = @UserName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            UserName = userName;

                            Form1 game = new Form1(UserName);
                            this.Hide();
                            game.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Tên đăng nhập không tồn tại. Vui lòng đăng ký trước.",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi đăng nhập: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            using (FormSignin formsignin = new FormSignin())
            {
                formsignin.ShowDialog();
            }
        }
    }
}