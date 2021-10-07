using System.Linq;
using System.Threading.Tasks;
using ASP.NETAuthITStep.Auth.Model;
using Microsoft.AspNetCore.Authorization;

namespace ITStepASP.Auth.Policies.RequirePermissions
{
    public class RequirePermissionHandler: AuthorizationHandler<RequirePermissions>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirePermissions requirement)
        {
            var principal = context.User;

            var principalPermissionSet = principal.Claims
                .Where(c => c.Type == MyClaimTypes.Permissions)
                .Select(c => c.Value)
                .ToHashSet();

            var principalCompliesRequirements = principalPermissionSet
                .IsSupersetOf(requirement.Permissions.Select(p => p.ToString()));

            if (principalCompliesRequirements)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}