using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Documents
{
    public class ResponseModel
    {
        public bool isSuccess { get; set; }
        public int code { get; set; }
        public string responseSuccess { get; set; }
        public string responseFailed { get; set; }
    }
}
