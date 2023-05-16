using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Enriched
    {
        public string key { get; set; }
        public string? Type { get; set; }
        public string? Events { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? jobName { get; set; }
        public string? jobCode { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public string? Status { get; set; } = "ENRICHED";
    }
}
