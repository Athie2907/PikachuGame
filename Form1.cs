using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pikachu
{
    public class PanelVeDuongNoi : Panel
    {
        public PanelVeDuongNoi()
        {
            SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer |
                System.Windows.Forms.ControlStyles.SupportsTransparentBackColor,
                true
            );

            BackColor = Color.Transparent;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Không tô nền để nhìn thấy bàn cờ bên dưới.
        }
    }
    public partial class Form1 : Form
    {
        int rows = 9;
        int cols = 16;
        int tileSize = 35;
        Image[] congiapImages;
        PictureBox[,] congiappictureBox;
        Random random = new Random();

        int[,] boardMap;

        PictureBox selected1 = null;
        PictureBox selected2 = null;
        bool isProcessing = false;
        private List<Point> connectionPoints;

        private string userName;
        private bool daLuuDiem = false;
        private int score = 0;
        private int timeLeft = 300;
        private Timer countdownTimer;
        PanelVeDuongNoi panelVeDuongNoi;
        public Form1(string currentUsername)
        {
            userName = currentUsername;
            InitializeComponent();

            panelVeDuongNoi = new PanelVeDuongNoi();

            panelVeDuongNoi.Location = panelMiddle.Location;
            panelVeDuongNoi.Size = panelMiddle.Size;

            panelVeDuongNoi.Paint += panelVeDuongNoi_Paint;

            this.Controls.Add(panelVeDuongNoi);
            panelVeDuongNoi.BringToFront();

            LoadConGiapImage();
            Initializeboard();



            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
            connectionPoints = new List<Point>();
        }

        private void panelVeDuongNoi_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadConGiapImage()
        {
            // Load the Pikachu image from the resources
            congiapImages = new Image[]
            {
                Properties.Resources.conchuot,
                Properties.Resources.contrau,
                Properties.Resources.conho,
                Properties.Resources.contho,
                Properties.Resources.conrong,
                Properties.Resources.conran,
                Properties.Resources.conngua,
                Properties.Resources.concuu,
                Properties.Resources.conkhi,
                Properties.Resources.conga,
                Properties.Resources.concho,
                Properties.Resources.conheo,
            };
        }
        private void Initializeboard()
        {
            congiappictureBox = new PictureBox[rows, cols];
            boardMap = new int[rows, cols];

            List<int> congiapIndexes = new List<int>();
            for (int i = 0; i < (rows * cols) / 2; i++)
            {
                int index = i % congiapImages.Length;
                congiapIndexes.Add(index);
                congiapIndexes.Add(index);
            }

            for (int i = 0; i < congiapIndexes.Count; i++)
            {
                int j = random.Next(i, congiapIndexes.Count);
                int temp = congiapIndexes[i];
                congiapIndexes[i] = congiapIndexes[j];
                congiapIndexes[j] = temp;
            }

            int k = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Width = tileSize;
                    pb.Height = tileSize;
                    pb.Left = j * tileSize;
                    pb.Top = i * tileSize;

                    pb.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb.BorderStyle = BorderStyle.FixedSingle;

                    int imangeIndex = congiapIndexes[k++];

                    pb.Image = congiapImages[imangeIndex];

                    boardMap[i, j] = imangeIndex;

                    panelMiddle.Controls.Add(pb);
                    congiappictureBox[i, j] = pb;

                    pb.Click += picturebox_Click;
                }
            }
        }
        private async Task ResetSelectionAsync()
        {
            // Đợi nửa giây để người chơi nhận ra mình chọn sai
            await Task.Delay(500);

            if (selected1 != null) selected1.BorderStyle = BorderStyle.FixedSingle;
            if (selected2 != null) selected2.BorderStyle = BorderStyle.FixedSingle;

            selected1 = null;
            selected2 = null;
        }
        private async void picturebox_Click(object sender, EventArgs e)
        {
            if (isProcessing) return;

            PictureBox selected = sender as PictureBox;

            if (selected.Image == null)
            {
                return;
            }
            if (selected1 == selected)
            {
                selected1.BorderStyle = BorderStyle.FixedSingle;
                selected1 = null;
                return;
            }

            if (selected1 == null)
            {
                selected1 = selected;
                selected1.BorderStyle = BorderStyle.Fixed3D;
                return;
            }
            if (selected2 == null && selected != selected1)
            {
                selected2 = selected;
                selected2.BorderStyle = BorderStyle.Fixed3D;

                isProcessing = true;

                Point pos1 = GetPictureBoxPosition(selected1);
                Point pos2 = GetPictureBoxPosition(selected2);

                if (boardMap[pos1.X, pos1.Y] == boardMap[pos2.X, pos2.Y])
                {
                    var path = ConnectStraight(pos1, pos2) ?? ConnectL(pos1, pos2) ?? Connect3Line(pos1, pos2);
                    if (path != null)
                    {
                        connectionPoints = path.Select(p =>
                        {
                            Point viTri = new Point(
                                p.Y * tileSize + tileSize / 2,
                                p.X * tileSize + tileSize / 2
                            );

                            return panelBoard.PointToClient(
                                panelMiddle.PointToScreen(viTri)
                            );
                        }).ToList();

                        panelBoard.Invalidate();

                        boardMap[pos1.X, pos1.Y] = -1;
                        boardMap[pos2.X, pos2.Y] = -1;

                        selected1.Image = null;
                        selected2.Image = null;

                        selected1.BorderStyle = BorderStyle.FixedSingle;
                        selected2.BorderStyle = BorderStyle.FixedSingle;

                        selected1 = null;
                        selected2 = null;

                        score += 10;
                        diem.Text = "Điểm: " + score;
                        if (IsBoardCleaned())
                        {
                            countdownTimer.Stop();

                            SavePlayerScore(userName, score);

                            DialogResult result = MessageBox.Show(
                                "Chúc mừng bạn đã chiến thắng! Điểm của bạn: " + score,
                                "Bạn có muốn chơi tiếp không?",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question
                            );
                            if (result == DialogResult.Yes)
                            {
                                ResetGame(true);
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            // Chọn sai 2 hình khác nhau
                            await ResetSelectionAsync();
                        }
                    }
                    else
                    {
                        await ResetSelectionAsync();
                    }
                    isProcessing = false;
                }
            }
        }
        private int GetBoardValue(int x, int y)
        {
            if (x == -1 || y == -1 || x == rows || y == cols)
            {
                return -1;
            }
            if (x < -1 || x > rows || y < -1 || y > cols)
            {
                return -2;
            }
            return boardMap[x, y];
        }
        private List<Point> ConnectStraight(Point a, Point b)
        {
            if (a.X == b.X)
            {
                int minY = Math.Min(a.Y, b.Y);
                int maxY = Math.Max(a.Y, b.Y);
                for (int y = minY + 1; y < maxY; y++)
                {
                    if (GetBoardValue(a.X, y) != -1)
                    {
                        return null;
                    }
                }
                return new List<Point> { a, b };
            }
            else if (a.Y == b.Y)
            {
                int minX = Math.Min(a.X, b.X);
                int maxX = Math.Max(a.X, b.X);
                for (int x = minX + 1; x < maxX; x++)
                {
                    if (GetBoardValue(x, a.Y) != -1)
                    {
                        return null;
                    }
                }
                return new List<Point> { a, b };
            }
            return null;
        }
        private Point GetPictureBoxPosition(PictureBox pb)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (congiappictureBox[i, j] == pb)
                    {
                        return new Point(i, j);
                    }
                }
            }
            return Point.Empty;
        }
        private List<Point> ConnectL(Point a, Point b)
        {
            Point c1 = new Point(a.X, b.Y);
            if (Isempty(c1) && ConnectStraight(a, c1) != null && ConnectStraight(c1, b) != null)
            {
                return new List<Point> { a, c1, b };
            }
            Point c2 = new Point(b.X, a.Y);
            if (Isempty(c2) && ConnectStraight(a, c2) != null && ConnectStraight(c2, b) != null)
            {
                return new List<Point> { a, c2, b };
            }
            return null;
        }
        private bool Isempty(Point p)
        {
            return GetBoardValue(p.X, p.Y) == -1;
        }

        private List<Point> Connect3Line(Point a, Point b)
        {
            for (int i = -1; i <= rows; i++)
            {
                Point mid1 = new Point(i, a.Y);
                Point mid2 = new Point(i, b.Y);
                if (Isempty(mid1) && Isempty(mid2))
                {
                    if (ConnectStraight(a, mid1) != null && ConnectL(mid1, mid2) != null && ConnectStraight(mid2, b) != null)
                    {
                        return new List<Point> { a, mid1, mid2, b };
                    }
                }
            }
            for (int j = -1; j <= cols; j++)
            {
                Point mid1 = new Point(j, a.Y);
                Point mid2 = new Point(j, b.Y);
                if (Isempty(mid1) && Isempty(mid2))
                {
                    if (ConnectStraight(a, mid1) != null && ConnectL(mid1, mid2) != null && ConnectStraight(mid2, b) != null)
                    {
                        return new List<Point> { a, mid1, mid2, b };
                    }
                }

            }

            return null;
        }
        private bool IsBoardCleaned()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (boardMap[i, j] != -1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            diem.Text = "Điểm: " + score;
            Time.Text = "Thời gian: " + timeLeft;

            GetHighScore(userName);
        }

        private void diemkyluc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Điểm hiện tại: " + score);
        }


        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            Time.Text = "Thời gian: " + timeLeft;

            if (timeLeft <= 0)
            {
                countdownTimer.Stop();

                SavePlayerScore(userName, score);

                DialogResult result = MessageBox.Show(
                    "Hết thời gian! Điểm của bạn: " + score,
                    "Bạn có muốn chơi lại không?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.Yes)
                {
                    ResetGame(true);
                }
                else
                {
                    this.Close();
                }
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (countdownTimer != null)
            {
                countdownTimer.Stop();
            }
            SavePlayerScore(userName, score);
        }
        private void SavePlayerScore(string userName, int score)
        {
            if (daLuuDiem)
                return;

            daLuuDiem = true;

            try
            {
                using (SqlConnection connection = CONNECT.GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO [dbo].[PlayerScores] (UserName, Score) " +
                                   "VALUES (@UserName, @Score)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                        command.Parameters.Add("@Score", SqlDbType.Int).Value = score;

                        command.ExecuteNonQuery();
                    }
                }
                GetHighScore(userName);
            }
            catch (Exception ex)
            {
                daLuuDiem = false;

                MessageBox.Show("Có lỗi khi lưu điểm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBXH_Click(object sender, EventArgs e)
        {
            FormBXH formBXH = new FormBXH();
            formBXH.ShowDialog();
        }

        private void ResetGame(bool IsContinue = false)
        {
            panelMiddle.Controls.Clear();
            congiappictureBox = new PictureBox[rows, cols];
            boardMap = new int[rows, cols];
            selected1 = null;
            selected2 = null;
            isProcessing = false;
            daLuuDiem = false;

            score = 0;
            timeLeft = 300;
            diem.Text = "Điểm: 0";
            Time.Text = "Thời gian: " + timeLeft + "s";

            Initializeboard();

            if (countdownTimer != null)
            {
                countdownTimer.Stop();
                countdownTimer.Start();
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void GetHighScore(string userName)
        {
            try
            {
                using (SqlConnection connection = CONNECT.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT MAX(Score) FROM [dbo].[PlayerScores] WHERE UserName = @UserName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int highScore = Convert.ToInt32(result);
                            diemkyluc.Text = "Điểm Kỷ Lục: " + highScore;
                        }
                        else
                        {
                            diemkyluc.Text = "Điểm Kỷ Lục: 0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi lấy điểm kỷ lục: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void panelBoard_Paint(object sender, PaintEventArgs e)
        {
            if (connectionPoints != null && connectionPoints.Count > 1)
            {
                using (Pen redPen = new Pen(Color.Red, 3))
                {
                    for (int i = 0; i < connectionPoints.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(
                            redPen,
                            connectionPoints[i],
                            connectionPoints[i + 1]
                        );
                    }
                }
            }
        }
        private void panelOverPlay_Paint(object sender, PaintEventArgs e)
        {
            if (connectionPoints != null && connectionPoints.Count > 1)
            {
                using (Pen redPen = new Pen(Color.Red, 3))
                {
                    for (int i = 0; i < connectionPoints.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(
                            redPen,
                            connectionPoints[i],
                            connectionPoints[i + 1]
                        );
                    }
                }
            }

        }
        private void panelVeDuongNoi_Paint(object sender, PaintEventArgs e)
        {
            if (connectionPoints == null || connectionPoints.Count < 2)
                return;

            using (Pen redPen = new Pen(Color.Red, 3))
            {
                for (int i = 0; i < connectionPoints.Count - 1; i++)
                {
                    e.Graphics.DrawLine(
                        redPen,
                        connectionPoints[i],
                        connectionPoints[i + 1]
                    );
                }
            }
        }
    }
}
