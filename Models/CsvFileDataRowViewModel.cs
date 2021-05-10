using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public class CsvFileDataRowViewModel
    {
        public int RowIndex {get;set;}
        public List<string> ColumnDataList {get;set;}
    }
}