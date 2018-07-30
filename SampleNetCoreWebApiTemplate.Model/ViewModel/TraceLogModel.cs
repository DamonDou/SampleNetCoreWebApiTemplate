using System;
using System.Collections.Generic;
using System.Text;

namespace SampleNetCoreWebApiTemplate.Model.ViewModel
{
    public class TraceLogModel
    {
        public string ClientIp { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Parameter { get; set; }
        public string ResponseContent { get; set; }
        public int ConsumeTime { get; set; }
        public DateTime RequestDate { get; set; }
        public string Type { get; set; }
        public int HttpStatusCode { get; set; }
    }
}
