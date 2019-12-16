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
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(118, 310);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Dalej";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lista zarejestrowanych użytkowników";
            // 
            // listBoxUsers
            // 
            this.listBoxUsers.Enabled = false;
            this.listBoxUsers.Location = new System.Drawing.Point(12, 61);
            this.listBoxUsers.Name = "listBoxUsers";
            this.listBoxUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxUsers.Size = new System.Drawing.Size(287, 225);
            this.listBoxUsers.TabIndex = 2;
            this.listBoxUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxUsers_SelectedIndexChanged);
            // 
            // checkBoxShare
            // 
            this.checkBoxShare.AutoSize = true;
            this.checkBoxShare.Location = new System.Drawing.Point(12, 25);
            this.checkBoxShare.Name = "checkBoxShare";
            this.checkBoxShare.Size = new System.Drawing.Size(144, 17);
            this.checkBoxShare.TabIndex = 3;
            this.checkBoxShare.Text = "Udostępnij osobą trzecim";
            this.checkBoxShare.UseVisualStyleBackColor = true;
            this.checkBoxShare.CheckedChanged += new System.EventHandler(this.checkBoxShare_CheckedChanged);
            // 
            // ShareFile
            // 
            this.AcceptButton = this.buttonNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 345);
            this.Controls.Add(this.checkBoxShare);
            this.Controls.Add(this.listBoxUsers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonNext);
            this.Name = "ShareFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShareFile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxUsers;
        private System.Windows.Forms.CheckBox checkBoxShare;
    }
}