using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace SevenSlots.Services
{
    public class DatabaseService:IDatabaseService
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        HttpClient client;
        private const string baseUrl = "http://10.0.2.2:7116/User";

         public async Task getUserWithIdAsync(Guid id)
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

        Task IDatabaseService.addUser(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
