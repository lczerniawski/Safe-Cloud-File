namespace DesktopApp_Example
{
    partial class DownloadShared
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxJsonLink = new System.Windows.Forms.TextBox();
            this.textBoxFileLink = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(531, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Link do pliku JSON";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(531, 34);
            this.label2.TabIndex = 1;
            this.label2.Text = "Link do zaszyfrowanego pliku";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxJsonLink
            // 
            this.textBoxJsonLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxJsonLink.Location = new System.Drawing.Point(4, 35);
            this.textBoxJsonLink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxJsonLink.Name = "textBoxJsonLink";
            this.textBoxJsonLink.Size = new System.Drawing.Size(531, 22);
            this.textBoxJsonLink.TabIndex = 2;
            this.textBoxJsonLink.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxFileLink
            // 
            this.textBoxFileLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFileLink.Location = new System.Drawing.Point(4, 118);
            this.textBoxFileLink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxFileLink.Name = "textBoxFileLink";
            this.textBoxFileLink.Size = new System.Drawing.Size(531, 22);
            this.textBoxFileLink.TabIndex = 3;
            this.textBoxFileLink.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDownload.Enabled = false;
            this.buttonDownload.Location = new System.Drawing.Point(4, 275);
            this.buttonDownload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(531, 34);
            this.buttonDownload.TabIndex = 4;
            this.buttonDownload.Text = "Pobierz";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonDownload, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxJsonLink, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFileLink, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.30043F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.87983F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1588F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.07296F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.87554F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 313);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // DownloadShared
            // 
            this.AcceptButton = this.buttonDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 313);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DownloadShared";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pobierz udostępniony plik";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxJsonLink;
        private System.Windows.Forms.TextBox textBoxFileLink;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}