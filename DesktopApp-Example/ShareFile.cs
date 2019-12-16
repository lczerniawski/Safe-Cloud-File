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
        private readonly string BaseUrl = "https://localhost:44312";
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
                buttonNext.Enabled = false;
                listBoxUsers.Enabled = true;
                var loader = new Loader("Pobieranie listy użytkowników",
                    "Trwa pobieranie listy użytkowników, proszę czekać!");
                loader.ControlBox = false;
                loader.Show();
                loader.Owner = this;
                await PopulateUserList();
                Invoke(new Action(loader.Close));
            }
            else
            {
                buttonNext.Enabled = true;
                listBoxUsers.Enabled = false;
            }
        }

        private async Task PopulateUserList()
        {
            Invoke(new Action(listBoxUsers.Items.Clear));
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                var result = await client.GetAsync($"{BaseUrl}/api/User");
                if (!result.IsSuccessStatusCode)
                    throw new Exception("Error while deleting file!");

                var resultString = await result.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(resultString);

                foreach (var userDto in userList)
                {
                    if(!userDto.Email.Equals(_userEmail))
                        Invoke(new Func<UserDto,int>(listBoxUsers.Items.Add), userDto);
                }
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
    }
}
