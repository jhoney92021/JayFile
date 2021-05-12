using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunWithFiles.Models;
using FunWithFiles.Utilities;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        public IActionResult ViewFromUrl(string target_url_list_selection, string extentsion_type, string? target_url)
        {
            if(!String.IsNullOrWhiteSpace(target_url))
            {
                ViewBag.TargetUrl = target_url;
            }
            else
            {
                    ViewBag.TargetUrl = target_url_list_selection;
            }

            if (Enum.IsDefined(typeof(SupportedFileTypes), extentsion_type))
            {
                extentsion_type += " Note: this is not currently supported ";
            }
            ViewBag.TargetExtensionType = extentsion_type;
            return View("ConfirmViewFromUrl");
        }

        [HttpPost]
        public IActionResult ConfirmViewFromUrl(string target_url, string extentsion_type)
        {
            if (Enum.IsDefined(typeof(SupportedFileTypes), extentsion_type))
            {
                ViewBag.RejectedRequest = $"Request cannot be processed as the chosen file type {extentsion_type} is not currently supported";
                return View("ConfirmViewFromUrl");
            }
            else
            {

                var readFileObject = ReadCsvWithHeaderFromWebClient(target_url);
                return View("ViewFileAsRaw", readFileObject);
            }
        }

        [HttpPost]
        public IActionResult OrderFile(string filter_by, string order_by, string order_direction, string file_type_to_view)
        {
            var viewName = $"ViewFileAs{file_type_to_view}";

            OrderData(filter_by, order_by, order_direction, file_type_to_view);

            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");

            return View(viewName, fileToView);
        }
        [HttpPost]
        public IActionResult OrdedddrFile(string filter_by, string order_by, string order_direction, string file_type_to_view)
        {
            var viewName = $"ViewFileAs{file_type_to_view}";
            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");

            if (order_by == "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRows = fileToView.DataRows.OrderByDescending(dr => dr.RowIndex).ToList();
                return View(viewName, fileToView);
            }
            if (order_by == "RowNumber" && order_direction == "Ascending")
            {
                fileToView.DataRows = fileToView.DataRows.OrderBy(dr => dr.RowIndex).ToList();
                return View(viewName, fileToView);
            }

            var indexToOrderBy = fileToView.FileHeader.Columns.IndexOf(order_by);

            if (order_by != "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRows = fileToView.DataRows
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderByDescending(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            }
            if (order_by != "RowNumber" && order_direction == "Ascending")
            {                
                fileToView.DataRows = fileToView.DataRows
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderBy(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            }            

            return View(viewName, fileToView);
        }
        [HttpPost]
        public IActionResult FileTypeViewer(string file_type_to_view)
        {
            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");
            var viewName = $"ViewFileAs{file_type_to_view}";
            return View(viewName,fileToView);
        }
        [HttpPost]
        public IActionResult DownloadFileInCurrentState(string file_type_to_download)
        {
            var viewName = $"ViewFileAs{file_type_to_download}";

            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");

            var fileBody = GetFileBodyForFileType(fileToView, file_type_to_download);

            DataFile.WriteStringToFile(fileBody, fileToView.FileHeader.FileName, file_type_to_download);

            return View(viewName,fileToView);
        }
        private FileViewModel ReadCsvWithHeaderFromWebClient(string target_url)
        {
            using WebClient client = new WebClient();
            using Stream stream = client.OpenRead(target_url);
            using StreamReader reader = new StreamReader(stream);

            String header = reader.ReadLine();
            String content = reader.ReadToEnd();

            return BuildCsvFileObject(content, header, client.ResponseHeaders);
        }

        private FileViewModel BuildCsvFileObject(string fileBody, string fileHeader, WebHeaderCollection? responseHeaders)
        {

            var fileObject = new FileViewModel();
            fileObject.FileHeader = new FileHeaderViewModel();
            fileObject.FileHeader.ParseHeader(fileHeader);
            fileObject.FileHeader.SetColumnsRaw(fileHeader);
            fileObject.FileHeader.ConvertToXml();
            fileObject.FileHeader.ConvertToJson();
            fileObject.ParseDataRows(fileBody, fileObject.FileHeader.Columns);
            fileObject.ConvertDataRowsToXml();
            fileObject.ConvertDataRowsToJson();
            fileObject.RawFileData = fileBody;


            if (responseHeaders != null)
            {
                fileObject.FileHeader.FileName = TryParseFileNameFromResponseHeaders(responseHeaders, "filename=", ".csv");
            }

            if (HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView") == null)
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

        private string GetFileBodyForFileType(FileViewModel download_file_in_current_state, string fileExtension)
        {
            switch(fileExtension)
            {
                case "Raw":
                   return ParseRowsToString(download_file_in_current_state.DataRows, download_file_in_current_state.FileHeader.ColumnsRaw);
                case "Json":
                   return ParseRowsToString(download_file_in_current_state.DataRowsJson, download_file_in_current_state.FileHeader.ColumnsJson);
                case "Xml":
                   return ParseRowsToString(download_file_in_current_state.DataRowsXml, download_file_in_current_state.FileHeader.ColumnsXml);
                default: return "";
            }
        }

        private string ParseRowsToString (List<FileDataRowViewModel> dataRows, string header)
        {
            string parsedDataRows = header;

            foreach(FileDataRowViewModel row in dataRows)
            {
                foreach(string column in row.ColumnDataList)
                {
                    parsedDataRows = String.Concat(parsedDataRows,column);
                }
            }
            return parsedDataRows;
        }
        private void OrderData(string filter_by, string order_by, string order_direction, string file_type_to_view)
        {
            switch(file_type_to_view)
            {
                case "Raw":
                   OrderRawData(filter_by, order_by,order_direction);
                   break;
                case "Json":
                   OrderJsonData(filter_by, order_by,order_direction);
                   break;
                case "Xml":
                   OrderXmlData(filter_by, order_by,order_direction);
                   break;
                default: break;
            }
        }
        private void OrderRawData(string filter_by, string order_by, string order_direction)
        {
            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");
            if (order_by == "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRows = fileToView.DataRows.OrderByDescending(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   ;
                return;
            }
            if (order_by == "RowNumber" && order_direction == "Ascending")
            {
                fileToView.DataRows = fileToView.DataRows.OrderBy(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   
                return;
            }

            var indexToOrderBy = fileToView.FileHeader.Columns.IndexOf(order_by);

            if (order_by != "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRows = fileToView.DataRows
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderByDescending(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            }
            if (order_by != "RowNumber" && order_direction == "Ascending")
            {                
                fileToView.DataRows = fileToView.DataRows
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderBy(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            } 

            HttpContext.Session.SetObjectAsJson("FileToView", fileToView);      
        }
        private void OrderJsonData(string filter_by, string order_by, string order_direction)
        {
            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");
            if (order_by == "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRowsJson = fileToView.DataRowsJson.OrderByDescending(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   ;
                return;
            }
            if (order_by == "RowNumber" && order_direction == "Ascending")
            {
                fileToView.DataRowsJson = fileToView.DataRowsJson.OrderBy(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   
                return;
            }

            var indexToOrderBy = fileToView.FileHeader.Columns.IndexOf(order_by);

            if (order_by != "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRowsJson = fileToView.DataRowsJson
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderByDescending(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            }
            if (order_by != "RowNumber" && order_direction == "Ascending")
            {                
                fileToView.DataRowsJson = fileToView.DataRowsJson
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderBy(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            } 

            HttpContext.Session.SetObjectAsJson("FileToView", fileToView);      
        }
        private void OrderXmlData(string filter_by, string order_by, string order_direction)
        {
            var fileToView = HttpContext.Session.GetObjectFromJson<FileViewModel>("FileToView");
            if (order_by == "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRowsXml = fileToView.DataRowsXml.OrderByDescending(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   ;
                return;
            }
            if (order_by == "RowNumber" && order_direction == "Ascending")
            {
                fileToView.DataRowsXml = fileToView.DataRowsXml.OrderBy(dr => dr.RowIndex).ToList();
                HttpContext.Session.SetObjectAsJson("FileToView", fileToView);   
                return;
            }

            var indexToOrderBy = fileToView.FileHeader.Columns.IndexOf(order_by);

            if (order_by != "RowNumber" && order_direction == "Descending")
            {
                fileToView.DataRowsXml = fileToView.DataRowsXml
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderByDescending(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            }
            if (order_by != "RowNumber" && order_direction == "Ascending")
            {                
                fileToView.DataRowsXml = fileToView.DataRowsXml
                                            .Where(
                                                    dr=>double.Parse(dr.ColumnDataList[indexToOrderBy]) >= double.Parse(filter_by)
                                                  )
                                            .OrderBy(dr => dr.ColumnDataList[indexToOrderBy])
                                            .ToList();
            } 

            HttpContext.Session.SetObjectAsJson("FileToView", fileToView);      
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
