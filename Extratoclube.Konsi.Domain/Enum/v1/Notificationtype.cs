using System.ComponentModel;

namespace Extratoclube.Konsi.Domain.Enum.v1;
public enum NotificationType
{
    [Description("ValidationError")]
    ValidationError,
    [Description("Success")]
    Success,
    [Description("Warn")]
    Warn,
    [Description("NotFound")]
    NotFound
}
