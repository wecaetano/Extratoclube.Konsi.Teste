using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public class CustomApiResponse<T>
{
    public object? Content { get; init; }

    public long Timestamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

    public HttpStatusCode Status { get; set; }

    public IEnumerable<Notification> Notifications { get; set; } = new List<Notification>();
}
