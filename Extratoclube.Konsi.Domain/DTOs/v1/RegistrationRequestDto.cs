using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public sealed record RegistrationRequestDto
{
    public string Document { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}
