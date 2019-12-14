namespace DesktopApp_Example
{
    partial class ServerLogin
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
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelLoginErrors = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "E-mail";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(253, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hasło";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEmail.Location = new System.Drawing.Point(3, 154);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(253, 20);
            this.textBoxEmail.TabIndex = 3;
            this.textBoxEmail.Text = "luki.lary@gmail.com";
            this.textBoxEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxEmail.TextChanged += new System.EventHandler(this.textBox_Changed);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Location = new System.Drawing.Point(3, 219);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(253, 20);
            this.textBoxPassword.TabIndex = 4;
            this.textBoxPassword.Text = "podczaszy96";
            this.textBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBox_Changed);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLogin.Enabled = false;
            this.buttonLogin.Location = new System.Drawing.Point(3, 256);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(253, 27);
            this.buttonLogin.TabIndex = 5;
            this.buttonLogin.Text = "Zaloguj się";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonRegister
            // 
            this.buttonRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRegister.Location = new System.Drawing.Point(3, 393);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(253, 28);
            this.buttonRegister.TabIndex = 6;
            this.buttonRegister.Text = "Zarejestruj się";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 358);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(253, 32);
            this.label3.TabIndex = 7;
            this.label3.Text = "Nie masz konta?";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLoginErrors
            // 
            this.labelLoginErrors.AutoSize = true;
            this.labelLoginErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoginErrors.ForeColor = System.Drawing.Color.Red;
            this.labelLoginErrors.Location = new System.Drawing.Point(3, 0);
            this.labelLoginErrors.Name = "labelLoginErrors";
            this.labelLoginErrors.Size = new System.Drawing.Size(253, 128);
            this.labelLoginErrors.TabIndex = 8;
            this.labelLoginErrors.Text = "label4";
            this.labelLoginErrors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelLoginErrors, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxEmail, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxPassword, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonLogin, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonRegister, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.8084F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.036746F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.711287F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.086614F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.448819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.527132F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.46966F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.17942F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(259, 424);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLoading.ImageLocation = "Assets/loadingCircle.gif";
            this.pictureBoxLoading.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(259, 424);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLoading.TabIndex = 10;
            this.pictureBoxLoading.TabStop = false;
            // 
            // ServerLogin
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 424);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ServerLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logowanie";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLoginErrors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
    }
}