using System;
using System.Collections.Generic;

namespace FunWithFiles.Models
{
    public static class XmlConverter
    {
        public static string ConvertStringListToXmlString(List<string> parsedString, string tagToWrap, string? tagToWrap2 = null)
        {
            string xmlString = "";
            string openTag = $"< {tagToWrap} >";
            string closeTag = $"";

            xmlString = String.Concat(xmlString,openTag);            

            if(!String.IsNullOrWhiteSpace(tagToWrap2)){
                openTag = $"< {tagToWrap2} >";
                closeTag = $"</ {tagToWrap2} >";
            }else
            {
                openTag = "";
            };

            foreach(string element in parsedString)
            {
                xmlString = String.Concat(xmlString,openTag,element,closeTag);
            }

            closeTag = $"</ {tagToWrap} >";
            xmlString = String.Concat(xmlString,closeTag);

            return xmlString;
        }
        public static List<string> ConvertStringListToXmlStringList(List<string> parsedStringList, List<string> tagsToWrap)
        {
            var xmlStringList = new List<string>();

            string xmlString = "";
            string openTag = "";
            string closeTag = "";
            int indexOfColumnName = 0;
            foreach(string parsedString in parsedStringList)
            {
                indexOfColumnName = parsedStringList.IndexOf(parsedString);
                
                openTag = $"< {tagsToWrap[indexOfColumnName]} >";
                closeTag = $"</ {tagsToWrap[indexOfColumnName]} >";

                xmlStringList.Add(
                    String.Concat(xmlString,openTag,parsedString,closeTag)
                );
            }

            return xmlStringList;
        }

        public static List<FileDataRowViewModel> ConvertDataRowsToXml(List<FileDataRowViewModel> parsedList, List<string> tagsToWrap)
        {
            var xmlList = new List<FileDataRowViewModel>();

            foreach(FileDataRowViewModel element in parsedList)
            {
               xmlList.Add(
                    new FileDataRowViewModel(){
                        ColumnDataList = ConvertStringListToXmlStringList(element.ColumnDataList,tagsToWrap),
                        RowIndex = element.RowIndex
                    }
               );
            }

            return xmlList;
        }
    }
}