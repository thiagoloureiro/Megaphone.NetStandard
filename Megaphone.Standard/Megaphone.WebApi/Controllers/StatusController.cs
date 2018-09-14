using Megaphone.Core;
using Microsoft.AspNetCore.Mvc;

namespace Megaphone.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public string GetStatus()
        {
            Logger.Information("OK");
            return "ok";
        }
    }
}