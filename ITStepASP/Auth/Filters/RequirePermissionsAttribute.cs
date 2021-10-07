using System.Collections.Generic;
using System.Linq;
using ASP.NETAuthITStep.Auth.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITStepASP.Auth.Filters
{
    public class RequirePermissionsAttribute: ActionFilterAttribute
    {
        private List<Permission> _requiredPermissions;

        public RequirePermissionsAttribute(params Permission[] requiredPermissions)
        {
            _requiredPermissions = new List<Permission>(requiredPermissions);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var principal = context.HttpContext.User;

            var principalPermissionSet = principal.Claims
                .Where(c => c.Type == "Permissions")
                .Select(c => c.Value)
                .ToHashSet();

            var principalCompliesPermissions = principalPermissionSet
                .IsSupersetOf(
                    _requiredPermissions.Select(p => p.ToString())
                );

            if (principalCompliesPermissions)
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = new ForbidResult();
        }
    }
}