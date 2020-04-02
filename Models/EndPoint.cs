using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScannerBackend.Models
{
    public class EndPoint
    {
        public string ip { get; set; }
        public bool isUP { get; set; }
        public string ms { get; set; }

        public EndPoint()
        {

        }

    }
}
