using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunWithFiles.Models
{
    public class CsvFileDataModel
    {
        [Required]
        [Column(name:"FileId", TypeName="pk")]
        [Key]
        int Id {get;set;}
        public CsvFileHeaderViewModel FileHeader {get;set;}
        public List<FileDataRowViewModel> DataRows {get;set;}
        [Required]
        [Column(name:"RawHeaderFileData")]
        public string RawHeaderFileData {get;set;}
        [Required]
        [Column(name:"RawFileData")]
        public string RawFileData {get;set;}
        string FileName {get;set;}

        // public void ParseDataRows(string rawNonHeaderData)
        // {
        //     DataRows = CsvParser.ParseCsvRowsToDataRowObject(rawNonHeaderData);
        // }
    }
}