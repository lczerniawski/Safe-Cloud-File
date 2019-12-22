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
    public partial class ShareFile : Form
    {
        public List<UserDto> SelectedUsers { get; }
        private readonly string _token;
        private readonly string _userEmail;

        public ShareFile(string token, string userEmail)
        {
            InitializeComponent();
            _token = token;
            _userEmail = userEmail;
            SelectedUsers = new List<UserDto>();
        }

        private async void checkBoxShare_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShare.Checked)
            {
                ChangeFormStatus(false);
                var loader = new Loader("Pobieranie listy użytkowników",
                    "Trwa pobieranie listy użytkowników, proszę czekać!");
                loader.ControlBox = false;
                loader.Show();
                loader.Owner = this;
                try
                {
                    await PopulateUserList();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Błąd podczas pobierania listy użytkowników aplikacji. Sprobój ponownie pózniej!",
                        "Błąd pobierania listy użytkowników", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    ChangeFormStatus(true);
                    checkBoxShare.Checked = false;
                    Invoke(new Action(loader.Close));
                    return;
                }
                Invoke(new Action(loader.Close));
            }
            else
            {
                ChangeFormStatus(true);
            }
        }

        private async Task PopulateUserList()
        {
            Invoke(new Action(listBoxUsers.Items.Clear));
            var userList = await ServerConnectionLogic.GetUserList(_token);

            foreach (var userDto in userList)
            {
                if (!userDto.Email.Equals(_userEmail))
                    Invoke(new Func<UserDto, int>(listBoxUsers.Items.Add), userDto);
            }
            
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (checkBoxShare.Checked)
            {
                var selectedItems = listBoxUsers.SelectedItems;
                foreach (var selectedItem in selectedItems)
                {
                    SelectedUsers.Add(selectedItem as UserDto);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonNext.Enabled = listBoxUsers.SelectedItems.Count > 0;
        }

        private void ChangeFormStatus(bool isActive)
        {
            buttonNext.Enabled = isActive;
            listBoxUsers.Enabled = !isActive;
        }
    }
}
