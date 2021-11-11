using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.ErrorHandler
{
    public class ExceptionHandler : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; set; }
        public ExceptionHandler(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}
