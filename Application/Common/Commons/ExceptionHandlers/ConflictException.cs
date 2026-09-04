using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commons.ExceptionHandlers
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}
