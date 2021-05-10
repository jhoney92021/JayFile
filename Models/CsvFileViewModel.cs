using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunWithFiles.Models
{
    public class CsvFileViewModel
    {
        string FileName {get;set;}        
        [NotMapped]
        public CsvFileHeaderViewModel FileHeader {get;set;}
        [NotMapped]
        public List<CsvFileDataRowViewModel> DataRows {get;set;}
        public object RawFileData {get;set;}

        public void ParseDataRows(string rawNonHeaderData)
        {
            DataRows = CsvParser.ParseCsvRowsToDataRowObject(rawNonHeaderData);
        }
    }
}