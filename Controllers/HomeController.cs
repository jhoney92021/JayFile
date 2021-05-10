using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunWithFiles.Models;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
        public IActionResult OrderFile(string order_by)
        {
            var fileToView = HttpContext.Session.GetObjectFromJson<CsvFileViewModel>("FileToView");
            if(order_by == "RowNumber"){
                fileToView.DataRows = (List<CsvFileDataRowViewModel>)fileToView.DataRows.OrderBy(dr => dr.RowIndex);
                return View("FileToView", fileToView);
            }

            Console.WriteLine(order_by);
            Console.WriteLine(fileToView.FileHeader.FileName);
            var indexToOrderBy = fileToView.FileHeader.Columns.IndexOf(order_by);
            Console.WriteLine(indexToOrderBy);

           fileToView.DataRows = (List<CsvFileDataRowViewModel>)fileToView.DataRows.OrderBy(dr => dr.ColumnDataList[indexToOrderBy]);
            // readFileObject.DataRows = from dr in readFileObject.DataRows
            //                             orderby dr.ColumnDataList.(indexToOrderBy);
            return View("FileToView", fileToView);

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

            if (HttpContext.Session.GetObjectFromJson<CsvFileViewModel>("FileToView") == null)
            {
                HttpContext.Session.SetObjectAsJson("FileToView", fileObject);
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

    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes the object to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
