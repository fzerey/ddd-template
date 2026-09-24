using System.Text.Json;

namespace Fzerey.DDDStarter.WebApi.Models.Exception
{
    public class ErrorModel
    {
        public string? ErrorCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
