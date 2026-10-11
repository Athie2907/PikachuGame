using System;

namespace Pikachu
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelBoard = new System.Windows.Forms.Panel();
            this.bxh = new System.Windows.Forms.Button();
            this.choilai = new System.Windows.Forms.Button();
            this.Time = new System.Windows.Forms.Label();
            this.diem = new System.Windows.Forms.Label();
            this.diemkyluc = new System.Windows.Forms.Label();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.Controls.Add(this.bxh);
            this.panelBoard.Controls.Add(this.choilai);
            this.panelBoard.Controls.Add(this.Time);
            this.panelBoard.Controls.Add(this.diem);
            this.panelBoard.Controls.Add(this.diemkyluc);
            this.panelBoard.Location = new System.Drawing.Point(4, 5);
            this.panelBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(889, 131);
            this.panelBoard.TabIndex = 0;
            // 
            // bxh
            // 
            this.bxh.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bxh.Location = new System.Drawing.Point(511, 19);
            this.bxh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bxh.Name = "bxh";
            this.bxh.Size = new System.Drawing.Size(199, 94);
            this.bxh.TabIndex = 4;
            this.bxh.Text = "Bảng Xếp Hạng";
            this.bxh.UseVisualStyleBackColor = true;
            // 
            // choilai
            // 
            this.choilai.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.choilai.Location = new System.Drawing.Point(728, 20);
            this.choilai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.choilai.Name = "choilai";
            this.choilai.Size = new System.Drawing.Size(142, 92);
            this.choilai.TabIndex = 3;
            this.choilai.Text = "Chơi Lại";
            this.choilai.UseVisualStyleBackColor = true;
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Time.ForeColor = System.Drawing.Color.Red;
            this.Time.Location = new System.Drawing.Point(289, 25);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(161, 32);
            this.Time.TabIndex = 2;
            this.Time.Text = "Thời gian: 0";
            // 
            // diem
            // 
            this.diem.AutoSize = true;
            this.diem.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diem.ForeColor = System.Drawing.SystemColors.Highlight;
            this.diem.Location = new System.Drawing.Point(11, 84);
            this.diem.Name = "diem";
            this.diem.Size = new System.Drawing.Size(106, 32);
            this.diem.TabIndex = 1;
            this.diem.Text = "Điểm: 0";
            // 
            // diemkyluc
            // 
            this.diemkyluc.AutoSize = true;
            this.diemkyluc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.diemkyluc.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diemkyluc.ForeColor = System.Drawing.Color.Indigo;
            this.diemkyluc.Location = new System.Drawing.Point(9, 19);
            this.diemkyluc.Name = "diemkyluc";
            this.diemkyluc.Size = new System.Drawing.Size(241, 37);
            this.diemkyluc.TabIndex = 0;
            this.diemkyluc.Text = "Điểm Kỷ Lục: 0";
            this.diemkyluc.Click += new System.EventHandler(this.diemkyluc_Click);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Location = new System.Drawing.Point(7, 151);
            this.panelMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(885, 468);
            this.panelMiddle.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 634);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelBoard);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelBoard.ResumeLayout(false);
            this.panelBoard.PerformLayout();
            this.ResumeLayout(false);

        }

        
        
        #endregion

        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Button bxh;
        private System.Windows.Forms.Button choilai;
        private System.Windows.Forms.Label Time;
        private System.Windows.Forms.Label diem;
        private System.Windows.Forms.Label diemkyluc;
        private System.Windows.Forms.Panel panelMiddle;
    }
}

