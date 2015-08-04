using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EvilDuck.Framework.Core.Security
{
    public static class PrincipalExtensions
    {
        public static string GetId(this IPrincipal principal)
        {
            var p = principal as ClaimsPrincipal;
            if (p == null) return String.Empty;

            return p.FindFirst("EvilDuck.Id").Value;
        }
    }
}
