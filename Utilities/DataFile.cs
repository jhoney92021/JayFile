using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;

namespace FunWithFiles.Utilities
{
    public static class DataFile
    {
        private const string _defaultFileName = "noName";
        public static async Task DownloadFile(string fileName, string fileBody)
        {
            await File.WriteAllTextAsync($"{fileName}", fileBody);
        }

        public static void DownloadFileToLocal(string filePath, string fileName)
        {
            using WebClient client = new WebClient();
            client.DownloadFile(filePath ,fileName);	
        }

        public static void WriteStringToFile(string fileData, string? fileName = "noName", string? fileExtension = ".txt")
        {
            using (MemoryStream ms = new MemoryStream()){
                using (FileStream file = new FileStream($"{fileName}.{fileExtension}", FileMode.Create, System.IO.FileAccess.Write)) {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                }
            }
        }
    }
}