using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Upload;

namespace DesktopApp_Example.DTO
{
    public class UploadJsonDto
    {
        public UploadJsonDto(string id, string shareLink)
        {
            Id = id;
            ShareLink = shareLink;
        }

        public string Id { get; }
        public string ShareLink { get; }
    }
}
