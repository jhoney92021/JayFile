using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public class CsvFileDataRowViewModel
    {
        int RowIndex {get;set;}
        string RowData {get;set;}
        List<string> Columns {get;set;}
        public List<string> ColumnDataList {get;set;}
        object FileHeaderData {get;set;}
    }
}