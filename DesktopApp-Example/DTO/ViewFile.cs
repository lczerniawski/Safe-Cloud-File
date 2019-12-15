using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.DTO
{
    public class ViewFile
    {
        public ViewFile(string id, string name, string jsonFileId)
        {
            Id = id;
            Name = name;
            JsonFileId = jsonFileId;
        }

        public string Id { get; }
        public string Name { get; }
        public string JsonFileId { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
