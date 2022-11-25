using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace SevenSlots.Services
{
    public class DatabaseRepository: IDatabaseRepository
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
                Console.WriteLine("Exception Caught! Message :{0} ", e.Message);
                throw;
            }
        }

        public async Task addUser(User newUser)
        {
            try
            {
                client = new HttpClient(httpClientHandler, true);

                string json = JsonSerializer.Serialize<User>(newUser);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(baseUrl + "/User", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Adding the user failed: {0}", response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught! Message :{0} ", e.Message);
            }
        }
    }
}
