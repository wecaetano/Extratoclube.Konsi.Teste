using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Enum.v1;
using Microsoft.AspNetCore.Mvc;

namespace Extratoclube.Konsi.Api.Controllers;

public class BaseController : ControllerBase
{
    public BaseController()
    {
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    private IActionResult FormatResponseBySuccessFlow(int statusCode, CustomApiResponse response)
    {
        if (statusCode < 200 || statusCode == 202 || statusCode == 304)
        {
            return StatusCode(statusCode);
        }

        return StatusCode(statusCode, response);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult ReturnFromNotifications(CustomApiResponse response)
    {
        if (response.Notifications.Any((Notification x) => x.Type == NotificationType.NotFound))
        {
            return NotFound(response);
        }

        if (response.Notifications.Any((Notification x) => x.Type == NotificationType.ValidationError))
        {
            return StatusCode(400, response);
        }

        return FormatResponseBySuccessFlow(200, response);
    }
}
