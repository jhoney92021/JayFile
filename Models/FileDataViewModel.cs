using System.Collections.Generic;
using FunWithFiles.Utilities;

namespace FunWithFiles.Models
{
    public class FileDataViewModel
    {
        public FileDataHeaderViewModel FileHeader {get;set;}
        public List<FileDataRowViewModel> DataRows {get;set;}
        public List<FileDataRowViewModel> DataRowsJson {get;set;}
        public List<FileDataRowViewModel> DataRowsXml {get;set;}
        public string RawFileData {get;set;}
        public void ParseDataRows(string rawNonHeaderData, List<string> columnDefs)
        {
            DataRows = CsvParser.ParseCsvRowsToDataRowObject(rawNonHeaderData, columnDefs);
        }
        public void ConvertDataRowsToXml()
        {
            DataRowsXml = XmlConverter.ConvertDataRowsToXml(DataRows,FileHeader.Columns);
        }
        public void ConvertDataRowsToJson()
        {
            DataRowsJson = JsonConverter.ConvertDataRowsToJson(DataRows,FileHeader.Columns);
        }
    }
}