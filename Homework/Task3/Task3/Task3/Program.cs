using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Gamp.ConsoleApp
{
    class Program
    {
        private static bool IsStopped = false;

        static void Main(string[] args)
        {
            Run();
            Console.ReadLine();
            IsStopped = true;
        }


        public static async Task Run()
        {
            Func<Task> func = async () =>
            {
                var blockId = await ReadIrreversibleBlockAsync();
                await TrackEvent(blockId);
                Console.WriteLine(blockId);
            };

            while (!IsStopped)
            {
                Task.Run(func);
                await Task.Delay(1000);
            }
        }

        private static async Task<string> ReadIrreversibleBlockAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wax.cryptolions.io/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/v1/chain/get_info");
                response.EnsureSuccessStatusCode();

                var action = await response.Content.ReadAsStringAsync();

                JObject joResponse = JObject.Parse(action);
                var last_irreversible_block_id = joResponse["last_irreversible_block_id"];

                return last_irreversible_block_id.Value<string>();
            }
        }

        public static async Task<HttpResponseMessage> TrackEvent(string blockId)
        {
            using (var httpClient = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("v", "1"),
                    new KeyValuePair<string, string>("tid", "UA-194282198-1"),
                    new KeyValuePair<string, string>("cid", "123225256.1617981309"),
                    new KeyValuePair<string, string>("t", "event"),
                    new KeyValuePair<string, string>("ec", "last_irreversible_block"),
                    new KeyValuePair<string, string>("ea", blockId)
                };

                return await httpClient.PostAsync("https://www.google-analytics.com/collect", new FormUrlEncodedContent(postData));
            }
        }
    }
}