using Microsoft.AspNetCore.Mvc;

namespace Megaphone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public string GetData()
        {
            return "Data OK";
        }
    }
}