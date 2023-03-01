using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
