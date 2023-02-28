using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Extratoclube.Konsi.Api.Controllers.v1;
[Route("api/v1/registration")]
[Produces("application/json")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly ICrawlerService _crawlerService;

    public RegistrationController(
        ILogger<RegistrationController> logger, 
        ICrawlerService crawlerService)
    {
        _logger = logger;
        _crawlerService = crawlerService;
    }

    [HttpPost("benefits")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetBenefitsAsync([FromBody] RegistrationRequestDto dto)
    {
        _crawlerService.CrawlerAsync();
        return Ok();
    }
}
