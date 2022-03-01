using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Model
{
    public class FileSaved
    {
     

        private byte[] FileToSave(string path)
        {
            return File.ReadAllBytes(path);
        }

        private string DisplayFile(byte[] arrayByte)
        {
            return Convert.ToBase64String(arrayByte); 
        }

    }
}
