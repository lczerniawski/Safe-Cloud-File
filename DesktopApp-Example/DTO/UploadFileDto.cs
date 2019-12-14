using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Upload;

namespace DesktopApp_Example.DTO
{
    public class UploadFileDto
    {
        public UploadFileDto(string id, UploadStatus uploadStatus)
        {
            Id = id;
            UploadStatus = uploadStatus;
        }

        public string Id { get; }
        public UploadStatus UploadStatus { get; }
    }
}
