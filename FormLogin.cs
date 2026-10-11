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
        public string UserName { get; set; }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string userName = tbDangNhap.Text.Trim();
            using (SqlConnection connection = CONNECT.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE UserName = @UserName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            UserName = userName;
                            // User exists, proceed to the main form
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            // User does not exist, show an error message
                            MessageBox.Show("Tên đăng nhập không tồn tại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            FormSignin formsignin = new FormSignin();
            formsignin.ShowDialog();
        }
    }
}
