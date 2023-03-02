using System.Net;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public class CustomApiResponse
{
    public object? Result { get; init; }

    public long Timestamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

    public HttpStatusCode Status { get; set; }

    public List<Notification> Notifications { get; set; } = new List<Notification>();
}
