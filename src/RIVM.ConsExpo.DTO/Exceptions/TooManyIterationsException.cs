using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Exceptions
{
    public class TooManyIterationsException : Exception
    {
        public TooManyIterationsException(string message) : base(message)
        { }

        public TooManyIterationsException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}