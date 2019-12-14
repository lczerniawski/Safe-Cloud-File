using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.Services
{
    public static class FileServiceFactory
    {
        private static readonly Dictionary<string, IFileService> _fileServices = new Dictionary<string, IFileService>
        {
            {"GoogleDrive", new GoogleDriveFileService()},
            {"OneDrive",new OneDriveFileService() },
            {"OwnServer",new OwnServerFileService() }
        };

        public static IFileService Create(string type)
        {
            return _fileServices[type];
        }
    }
}
