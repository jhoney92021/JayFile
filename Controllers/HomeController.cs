using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunWithFiles.Models;
using System.Net.Http;
using System.IO;
using System.Net;
using Microsoft.VisualBasic.FileIO;

namespace FunWithFiles.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DownloadFromUrl(string target_url)
        {
            Console.WriteLine("-------url-------");
            Console.WriteLine(target_url);
            Console.WriteLine("-------url-------");
            ViewBag.TargetUrl = target_url;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDownloadFromUrl(string target_url)
        {
            Console.WriteLine("-------posting url-------");
            Console.WriteLine(target_url);
            Console.WriteLine("-------posting url-------");

            StringContent httpContent = new StringContent("test");

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(target_url);
            StreamReader reader = new StreamReader(stream);

            String rawheader = reader.ReadLine();        
            String content = reader.ReadToEnd();
           
            var fileObject = new CsvFileViewModel();
            fileObject.Header = new CsvFileHeaderViewModel();
            fileObject.Header.ParseHeader(rawheader);
            fileObject.ParseDataRows(content);


            ViewBag.FileHeader = fileObject.Header.Columns;
            ViewBag.FileData = CsvParser.ParseCsvRowsToLists(content);
            return View("Results");
        }

        [HttpPost]
        public IActionResult ViewFileData()
        {
            return RedirectToAction("Results");
        }

        [HttpGet]
        public IActionResult Results()
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
