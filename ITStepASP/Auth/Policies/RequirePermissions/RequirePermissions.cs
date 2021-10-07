using System.Collections.Generic;
using ASP.NETAuthITStep.Auth.Model;
using Microsoft.AspNetCore.Authorization;

namespace ITStepASP.Auth.Policies.RequirePermissions
{
    public class RequirePermissions: IAuthorizationRequirement
    {
        public List<Permission> Permissions { get; }

        public RequirePermissions(params Permission[] permissions)
        {
            Permissions = new List<Permission>(permissions);
        }
    }
}