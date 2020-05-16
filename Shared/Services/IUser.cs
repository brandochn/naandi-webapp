using Naandi.Shared.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Naandi.Shared.Services
{
    public interface IUser
    {
        bool ValidateLogin(User user);

        IEnumerable<UserRolesRelation> GetUserRolesRelationByUserName(string userName);

        IEnumerable<Claim> GetClaimsByByUserName(string userName);
    }
}