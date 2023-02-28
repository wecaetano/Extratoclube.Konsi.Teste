using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public record Notification
{
    public string? Message { get; init; }
}
