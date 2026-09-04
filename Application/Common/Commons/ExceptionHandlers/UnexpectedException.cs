using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commons.ExceptionHandlers
{
    public class UnexpectedException : Exception
    {
        public UnexpectedException() { }

        public UnexpectedException(string message) : base(message) { }
    }
}
