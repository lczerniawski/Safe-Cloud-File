using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;
using Newtonsoft.Json;

namespace DesktopApp_Example.Helpers
{
    public static class FileDataHelpers
    {
        public static FileData DownloadFileData(Stream jsonStream, string receiverEmail)
        {
            jsonStream.Position = 0;
            var streamReader = new StreamReader(jsonStream);
            var jsonString = streamReader.ReadToEnd();
            var fileData = JsonConvert.DeserializeObject<FileData>(jsonString);
            if (!fileData.UserKeys.ContainsKey(receiverEmail))
                throw new Exception("User can't decrypt this file!");

            return fileData;
        }
    }
}
