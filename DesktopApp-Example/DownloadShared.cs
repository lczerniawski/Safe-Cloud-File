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

namespace DesktopApp_Example
{
    public partial class DownloadShared : Form
    {
        private readonly AuthData _authData;
        private readonly IFileService _fileService;

        public DownloadShared(AuthData authData,IFileService fileService)
        {
            _authData = authData;
            _fileService = fileService;
            InitializeComponent();
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    var loader = new Loader("Pobieranie w toku","Trwa pobieranie pliku, proszę czekać!");
                    loader.ControlBox = false;
                    loader.Owner = this;
                    loader.Show();
                    SwitchFormEnabled(false);
                    SharedDownload sharedDownload = null;
                    try
                    {
                        sharedDownload = await _fileService.DownloadShared(textBoxFileLink.Text, textBoxJsonLink.Text, _authData.Email, _authData.RsaKeys.MapToRsaParameters());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            "Podane Linki nie są prawidłowe, lub wystapił problem podczas pobierania pliku, sprawdz swoje linki i sproboj ponownie pozniej.",
                            "Błąd pobierania", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        loader.Close();
                        SwitchFormEnabled(true);
                        return;
                    }
                    var filePath = folderBrowserDialog.SelectedPath;
                    if (File.Exists(Path.Combine(filePath,sharedDownload.FileName)))
                    {
                        MessageBox.Show(
                            $"Plik o nazwie {sharedDownload.FileName} istnieje w katalogu docelowym, wybierz inny katalog lub usuń isteniejący plik"
                            , "Powtarzająca się nazwa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    using (var fileStream = new FileStream(filePath + "/" + sharedDownload.FileName, FileMode.Create))
                    {
                        sharedDownload.MemoryStream.Position = 0;
                        await sharedDownload.MemoryStream.CopyToAsync(fileStream);
                    }

                    Invoke(new Action(loader.Close));
                    MessageBox.Show("Plik pobrany", "Plik został pobrany pomyślnie!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Invoke(new Action(Close));
                }
            }
        }

        private void SwitchFormEnabled(bool isActive)
        {
            textBoxFileLink.Enabled = isActive;
            textBoxJsonLink.Enabled = isActive;
            buttonDownload.Enabled = isActive;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            buttonDownload.Enabled = textBoxFileLink.Text.Length > 0 && textBoxJsonLink.Text.Length > 0;
        }
    }
}
