
namespace Fzerey.DDDStarter.Application.Common.Exceptions.Base
{
    public class NotFoundException(string message, string code) : Exception(message)
    {
        public string Code { get; set; } = code;
        
    }
}
