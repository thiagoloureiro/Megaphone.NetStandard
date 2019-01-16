using Megaphone.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Megaphone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KVController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get(string key)
        {
            var str = await Cluster.KvGetAsync<string>(key);
            return Ok(str);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Set(string key, string value)
        {
            await Cluster.KvPutAsync(key, value);

            return Ok();
        }
    }
}