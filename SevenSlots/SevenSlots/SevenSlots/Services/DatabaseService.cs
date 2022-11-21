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
    public class DatabaseService:IDatabaseService
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        HttpClient client;
        private const string baseUrl = "https://7slotsapi.azurewebsites.net/";

         public async Task getAllUsers()
        {
            try
            {
                client = new HttpClient(httpClientHandler, true);
                Console.WriteLine(await client.GetAsync(baseUrl));
               // response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

               // Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
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
