using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FunWithFiles.Models
{
    public class CsvFileHeaderViewModel
    {
        public string FileName { get; set; }
        [NotMapped]
        public List<string> Columns { get; set; }
        object FileHeaderData { get; set; }

        public void ParseHeader(string headerRow)
        {
            Columns = CsvParser.ParseCsvRowToList(headerRow);
        }

    }
}