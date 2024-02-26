using Microsoft.AspNetCore.Mvc;
using Starpoint.Core;

namespace Starpoint.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromServices] IInclusaoTransferenciaHandler handler, [FromBody] InclusaoTransferenciaCommand command)
        {
            handler.Handle(command);
            return Ok();
        }
    }
}