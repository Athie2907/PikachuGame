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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pikachu
{
    public partial class FormSignin : Form
    {
        public FormSignin()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click (object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string username = tbDangKy.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng ký.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = CONNECT.GetConnection())
                {
                    connection.Open();

                    string checkQuery = "SELECT * FROM [dbo].[Table] WHERE UserName = @UserName";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@UserName", username);

                        using (SqlDataReader reader = checkCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("Tên đăng ký đã tồn tại. Vui lòng chọn tên khác.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    string insertQuery = "INSERT INTO [dbo].[Table] (UserName) VALUES (@UserName)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@UserName", username);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đăng ký thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Đăng ký thất bại. Vui lòng thử lại.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi đăng ký: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
