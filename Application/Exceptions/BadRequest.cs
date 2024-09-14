using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class BadRequest : Exception
    {
        public BadRequest(string message) : base(message) { }   

        public BadRequest(string message,  Exception innerException) : base(message, innerException) { }
    }
}
