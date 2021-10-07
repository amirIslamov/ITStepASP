using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ASP.NETAuthITStep.Auth;
using ASP.NETAuthITStep.Auth.Model;
using ITStepASP.Auth.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITStepASP.Auth.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class LoginController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private JwtService _jwtService;
        private UserClaimsPrincipalFactory<IdentityUser> _principalFactory;

        public LoginController(
            UserManager<IdentityUser> userManager,
            JwtService jwtService,
            UserClaimsPrincipalFactory<IdentityUser> principalFactory)
            => (_userManager, _jwtService, _principalFactory) = (userManager, jwtService, principalFactory);

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = new IdentityUser(request.UserName);

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest();
            }

            var principal = await _principalFactory.CreateAsync(user);
            var token = _jwtService.CreateToken(principal);

            return new LoginResponse()
            {
                Token = token
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _userManager.CreateAsync(
                new IdentityUser()
                {
                    UserName = request.UserName,
                    Email = request.Email
                },
                request.Password
            );

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            if (request.UserName == "admin")
            {
                await _userManager.AddClaimAsync(
                    new IdentityUser(request.UserName),
                    new Claim("Permissions", Permission.ExtendedAccess.ToString())
                );
            }


            return Ok();
        }
    }
}