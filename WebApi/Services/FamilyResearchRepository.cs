using System;
using System.Text.Json;
using MySql.Data.MySqlClient;
using Naandi.Shared;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using WebApi.Data;

namespace WebApi.Services
{
    public class FamilyResearchRepository : IFamilyResearch
    {
        private ApplicationDbContext applicationDbContext;

        public FamilyResearchRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }
        public void Add(FamilyResearch familyResearch)
        {
            int? spouseId = AddOrUpdateSpouse(familyResearch.LegalGuardian.Spouse);
            familyResearch.LegalGuardian.SpouseId = spouseId ?? 0;

            int? legalGuardianId = AddOrUpdateLegalGuardian(familyResearch.LegalGuardian);
        }

        private int? AddOrUpdateSpouse(Spouse spouse)
        {
            int spouseId;

            if (spouse == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

            var serializedResult = JsonSerializer.Serialize(spouse, typeof(Spouse), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateSpouse";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "JSONData",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.LongText,
                    Value = serializedResult
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "SpouseId",
                    Direction = System.Data.ParameterDirection.Output,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = 0
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "ErrorMessage",
                    Direction = System.Data.ParameterDirection.Output,
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Empty
                });

                connection.Open();
                cmd.ExecuteNonQuery();
                var errorMessage = cmd.Parameters["ErrorMessage"].Value as string;
                if (string.IsNullOrEmpty(errorMessage) == false)
                {
                    if (errorMessage.Contains("45000"))
                    {
                        throw new BusinessLogicException(errorMessage);
                    }
                    throw new Exception(errorMessage);
                }

                spouseId = Convert.ToInt32(cmd.Parameters["SpouseId"].Value);
            }

            return spouseId;
        }

        private int? AddOrUpdateLegalGuardian(LegalGuardian legalGuardian)
        {
            int legalGuardianId;

            if (legalGuardian == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

            var serializedResult = JsonSerializer.Serialize(legalGuardian, typeof(LegalGuardian), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateLegalGuardian";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "JSONData",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.LongText,
                    Value = serializedResult
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "SpouseId",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = legalGuardian.SpouseId == 0 ? (int?)null : legalGuardian.SpouseId
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "LegalGuardianId",
                    Direction = System.Data.ParameterDirection.Output,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = 0
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "ErrorMessage",
                    Direction = System.Data.ParameterDirection.Output,
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Empty
                });

                connection.Open();
                cmd.ExecuteNonQuery();
                var errorMessage = cmd.Parameters["ErrorMessage"].Value as string;
                if (string.IsNullOrEmpty(errorMessage) == false)
                {
                    if (errorMessage.Contains("45000"))
                    {
                        throw new BusinessLogicException(errorMessage);
                    }
                    throw new Exception(errorMessage);
                }

                legalGuardianId = Convert.ToInt32(cmd.Parameters["LegalGuardianId"].Value);
            }

            return legalGuardianId;
        }
    }
}