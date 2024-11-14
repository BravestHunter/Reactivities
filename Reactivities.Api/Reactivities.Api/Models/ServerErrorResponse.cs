namespace Reactivities.Api.Models
{
    public class ServerErrorResponse
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
        public string? Details { get; set; }
    }
}
