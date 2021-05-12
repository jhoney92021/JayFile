using System.IO;
using Microsoft.Win32;
using System;
using System.Text;

namespace FunWithFiles.Utilities
{
    public static class FileWriterUtility
    {
        public static void WriteStringToFile(string fileData, string? fileName = "noName", string? fileExtension = ".txt")
        {
            var fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string downloadsPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
            downloadsPath = String.Concat(downloadsPath, @"\", $"{fileNameNoExt}.{fileExtension}");

            UnicodeEncoding uniEncoding = new UnicodeEncoding();

            using (MemoryStream ms = new MemoryStream())
            {

                using (var sw = new StreamWriter(ms, uniEncoding))
                {
                    sw.Write(fileData);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    using (FileStream file = new FileStream(downloadsPath, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }
            }
        }
    }
}