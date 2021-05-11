using System;
using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public static class JsonConverter
    {
        public static string ConvertListToJson(List<string> parsedString, string property, string? property2 = null)
        {
            string jsonString = "";
            string openTag = String.Concat("{" , $"{property} : ");
            string closeTag = "";

            jsonString = String.Concat(jsonString,openTag);            

            if(!String.IsNullOrWhiteSpace(property2)){
                openTag = String.Concat("{" , $"{property2} : ");
                closeTag = "}";
            }else
            {
                openTag = "";
            };

            foreach(string element in parsedString)
            {
                jsonString = String.Concat(jsonString,openTag,element,closeTag);
            }

            closeTag = "}";
            jsonString = String.Concat(jsonString,closeTag);

            return jsonString;
        }

        public static List<string> ConvertStringListToJsonStringList(List<string> parsedStringList, List<string> propertyToWrap)
        {
            var jsonStringList = new List<string>();

            string jsonString = "";
            string openTag = "";
            string closeTag = "}";

            int indexOfColumnName = 0;
            
            foreach(string parsedString in parsedStringList)
            {
                indexOfColumnName = parsedStringList.IndexOf(parsedString);
                
                openTag = String.Concat("{" , $"{propertyToWrap[indexOfColumnName]} : ");            

                jsonStringList.Add(
                    String.Concat(jsonString,openTag,parsedString,closeTag)
                );
            }

            return jsonStringList;
        }

        public static List<FileDataRowViewModel> ConvertDataRowsToJson(List<FileDataRowViewModel> parsedList, List<string> tagsToWrap)
        {
            var jsonList = new List<FileDataRowViewModel>();

            foreach(FileDataRowViewModel element in parsedList)
            {
               jsonList.Add(
                    new FileDataRowViewModel(){
                        ColumnDataList = ConvertStringListToJsonStringList(element.ColumnDataList,tagsToWrap),
                        RowIndex = element.RowIndex
                    }
               );
            }

            return jsonList;
        }
    }
}