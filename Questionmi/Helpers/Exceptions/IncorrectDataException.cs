using System;

namespace Questionmi.Helpers.Exceptions
{
    public class IncorrectDataException : Exception
    {
        public IncorrectDataException(string message) : base(message)
        {
        }
    }
}
