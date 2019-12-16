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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Link do pliku JSON";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Link do zaszyfrowanego pliku";
            // 
            // textBoxJsonLink
            // 
            this.textBoxJsonLink.Location = new System.Drawing.Point(12, 25);
            this.textBoxJsonLink.Name = "textBoxJsonLink";
            this.textBoxJsonLink.Size = new System.Drawing.Size(514, 20);
            this.textBoxJsonLink.TabIndex = 2;
            // 
            // textBoxFileLink
            // 
            this.textBoxFileLink.Location = new System.Drawing.Point(12, 64);
            this.textBoxFileLink.Name = "textBoxFileLink";
            this.textBoxFileLink.Size = new System.Drawing.Size(514, 20);
            this.textBoxFileLink.TabIndex = 3;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(227, 212);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 4;
            this.buttonDownload.Text = "Pobierz";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // DownloadShared
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 249);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.textBoxFileLink);
            this.Controls.Add(this.textBoxJsonLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DownloadShared";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DownloadShared";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxJsonLink;
        private System.Windows.Forms.TextBox textBoxFileLink;
        private System.Windows.Forms.Button buttonDownload;
    }
}