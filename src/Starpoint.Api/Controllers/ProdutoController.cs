using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Starpoint.Core;

namespace Starpoint.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        [HttpPost]
        public Result Post([FromServices] IInclusaoTransferenciaHandler handler, [FromBody] InclusaoTransferenciaCommand command)
        {
            return handler.Handle(command);
        }
    }
}