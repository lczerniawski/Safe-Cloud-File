using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public AuthData AuthData { get; private set; }

        public ServerLogin()
        {
            InitializeComponent();
            labelLoginErrors.Text = String.Empty;
            pictureBoxLoading.Visible = false;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            labelLoginErrors.Text = String.Empty;
            changeControlsStatus(false);


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
                    Close();
                }
            }
            catch (Exception)
            {
                labelLoginErrors.Text = "Błąd podczas łączenia z serwerem!";
            }

            changeControlsStatus(true);
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

        private void changeControlsStatus(bool isActive)
        {
            textBoxEmail.Enabled = isActive;
            textBoxPassword.Enabled = isActive;
            buttonLogin.Enabled = isActive;
            buttonRegister.Enabled = isActive;
            pictureBoxLoading.Visible = !isActive;
        }
    }
}
