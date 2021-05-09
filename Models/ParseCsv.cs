using System;
using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public static class CsvParser
    {
        private const char _newLine = '\n';
        private const char _delimiter = ',';
        private static int _stringStartIndex = 0;

        public static List<string> ParseCsvRowToList(string csvRow)
        {
            var columnList = new List<string>();
            int length = csvRow.Length;

            for (int idx = 0; idx < length; idx++)
            {
                if (csvRow[idx] == _delimiter || csvRow[idx] == _newLine)
                {
                    columnList.Add(csvRow.Substring(_stringStartIndex, (idx - _stringStartIndex)));
                    _stringStartIndex = idx + 1;
                }
            }

            _stringStartIndex = 0;
            return columnList;
        }

        public static List<List<string>> ParseCsvRowsToLists(string csvRowsAsString)
        {
            var rowList = new List<List<string>>();
            int length = csvRowsAsString.Length;

            for (int idx = 0; idx < length; idx++)
            {
                if (csvRowsAsString[idx] == _newLine)
                {
                    rowList.Add(ParseCsvRowToList(csvRowsAsString.Substring(_stringStartIndex, (idx - _stringStartIndex))));
                    _stringStartIndex = idx + 1;
                }
            }
            return rowList;
        }
        public static List<CsvFileDataRowViewModel> ParseCsvRowsToDataRowObject(string csvRowsAsString)
        {
            var rowList = new List<CsvFileDataRowViewModel>();
            int length = csvRowsAsString.Length;

            for (int idx = 0; idx < length; idx++)
            {
                if (csvRowsAsString[idx] == _newLine)
                {
                    rowList.Add(
                        new CsvFileDataRowViewModel(){
                            ColumnDataList = ParseCsvRowToList(csvRowsAsString.Substring(_stringStartIndex, (idx - _stringStartIndex)))
                        }
                    );
                    _stringStartIndex = idx + 1;
                }
            }
            return rowList;
        }
    }
}