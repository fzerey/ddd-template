using System;

namespace Fzerey.DDDStarter.Application.Common.Exceptions.Base
{
    public class ValidationException(string message, string code) : Exception(message)
    {
        public string Code { get; set; } = code;
    }
}
