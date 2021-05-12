using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FunWithFiles.Models
{
    public class FileHeaderViewModel
    {
        public string FileName { get; set; }
        public List<string> Columns { get; set; }
        public string ColumnsXml { get; set; }
        public string ColumnsRaw { get; set; }
        public string ColumnsJson { get; set; }
        public void ParseHeader(string headerRow)
        {
            Columns = CsvParser.ParseCsvRowToList(headerRow);
            ColumnsRaw = headerRow;
        }

        public void ConvertToXml()
        {
            ColumnsXml = XmlConverter.ConvertStringListToXmlString(Columns, "header", "column");
        }
        public void ConvertToJson()
        {
            ColumnsJson = JsonConverter.ConvertListToJson(Columns, "header");
        }
    }
}