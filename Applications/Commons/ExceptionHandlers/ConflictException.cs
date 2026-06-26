using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Commons.ExceptionHandlers
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}
