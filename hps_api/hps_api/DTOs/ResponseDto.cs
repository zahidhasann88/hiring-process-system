using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hps_api.DTOs
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Payload { get; set; }
    }
}
