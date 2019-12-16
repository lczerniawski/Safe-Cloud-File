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
    public partial class LinksToShare : Form
    {
        public LinksToShare(string jsonFileLink,string encryptedFileLink)
        {
            InitializeComponent();
            textBoxJsonFileLink.Text = jsonFileLink;
            textBoxEncryptedFileLink.Text = encryptedFileLink;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
