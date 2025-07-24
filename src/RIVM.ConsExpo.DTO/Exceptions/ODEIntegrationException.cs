using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Exceptions
{
    public class ODEIntegrationException : ApplicationException
    {
        public ODEIntegrationException(string message)
            : base(message)
        { }

        public ODEIntegrationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}