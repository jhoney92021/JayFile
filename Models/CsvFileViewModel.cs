using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public class CsvFileViewModel
    {
        string FileName {get;set;}        
        public CsvFileHeaderViewModel Header {get;set;}
        public List<CsvFileDataRowViewModel> DataRows {get;set;}
        public object RawFileData {get;set;}

        public void ParseDataRows(string rawNonHeaderData)
        {
            DataRows = CsvParser.ParseCsvRowsToDataRowObject(rawNonHeaderData);
        }
    }
}