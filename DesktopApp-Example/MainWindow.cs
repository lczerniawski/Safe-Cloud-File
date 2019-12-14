using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopApp_Example.DTO;
using DesktopApp_Example.Helpers;
using DesktopApp_Example.Services;
using Inzynierka_Core.Model;

namespace DesktopApp_Example
{
    public partial class MainWindow : Form
    {
        private readonly AuthData _authData;
        private readonly IFileService _fileService;
        private ViewFile _selectedFile;

        public MainWindow()
        {
            InitializeComponent();

            var serverLogin = new ServerLogin();
            serverLogin.ShowDialog();
            _authData = serverLogin.AuthData;
            if (_authData == null)
                Environment.Exit(-1);

            var fileServerSelect = new FileServiceSelect();
            fileServerSelect.ShowDialog();
            var fileService = fileServerSelect.ServiceName;

            if (fileService == null)
                Environment.Exit(-1);

            _fileService = FileServiceFactory.Create(fileService);
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            var loader = new Loader("Pobieranie listy plików","Trwa pobieranie listy plików, proszę czekać!");
            loader.ControlBox = false;
            loader.Show();
            loader.Owner = this;
            SwitchFormEnabled(false);
            await RefreshFileList();
            Invoke(new Action(loader.Close));
            Invoke(new Action<bool>(SwitchFormEnabled),true);
        }

        private async void buttonUpload_Click(object sender, EventArgs e)
        {
            var receiverList = new List<Receiver>
            {
                new Receiver(_authData.Name, _authData.Email, _authData.RsaKeys.MapToRsaParameters())
            };

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileName = openFileDialog.SafeFileName?.Split('.').First();
                    var fileExtension = Path.GetExtension(openFileDialog.FileName);
                    var fileStream = openFileDialog.OpenFile() as FileStream;

                    var loader = new Loader("Uploadowanie w toku","Trwa uploadowanie pliku, proszę czekać!");
                    loader.ControlBox = false;
                    loader.Show();
                    SwitchFormEnabled(false);
                    await _fileService.UploadFile(fileName, fileExtension,fileStream, receiverList, _authData.RsaKeys.MapToRsaParameters());
                    await RefreshFileList();
                    Invoke(new Action(loader.Close));
                    Invoke(new Action<bool>(SwitchFormEnabled),true);
                }
            }
        }

        private void listBoxFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBoxFiles.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                listBoxFiles.SelectedIndex = index;
                _selectedFile = listBoxFiles.Items[index] as ViewFile;
                fileContextMenuStrip.Show(Cursor.Position);
                fileContextMenuStrip.Visible = true;
            }
            else
            {
                fileContextMenuStrip.Visible = false;
            }
        }

        private async void pobierzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = folderBrowserDialog.SelectedPath;
                    if (File.Exists(Path.Combine(filePath,_selectedFile.Name)))
                    {
                        MessageBox.Show(
                            "Plik o takiej nazwie istnieje w katalogu docelowym, wybierz inny katalog lub usuń isteniejący plik"
                            , "Powtarzająca się nazwa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    var loader = new Loader("Pobieranie w toku","Trwa pobieranie pliku, proszę czekać!");
                    loader.ControlBox = false;
                    loader.Show();
                    SwitchFormEnabled(false);
                    await _fileService.DownloadFile(filePath, _selectedFile, _authData.Email, _authData.RsaKeys.MapToRsaParameters(), _authData.RsaKeys.MapToRsaParameters());
                    Invoke(new Action(loader.Close));
                    Invoke(new Action<bool>(SwitchFormEnabled),true);
                }
            }
        }

        private async void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loader = new Loader("Usuwanie w toku","Trwa usuwanie pliku, proszę czekać!");
            loader.ControlBox = false;
            loader.Show();
            SwitchFormEnabled(false);
            await _fileService.DeleteFile(_selectedFile);
            await RefreshFileList();
            Invoke(new Action(loader.Close));
            Invoke(new Action<bool>(SwitchFormEnabled),true);
        }

        private async Task RefreshFileList()
        {
            Invoke(new Action(listBoxFiles.Items.Clear));
            var files = await _fileService.GetAllFiles();
            foreach (var viewFile in files)
            {
                Invoke(new Func<object,int>(listBoxFiles.Items.Add), viewFile);
            }
        }

        private void SwitchFormEnabled(bool isActive)
        {
            listBoxFiles.Enabled = isActive;
            buttonUpload.Enabled = isActive;
            fileContextMenuStrip.Enabled = isActive;
        }
    }
}
