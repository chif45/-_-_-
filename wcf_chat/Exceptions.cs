using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcf_chat
{
    internal class Exceptions
    {
        public class NotFound : Exception

        {
            public NotFound() { }
            public NotFound(string message) : base(message) { }
            public NotFound(string message, Exception innerException)
                : base(message, innerException) { }

        }

        public class WrongPassword : Exception

        {
            public WrongPassword() { }
            public WrongPassword(string message) : base(message) { }
            public WrongPassword(string message, Exception innerException)
                : base(message, innerException) { }

        }
        public class AlreadyExsist : Exception

        {
            public AlreadyExsist() { }
            public AlreadyExsist(string message) : base(message) { }
            public AlreadyExsist(string message, Exception innerException)
                : base(message, innerException) { }

        }
    }
}
