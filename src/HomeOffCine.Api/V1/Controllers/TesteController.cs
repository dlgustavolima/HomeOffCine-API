using Asp.Versioning;
using HomeOffCine.Api.Controllers;
using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HomeOffCine.Api.V1.Controllers;

[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/teste")]
public class TesteController : MainController
{
    public TesteController(INotificator notificador, IUser user) : base(notificador, user)
    {
    }

    [HttpGet]
    public IActionResult Teste()
    {
        return Ok("Teste API V1");
    }
}
