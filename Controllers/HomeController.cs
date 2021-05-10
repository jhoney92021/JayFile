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

namespace FunWithFiles.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ViewFromUrl(string target_url, string extentsion_type)
        {            
            ViewBag.TargetUrl = target_url;
            if(Enum.IsDefined(typeof(SupportedFileTypes), extentsion_type)){
                extentsion_type += " Note: this is not currently supported ";
            }
            ViewBag.TargetExtensionType = extentsion_type;
            return View("ConfirmViewFromUrl");
        }

        [HttpPost]
        public IActionResult ConfirmViewFromUrl(string target_url, string extentsion_type)
        {
            if(Enum.IsDefined(typeof(SupportedFileTypes), extentsion_type)){
                ViewBag.RejectedRequest = $"Request cannot be processed as the chosen file type {extentsion_type} is not currently supported";
                return View("ConfirmViewFromUrl");
            }
            else{

            var readFileObject = ReadCsvWithHeaderFromWebClient(target_url);
            return View("FileToView", readFileObject);
            }
        }

        [HttpPost]
        public IActionResult OrderFile(string order_by, CsvFileViewModel readFileObject){
            
            Console.WriteLine(order_by);
            var indexToOrderBy = readFileObject.FileHeader.Columns.IndexOf(order_by);
            Console.WriteLine(indexToOrderBy);

           readFileObject.DataRows = (List<CsvFileDataRowViewModel>)readFileObject.DataRows.OrderBy(dr => dr.ColumnDataList[indexToOrderBy]);
            // readFileObject.DataRows = from dr in readFileObject.DataRows
            //                             orderby dr.ColumnDataList.(indexToOrderBy);
            return View("FileToView", readFileObject);

        }
        private CsvFileViewModel ReadCsvWithHeaderFromWebClient(string target_url)
        {

            using WebClient client = new WebClient();
            using Stream stream = client.OpenRead(target_url);
            using StreamReader reader = new StreamReader(stream);

            String header = reader.ReadLine();
            String content = reader.ReadToEnd();            

            return BuildCsvFileObject(content, header, client.ResponseHeaders);

        }

        private CsvFileViewModel BuildCsvFileObject(string fileBody, string fileHeader, WebHeaderCollection? responseHeaders)
        {

            var fileObject = new CsvFileViewModel();
            fileObject.FileHeader = new CsvFileHeaderViewModel();
            fileObject.FileHeader.ParseHeader(fileHeader);
            fileObject.ParseDataRows(fileBody);


            if (responseHeaders != null)
            {
                fileObject.FileHeader.FileName = TryParseFileNameFromResponseHeaders(responseHeaders, "filename=", ".csv");
            }

            return fileObject;

        }

        private string TryParseFileNameFromResponseHeaders(WebHeaderCollection responseHeaders, string? searchValue = "filename=", string? fileExtension = ".csv")
        {
            var content_Disposition = responseHeaders.Get("Content-Disposition").ToString();
            var filenameIdx = content_Disposition.IndexOf(searchValue) + searchValue.Length;
            var extentsionIdx = content_Disposition.IndexOf(fileExtension) + fileExtension.Length;
            string fileName = content_Disposition.Substring(filenameIdx, extentsionIdx - filenameIdx);

            return fileName;

        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
