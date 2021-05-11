using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FunWithFiles.Models
{
    public class CsvFileViewModel
    {
        [NotMapped]
        public CsvFileHeaderViewModel FileHeader {get;set;}
        [NotMapped]
        public List<CsvFileDataRowViewModel> DataRows {get;set;}
        public string RawFileData {get;set;}

        public void ParseDataRows(string rawNonHeaderData, List<string> columnDefs)
        {
            DataRows = CsvParser.ParseCsvRowsToDataRowObject(rawNonHeaderData, columnDefs);
        }

        public void OrderDataRowsByIndex(int indexToOrderBy)
        {
            //DataRows = DataRows.OrderBy(dr => dr.ColumnDataList[0].ColumnIdx == indexToOrderBy).ToList();
        }
        public void OrderDataRowsByRowNumber(int rowNumber)
        {
            DataRows = DataRows.OrderBy(dr => dr.RowIndex).ToList();
        }
    }
}