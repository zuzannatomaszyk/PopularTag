using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PopularTag.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;

namespace PopularTag.Controllers
{
    public class HomeController : Controller
    {
        private async Task<ItemModel> getTags (string ApiPath){
            HttpClientHandler handler = new HttpClientHandler() {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            ItemModel result = new ItemModel(); 

            HttpResponseMessage response = await client.GetAsync(ApiPath);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ItemModel>();
            }
            if(result.items == null) {
                throw new NullReferenceException();
            }
            return result;
        }
        private int CountAll (List<TagModel> tags){
            int count = 0;
            foreach(var tag in tags){
                count += tag.count;
            }
            return count;
        }
        public IActionResult Exception(){
            return View();
        }
        public async Task< IActionResult > Index()
        {
            string ApiPath = "";
            ItemModel page = new ItemModel(); 
            List<TagModel> tags = new List<TagModel>();

            for (int i = 1; i <= 10; i++){
                try{
                    ApiPath = "https://api.stackexchange.com/2.2/tags?page=" + i + "&pagesize=100&order=desc&sort=popular&site=stackoverflow&filter=!*MPoAL(KAgsdNw0T";
                    page = await getTags(ApiPath);
                    tags.AddRange(page.items);
                } 
                catch (Exception e){
                    return View("Exception");
                }
            }
            int allTagsCounted = CountAll(tags);
            float percentage;
            foreach(var tag in tags){
                percentage = (float)tag.count / allTagsCounted * 100;
                percentage = (float)Math.Round(percentage * 100f) / 100f;
                tag.percentage = percentage;
               
            }
            return View(tags);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
