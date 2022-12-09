using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SevenSlots.Services
{
    public class UserRepository: IUserRepository
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        HttpClient client;
        private const string baseUrl = "https://7slotsapi.azurewebsites.net/";

        public async Task<List<User>> getAllUsers()
        {
            try
            {
                client = new HttpClient(httpClientHandler, true);
                HttpResponseMessage response = await client.GetAsync(baseUrl + "/User");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Fetching the users failed: {0}", response.Content.ReadAsStringAsync());
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync();
                List<User> Users = JsonSerializer.Deserialize<List<User>>(content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});

                return Users;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception caught while retrieving the users! Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<string> addUser(User newUser)
        {
            try
            {
                client = new HttpClient(httpClientHandler, true);

                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    UTF8Encoding utf8 = new UTF8Encoding();
                    byte[] data = md5.ComputeHash(utf8.GetBytes(newUser.Password));
                    newUser.Password = Convert.ToBase64String(data);
                }

                string json = JsonSerializer.Serialize<User>(newUser);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(baseUrl + "/User", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Adding the user failed: {0}", error);
                    return error;
                }

                return "Ok";
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception caught while adding the user! Message :{0} ", e.Message);

                return "HttpRequestException: " + e.Message;
            }
        }

        public async Task updateWallet(Guid id, double wallet)
        {
            try
            {
                client = new HttpClient(httpClientHandler, true);
                HttpResponseMessage response = await client.SendAsync(
                    new HttpRequestMessage(
                        new HttpMethod("PATCH"),
                        baseUrl + "/User?id=" + id.ToString() + "&wallet=" + wallet.ToString()
                        )
                );
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Updating the wallet failed: {0}", response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception caught while updating the wallet! Message :{0} ", e.Message);
            }
        }
    }
}
