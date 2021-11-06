using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClientHandler handler = new HttpClientHandler();
            //handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var httpClient = new HttpClient(handler);
            var apiUrl = ("http://api.stackexchange.com");

            //setup HttpClient
            httpClient.BaseAddress = new Uri(apiUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Tags tags = new Tags();

            //make request
            for (int i = 0; i < 10; i++)
            {
                string parameter = String.Format("/2.3/tags?page={0}&pagesize=100&order=desc&sort=popular&site=stackoverflow", i + 1);
                var response = await httpClient.GetAsync(parameter);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    tags.items.AddRange(new JavaScriptSerializer().Deserialize<Tags>(responseBody).items);
                }
            }

            double total = 0;

            foreach (var i in tags.items)
            {
                total += i.count;
            }
            ViewBag.Total = total;

            foreach (var i in tags.items)
            {
                i.percent = i.count / total * 100.0;
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
