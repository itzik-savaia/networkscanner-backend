using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScannerBackend.Models
{
    public class Result
    {
        public int status { get; set; }
        public string message { get; set; }
        public EndPoint endPoint { get; set; }

        public Result()
        {

        }
    }
}
