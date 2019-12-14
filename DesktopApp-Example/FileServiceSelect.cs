using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp_Example
{
    public partial class FileServiceSelect : Form
    {
        public string ServiceName { get; private set; }
        public FileServiceSelect()
        {
            InitializeComponent();
        }

        private void buttonGoogleDrive_Click(object sender, EventArgs e)
        {
            ServiceName = "GoogleDrive";
            Close();
        }

        private void buttonOneDrive_Click(object sender, EventArgs e)
        {
            ServiceName = "OneDrive";
            Close();
        }

        private void buttonOwnServer_Click(object sender, EventArgs e)
        {
            ServiceName = "OwnServer";
            Close();
        }
    }
}
