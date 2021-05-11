using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunWithFiles.Models
{   
    public class CsvFileDataRowViewModel
    {
        public int RowIndex {get;set;}        
        [NotMapped]
        public List<string> ColumnDataList {get;set;}
    }
}