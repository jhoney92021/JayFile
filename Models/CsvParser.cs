using System;
using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public static class CsvParser
    {
        private const char _newLine = '\n';
        private const char _delimiter = ',';

        public static List<string> ParseCsvRowToList(string csvRow)
        {
            var columnList = new List<string>();
            var lengthToParse = csvRow.Length;
            var stringStartIndex = 0;

            for (int idx = 0; idx < lengthToParse; idx++)
            {
                if (csvRow[idx] == _delimiter || csvRow[idx] == _newLine)
                {
                    columnList.Add(csvRow.Substring(stringStartIndex, (idx - stringStartIndex)));
                    stringStartIndex = idx + 1;
                }
            }

            return columnList;
        }

        public static List<List<string>> ParseCsvRowsToLists(string csvRowsAsString)
        {
            var rowList = new List<List<string>>();
            var lengthToParse = csvRowsAsString.Length;
            var stringStartIndex = 0;

            for (int idx = 0; idx < lengthToParse; idx++)
            {
                if (csvRowsAsString[idx] == _newLine)
                {
                    rowList.Add(ParseCsvRowToList(csvRowsAsString.Substring(stringStartIndex, (idx - stringStartIndex))));
                    stringStartIndex = idx + 1;
                }
            }

            return rowList;
        }
        public static List<CsvFileDataRowColumnViewModel> ParseCsvRowToCsvFileDataRowColumnList(string csvRow, int parentRowIdx, List<string> columnNames)
        {
            var columnList = new List<CsvFileDataRowColumnViewModel>();
            var lengthToParse = csvRow.Length;
            var stringStartIndex = 0;
            int columnIdx = 0;

            for (int idx = 0; idx < lengthToParse; idx++)
            {
                if (csvRow[idx] == _delimiter || csvRow[idx] == _newLine)
                {
                    columnList.Add(
                        new CsvFileDataRowColumnViewModel()
                        {
                            ParentRowIdx = parentRowIdx,
                            ColumnIdx = columnIdx,
                            ColumnName = columnNames[columnIdx],
                            ColumnData = csvRow.Substring(stringStartIndex, (idx - stringStartIndex))
                        }
                    );
                    columnIdx++;
                    stringStartIndex = idx + 1;
                }
            }

            return columnList;
        }
        public static List<CsvFileDataRowViewModel> ParseCsvRowsToDataRowObject(string csvRowsAsString, List<string> columnNames)
        {
            var rowList = new List<CsvFileDataRowViewModel>();
            var lengthToParse = csvRowsAsString.Length;
            var stringStartIndex = 0;

            for (int idx = 0; idx < lengthToParse; idx++)
            {
                if (csvRowsAsString[idx] == _newLine)
                {
                    rowList.Add(
                        new CsvFileDataRowViewModel(){
                            ColumnDataList = ParseCsvRowToList(csvRowsAsString.Substring(stringStartIndex, (idx - stringStartIndex))),
                            RowIndex = rowList.Count + 1
                        }
                    );
                    stringStartIndex = idx + 1;
                }
            }

            return rowList;
        }
    }
}