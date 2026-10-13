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
    public partial class FormBXH : Form
    {
        public FormBXH()
        {
            InitializeComponent();
        }

        private void FormBXH_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = CONNECT.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT UserName, Score " +
                                   "FROM [dbo].[PlayerScores] " +
                                   "ORDER BY Score DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    dataGVLeaderBoard.DataSource = dataTable;

                    if (dataGVLeaderBoard.Columns.Contains("UserName"))
                        dataGVLeaderBoard.Columns["UserName"].HeaderText = "Tên người chơi";

                    if (dataGVLeaderBoard.Columns.Contains("Score"))
                        dataGVLeaderBoard.Columns["Score"].HeaderText = "Số điểm kỷ lục";

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Chưa có dữ liệu điểm để hiển thị.",
                            "Bảng xếp hạng",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải bảng xếp hạng: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

    
    }
}
