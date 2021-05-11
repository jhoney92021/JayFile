using System;
using System.Collections.Generic;
using System.Linq;

namespace FunWithFiles.Models
{
    public static class CsvSorter
    {
        public static Dictionary<int, string> SortCsvRowDictionary()
        {
            return new Dictionary<int, string>();
        }

        public static List<CsvFileDataRowColumnViewModel> SortRowList(List<CsvFileDataRowColumnViewModel> readCsvFile, int orderByIdx)
        {

            Console.WriteLine("-------Order File --------");

            var orderedCsvFile = new List<CsvFileDataRowColumnViewModel>();

            // int comparerIdx = 0;

            // for (int rowIdx = 0; rowIdx <= readCsvFile.Count; rowIdx++)
            // {
            //     if ((int)readCsvFile[rowIdx].ColumnData[orderByIdx] > (int)readCsvFile[rowIdx].ColumnData[comparerIdx])
            //     {
            //         orderedCsvFile.Add(
            //             row
            //         );
            //     }
                // if((int)row.ColumnData[orderByIdx] > (int)firstElement.ColumnData)
                // {
                //     orderedCsvFile.Add(
                //         row
                //     );
                // }
            //}

            //readCsvFile.DataRows = readCsvFile.DataRows


            return orderedCsvFile;
        }
    }
}