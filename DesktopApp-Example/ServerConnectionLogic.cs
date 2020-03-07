using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using Newtonsoft.Json;

namespace DesktopApp_Example
{
    public static class ServerConnectionLogic
    {
        private const string _baseUrl = "http://localhost:57640";
        public static async Task<HttpResponseMessage> LoginUser(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var loginDto = new LoginDto(email, password);
                var jsonLoginDto = JsonConvert.SerializeObject(loginDto);

                var content = new StringContent(jsonLoginDto,Encoding.UTF8,"application/json");
                var response = await client.PostAsync($"{_baseUrl}/api/auth/login",content);

                return response;
            }
        }

        public static async Task<HttpResponseMessage> RegisterUser(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var registerDto = new RegisterDto(email, password);
                var jsonLoginDto = JsonConvert.SerializeObject(registerDto);

                var content = new StringContent(jsonLoginDto,Encoding.UTF8,"application/json");
                var response = await client.PostAsync($"{_baseUrl}/api/auth/register",content);

                return response;
            }
        }

        public static async Task<IEnumerable<UserDto>> GetUserList(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = await client.GetAsync($"{_baseUrl}/api/User");
                if (!result.IsSuccessStatusCode)
                    throw new Exception("Error while deleting file!");

                var resultString = await result.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(resultString);

                return userList;
            }
        }
    }
}
