using System.Collections.Generic;
using System.Security.Claims;

namespace Naandi.Shared.Services
{
    public interface IJwt
    {
        public string GenerateSecurityToken(List<Claim> claims);
    }
}