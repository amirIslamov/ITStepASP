using System.Threading.Tasks;
using ASP.NETAuthITStep.Auth.Model;
using ITStepASP.Auth.Filters;
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

        [HttpGet("extended")]
        [RequirePermissions(Permission.ExtendedAccess)]
        public async Task<IActionResult> GetWithPermissions()
        {
            return Ok();
        }
    }
}