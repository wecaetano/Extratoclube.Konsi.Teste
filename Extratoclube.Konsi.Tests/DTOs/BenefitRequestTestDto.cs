using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Tests.DTOs;
public class BenefitRequestTestDto
{
    public string Document { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public IEnumerable<string> Results { get; set; }
}
