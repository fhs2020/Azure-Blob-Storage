using AzureFunctionFHSWeb.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;

namespace AzureFunctionFHSWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static readonly HttpClient _httpClient = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            return View();
        }

        // http://localhost:7071/api/OnSalesUploadWriteToQueue

        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest)
        {
            salesRequest.Id = Guid.NewGuid().ToString();


            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest),
                System.Text.Encoding.UTF8, "Application/json"))
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:7071/api/OnSalesUploadWriteToQueue", content);

                string returnValue = response.Content.ReadAsStringAsync().Result;
            };


            return RedirectToAction(nameof(Index));
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