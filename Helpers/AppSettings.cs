using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPApp.Helpers
{
    public class AppSettings
    {
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; } //in mins
        public string Secret { get; set; }
    }
}
