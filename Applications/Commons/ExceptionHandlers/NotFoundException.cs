using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.ExceptionHandlers
{
    public class NotFoundException : ServerException
    {
        public NotFoundException(string message)
            : base(message, HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" with key ({key}) was not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
