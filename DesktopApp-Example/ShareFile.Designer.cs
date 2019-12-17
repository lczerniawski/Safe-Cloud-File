namespace DesktopApp_Example
{
    partial class ShareFile
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
            this.buttonNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxUsers = new System.Windows.Forms.ListBox();
            this.checkBoxShare = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNext.Location = new System.Drawing.Point(3, 366);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(238, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Dalej";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lista zarejestrowanych użytkowników";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxUsers
            // 
            this.listBoxUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUsers.Enabled = false;
            this.listBoxUsers.Location = new System.Drawing.Point(3, 56);
            this.listBoxUsers.Name = "listBoxUsers";
            this.listBoxUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxUsers.Size = new System.Drawing.Size(238, 304);
            this.listBoxUsers.TabIndex = 2;
            this.listBoxUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxUsers_SelectedIndexChanged);
            // 
            // checkBoxShare
            // 
            this.checkBoxShare.AutoSize = true;
            this.checkBoxShare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxShare.Location = new System.Drawing.Point(3, 3);
            this.checkBoxShare.Name = "checkBoxShare";
            this.checkBoxShare.Size = new System.Drawing.Size(238, 22);
            this.checkBoxShare.TabIndex = 3;
            this.checkBoxShare.Text = "Udostępnij osobą trzecim";
            this.checkBoxShare.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShare.UseVisualStyleBackColor = true;
            this.checkBoxShare.CheckedChanged += new System.EventHandler(this.checkBoxShare_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxShare, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonNext, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.listBoxUsers, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.894737F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.017544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.08772F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 392);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // ShareFile
            // 
            this.AcceptButton = this.buttonNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 392);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ShareFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Udostępnianie";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxUsers;
        private System.Windows.Forms.CheckBox checkBoxShare;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}