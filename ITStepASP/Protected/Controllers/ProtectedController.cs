using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITStepASP.Protected.Controllers
{
    [ApiController]
    [Route("api/v1/protected")]
    [Authorize]
    public class ProtectedController: ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}