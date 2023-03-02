using Extratoclube.Konsi.Domain.Enum.v1;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public record Notification
{
    public string? Message { get; init; }

    public NotificationType? Type { get; set; }
}
