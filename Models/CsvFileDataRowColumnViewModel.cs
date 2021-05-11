using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunWithFiles.Models
{
    public class CsvFileDataRowColumnViewModel
    {
        public int ColumnIdx {get;set;}
        public string ColumnName {get;set;}
        public string ColumnData {get;set;}
    } 
}