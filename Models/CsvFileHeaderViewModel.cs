using System;
using System.Collections.Generic;


namespace FunWithFiles.Models
{
    public class CsvFileHeaderViewModel
    {
        public string FileName { get; set; }
        public List<string> Columns { get; set; }
        object FileHeaderData { get; set; }

        public void ParseHeader(string headerRow)
        {
            Columns = CsvParser.ParseCsvRowToList(headerRow);
        }

    }
}