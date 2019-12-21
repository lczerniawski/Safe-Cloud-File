using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopApp_Example.DTO;
using Newtonsoft.Json;

namespace DesktopApp_Example
{
    public partial class ServerRegister : Form
    {
        public ServerRegister()
        {
            InitializeComponent();
            labelRegisterErrors.Text = String.Empty;
            pictureBoxLoading.Visible = false;
        }

        private async void buttonRegister_Click(object sender, EventArgs e)
        {
            labelRegisterErrors.Text = String.Empty;
            pictureBoxLoading.Visible = true;

            var name = textBoxName.Text;
            var email = textBoxEmail.Text;
            var password = textBoxPassword.Text;

            try
            {
                var response = await ServerConnectionLogic.RegisterUser(name, email, password);
                if (!response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var errorDto = JsonConvert.DeserializeObject<ValidationErrorDto>(responseString);

                    foreach (var errorsValue in errorDto.Errors.Values)
                    {
                        labelRegisterErrors.Text += errorsValue[0];
                    }
                }
                else
                {
                    MessageBox.Show("Konto zostało poprawnie utworzone!", "Konto utworzone",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception)
            {
                labelRegisterErrors.Text = "Błąd podczas łączenia z serwerem!";
            }

            pictureBoxLoading.Visible = false;
        }

        private void textBox_Changed(object sender, EventArgs e)
        {
            buttonRegister.Enabled = textBoxEmail.Text.Length > 0 && textBoxPassword.Text.Length > 0 && textBoxName.Text.Length > 0;
        }
    }
}
