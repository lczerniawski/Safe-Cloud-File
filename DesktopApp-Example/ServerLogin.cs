using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopApp_Example.DTO;
using Newtonsoft.Json;

namespace DesktopApp_Example
{
    public partial class ServerLogin : Form
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "authdata.json");
        public AuthData AuthData { get; private set; }

        public ServerLogin()
        {
            InitializeComponent();
            labelLoginErrors.Text = String.Empty;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            labelLoginErrors.Text = String.Empty;
            ChangeControlsStatus(false);


            var email = textBoxEmail.Text;
            var password = textBoxPassword.Text;

            try
            {
                var response = await AuthLogic.LoginUser(email, password);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorDto = JsonConvert.DeserializeObject<ValidationErrorDto>(responseString);

                    foreach (var errorsValue in errorDto.Errors.Values)
                    {
                        labelLoginErrors.Text += errorsValue[0];
                    }
                }
                else
                {
                    AuthData = JsonConvert.DeserializeObject<AuthData>(responseString);
                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        
                        await streamWriter.WriteAsync(responseString);
                    }
                    Close();
                }
            }
            catch (Exception)
            {
                labelLoginErrors.Text = "Błąd podczas łączenia z serwerem!";
            }

            ChangeControlsStatus(true);
        }

        private void textBox_Changed(object sender, EventArgs e)
        {
            buttonLogin.Enabled = textBoxEmail.Text.Length > 0 && textBoxPassword.Text.Length > 0;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new ServerRegister();
            registerForm.ShowDialog();
        }

        private void ChangeControlsStatus(bool isActive)
        {
            textBoxEmail.Enabled = isActive;
            textBoxPassword.Enabled = isActive;
            buttonLogin.Enabled = isActive;
            buttonRegister.Enabled = isActive;
            pictureBoxLoading.Visible = !isActive;
        }

        private void ServerLogin_Load(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                using (var fileStream = new FileStream(filePath,FileMode.Open))
                using (var streamReader = new StreamReader(fileStream))
                {
                    var authDataString = streamReader.ReadToEnd();
                    var authData = JsonConvert.DeserializeObject<AuthData>(authDataString);
                    var validDate = (new DateTime(1970, 1, 1)).AddSeconds(authData.TokenExpirationTime);
                    if (validDate > DateTime.Now)
                    {
                        AuthData = authData;
                        Close();
                    }
                    else
                    {
                        fileStream.Dispose();
                        File.Delete(filePath);
                    }
                }
            }
        }
    }
}
