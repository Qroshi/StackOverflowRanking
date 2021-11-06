using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Host;
using StackOverflowRanking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StackOverflowRanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private string version = "2.3";

        private int pages = 10;

        private double total = 0;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var httpClient = new HttpClient(handler);
            var apiUrl = ("http://api.stackexchange.com");

            httpClient.BaseAddress = new Uri(apiUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Tags tags= new Tags();

            try
            {
                for (int i = 0; i < pages; i++)
                {
                    string parameter = String.Format("/{0}/tags?page={1}&pagesize=100&order=desc&sort=popular&site=stackoverflow&filter=!LhRRNhD6sFb(YD9Wva1aMj", version, i + 1);
                    var response = await httpClient.GetAsync(parameter);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    tags.items.AddRange(JsonConvert.DeserializeObject<Tags>(responseBody).items);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            foreach (var i in tags.items)
            {
                total += i.count;
            }

            foreach (var i in tags.items)
            {
                i.percent = Math.Round(i.count / total * 100.0,4);
            }

            return View(tags.items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
