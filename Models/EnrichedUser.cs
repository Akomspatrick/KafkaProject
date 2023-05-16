using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EnrichedEvent
    {
        public string key { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public string? Status { get; set; } = "ENRICHED";
        public string? cp { get; set; }
        public string? tealium_account { get; set; }
        public string? tealium_datasource { get; set; }
        public string? tealium_profile { get; set; }
        public string? tealium_event { get; set; }
        public string? tealium_trace_id { get; set; }

    }
}

