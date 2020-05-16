using Dapper;
using MySql.Data.MySqlClient;
using Naandi.Shared.DataBase;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApp.Services
{
    public class UserRepository : IUser
    {
        private ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public IEnumerable<Claim> GetClaimsByByUserName(string userName)
        {
            var getUserRolesRelationByUserName = GetUserRolesRelationByUserName(userName)?.ToList();
            if (getUserRolesRelationByUserName == null)
            {
                return null;
            }

            var clamis = new List<Claim>();

            foreach (var iter in getUserRolesRelationByUserName)
            {
                clamis.Add(new Claim(ClaimTypes.Role, iter.Roles.Name));
                clamis.Add(new Claim(ClaimTypes.Name, iter.User.UserName));
                clamis.Add(new Claim(ClaimTypes.Email, iter.User.Email));
            }

            return clamis;
        }

        public IEnumerable<UserRolesRelation> GetUserRolesRelationByUserName(string userName)
        {
            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select ur.*, r.*, u.* 
                                from `UserRolesRelation` ur
                                join `Roles` r on r.Id = ur.RolesId
                                join `User` u on u.Id = ur.UserId
                                where u.UserName = @userName;";

                return connection.Query<UserRolesRelation, Roles, User, UserRolesRelation>(sql, (ur, r, u) =>
                {
                    ur.Roles = r;
                    ur.User = u;

                    return ur;

                }, param: new { userName });
            }
        }

        public bool ValidateLogin(User user)
        {
            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from `User` where `username` = @UserName and `password` = @Password";

                User result = connection.Query<User>(sql, param: new { user.UserName, user.Password }).FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}