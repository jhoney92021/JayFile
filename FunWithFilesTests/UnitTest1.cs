using NUnit.Framework;
using System.IO;
using System;
using System.Text;

namespace FunWithFilesTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            Console.WriteLine(Directory.GetParent(path).Parent.Parent.FullName);
            path = Directory.GetParent(path).Parent.Parent.Parent.FullName;
            Console.WriteLine(path);
            //C:\Users\jhoney\Desktop\topsecret\FunWithFiles\wwwroot
            path = String.Concat(path, "/wwwroot/sampleCsvs");
            var _underTest = Directory.GetFiles(path);
            Console.WriteLine(_underTest[0].ToString());
            Console.WriteLine(_underTest[0]);

            byte[] buffer;
            var fileStream = new FileStream(_underTest[1], FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }

            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < buffer.Length; i++)  
            {  
                builder.Append(buffer[i].ToString("x2"));  
            }  
            Console.WriteLine(builder.ToString()); 

            var test = builder.ToString();

            Assert.Pass();
        }
    }
}