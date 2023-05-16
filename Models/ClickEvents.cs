using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public class ClickEvents //: ISerDes
    {
        public string key { get; set; }
        public string? Type { get; set; }
        public string? Events { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }

    }


    public class UserClickEvents //: ISerDes
    {
        public string key { get; set; }
        public string? Type { get; set; }
        public string? Events { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }

    }


 

}
