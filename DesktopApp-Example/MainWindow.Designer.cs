namespace DesktopApp_Example
{
    partial class MainWindow
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pobierzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuńToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDownloadShared = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.fileContextMenuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUpload
            // 
            this.buttonUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpload.Location = new System.Drawing.Point(388, 423);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(380, 24);
            this.buttonUpload.TabIndex = 0;
            this.buttonUpload.Text = "Wgraj plik";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(765, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Twoje Pliki";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fileContextMenuStrip
            // 
            this.fileContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pobierzToolStripMenuItem,
            this.usuńToolStripMenuItem});
            this.fileContextMenuStrip.Name = "fileContextMenuStrip";
            this.fileContextMenuStrip.Size = new System.Drawing.Size(114, 48);
            // 
            // pobierzToolStripMenuItem
            // 
            this.pobierzToolStripMenuItem.Name = "pobierzToolStripMenuItem";
            this.pobierzToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.pobierzToolStripMenuItem.Text = "Pobierz";
            this.pobierzToolStripMenuItem.Click += new System.EventHandler(this.pobierzToolStripMenuItem_Click);
            // 
            // usuńToolStripMenuItem
            // 
            this.usuńToolStripMenuItem.Name = "usuńToolStripMenuItem";
            this.usuńToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.usuńToolStripMenuItem.Text = "Usuń";
            this.usuńToolStripMenuItem.Click += new System.EventHandler(this.usuńToolStripMenuItem_Click);
            // 
            // buttonDownloadShared
            // 
            this.buttonDownloadShared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDownloadShared.Location = new System.Drawing.Point(3, 423);
            this.buttonDownloadShared.Name = "buttonDownloadShared";
            this.buttonDownloadShared.Size = new System.Drawing.Size(379, 24);
            this.buttonDownloadShared.TabIndex = 4;
            this.buttonDownloadShared.Text = "Pobierz udotępniony plik";
            this.buttonDownloadShared.UseVisualStyleBackColor = true;
            this.buttonDownloadShared.Click += new System.EventHandler(this.buttonDownloadShared_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBoxFiles, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonUpload, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonDownloadShared, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.060606F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.93939F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(771, 450);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // listBoxFiles
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listBoxFiles, 2);
            this.listBoxFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.Location = new System.Drawing.Point(3, 28);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(765, 389);
            this.listBoxFiles.TabIndex = 3;
            this.listBoxFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxFiles_MouseDown);
            // 
            // MainWindow
            // 
            this.AcceptButton = this.buttonUpload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Głowne Okno";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.fileContextMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip fileContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem pobierzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usuńToolStripMenuItem;
        private System.Windows.Forms.Button buttonDownloadShared;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBoxFiles;
    }
}

