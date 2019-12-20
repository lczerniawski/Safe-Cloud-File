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
    public static class AuthLogic
    {
        private static string _baseUrl = "https://apiserver-example.azurewebsites.net";
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

        public static async Task<HttpResponseMessage> RegisterUser(string name, string email, string password)
        {
            using (var client = new HttpClient())
            {
                var registerDto = new RegisterDto(name,email, password);
                var jsonLoginDto = JsonConvert.SerializeObject(registerDto);

                var content = new StringContent(jsonLoginDto,Encoding.UTF8,"application/json");
                var response = await client.PostAsync($"{_baseUrl}/api/auth/register",content);

                return response;
            }
        }
    }
}
