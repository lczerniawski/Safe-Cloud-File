using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.DTO
{
    public class SharedDownload
    {
        public SharedDownload(MemoryStream memoryStream, string fileName)
        {
            MemoryStream = memoryStream;
            FileName = fileName;
        }

        public MemoryStream MemoryStream { get; }
        public string FileName { get;}
    }
}
