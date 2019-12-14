using System.Collections.Generic;

namespace ApiServer_Example.Domains.DTO
{
    public class ValidationErrorDto
    {
        public string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public string Title => "One or more validation errors occurred.";
        public string Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string,string[]> Errors { get; set; }
    }
}
