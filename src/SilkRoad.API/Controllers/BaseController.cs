using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _uow;

        protected BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}
