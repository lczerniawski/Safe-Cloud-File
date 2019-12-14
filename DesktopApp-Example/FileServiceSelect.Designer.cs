namespace DesktopApp_Example
{
    partial class FileServiceSelect
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
            this.buttonGoogleDrive = new System.Windows.Forms.Button();
            this.buttonOneDrive = new System.Windows.Forms.Button();
            this.buttonOwnServer = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonGoogleDrive
            // 
            this.buttonGoogleDrive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGoogleDrive.Location = new System.Drawing.Point(3, 3);
            this.buttonGoogleDrive.Name = "buttonGoogleDrive";
            this.buttonGoogleDrive.Size = new System.Drawing.Size(287, 109);
            this.buttonGoogleDrive.TabIndex = 0;
            this.buttonGoogleDrive.Text = "Google Drive";
            this.buttonGoogleDrive.UseVisualStyleBackColor = true;
            this.buttonGoogleDrive.Click += new System.EventHandler(this.buttonGoogleDrive_Click);
            // 
            // buttonOneDrive
            // 
            this.buttonOneDrive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOneDrive.Location = new System.Drawing.Point(3, 118);
            this.buttonOneDrive.Name = "buttonOneDrive";
            this.buttonOneDrive.Size = new System.Drawing.Size(287, 109);
            this.buttonOneDrive.TabIndex = 1;
            this.buttonOneDrive.Text = "OneDrive";
            this.buttonOneDrive.UseVisualStyleBackColor = true;
            this.buttonOneDrive.Click += new System.EventHandler(this.buttonOneDrive_Click);
            // 
            // buttonOwnServer
            // 
            this.buttonOwnServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOwnServer.Location = new System.Drawing.Point(3, 233);
            this.buttonOwnServer.Name = "buttonOwnServer";
            this.buttonOwnServer.Size = new System.Drawing.Size(287, 109);
            this.buttonOwnServer.TabIndex = 2;
            this.buttonOwnServer.Text = "Własny Serwer";
            this.buttonOwnServer.UseVisualStyleBackColor = true;
            this.buttonOwnServer.Click += new System.EventHandler(this.buttonOwnServer_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.buttonGoogleDrive, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOwnServer, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonOneDrive, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(293, 345);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // FileServiceSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 345);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FileServiceSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wybierz rodzaj serwera plików";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGoogleDrive;
        private System.Windows.Forms.Button buttonOneDrive;
        private System.Windows.Forms.Button buttonOwnServer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}