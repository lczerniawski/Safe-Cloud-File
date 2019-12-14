using System.Collections.Generic;

namespace DesktopApp_Example.DTO
{
    public class ValidationErrorDto
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string,string[]> Errors { get; set; }
    }
}
