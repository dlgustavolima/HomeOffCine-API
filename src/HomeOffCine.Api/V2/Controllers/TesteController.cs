using Asp.Versioning;
using HomeOffCine.Api.Controllers;
using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HomeOffCine.Api.V2.Controllers;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/teste")]
public class TesteController : MainController
{
    public TesteController(INotificator notificador, IUser user) : base(notificador, user)
    {
    }

    [HttpGet]
    public IActionResult Teste()
    {
        return Ok("Teste API V2");
    }
}
