using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API;

namespace MyApp.Namespace
{
    [Route("error/{StatusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [HttpGet]
        public IActionResult Error(int StatusCode)
        {
            return new ObjectResult(new APIResponse(StatusCode));
        }

    }

}
