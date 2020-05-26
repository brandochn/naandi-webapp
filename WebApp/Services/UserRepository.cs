using Dapper;
using MySql.Data.MySqlClient;
using Naandi.Shared.DataBase;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Data;

namespace WebApp.Services
{
    public class UserRepository : IUser
    {
        private ApplicationDbContext applicationDbContext;
        private readonly ApplicationRestClient applicationRestClient;

        public UserRepository(ApplicationDbContext _applicationDbContext, ApplicationRestClient _applicationRestClient)
        {
            applicationDbContext = _applicationDbContext;
            applicationRestClient = _applicationRestClient;
        }

        public string CreateToken(string username, string password)
        {
            var client = applicationRestClient.CreateRestClient(false);
            var request = new RestRequest("/api/Token/Create", Method.POST);
            request.AddParameter("username", username, ParameterType.QueryString);
            request.AddParameter("password", password, ParameterType.QueryString);
            
            var response = client.Post(request);

            if (response.ErrorException != null || response.IsSuccessful == false)
            {
                string message = Constants.UNHANDLED_EXCEPTION_MESSAGE;
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return response.Content;
        }

        public IEnumerable<Claim> GetClaimsByByUserName(string userName)
        {
            var getUserRolesRelationByUserName = GetUserRolesRelationByUserName(userName, false)?.ToList();
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

        public IEnumerable<UserRolesRelation> GetUserRolesRelationByUserName(string userName, bool includeInactives)
        {
            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select ur.*, r.*, u.*
                                from `UserRolesRelation` ur
                                join `Roles` r on r.Id = ur.RolesId
                                join `User` u on u.Id = ur.UserId
                                where u.UserName = @userName;";

                if (includeInactives == false)
                {
                    sql = @"select ur.*, r.*, u.*
                                from `UserRolesRelation` ur
                                join `Roles` r on r.Id = ur.RolesId
                                join `User` u on u.Id = ur.UserId
                                where u.UserName = @userName and ur.active = 1;";
                }

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
                string sql = @"select * from `User` where `username` = @UserName and `password` = @Password and active = 1;";

                User result = connection.Query<User>(sql, param: new { user.UserName, user.Password }).FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return true;
            }
        }

        public User GetUserByName(string username)
        {
            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from `User` where `username` = @username";

                return connection.Query<User>(sql, param: new { username }).FirstOrDefault();
            }
        }
    }
}