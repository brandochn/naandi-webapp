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

            int? addressId = AddOrUpdateAddress(familyResearch.LegalGuardian.Address);

            int? legalGuardianId = AddOrUpdateLegalGuardian(familyResearch.LegalGuardian, spouseId, addressId);

            int? formalEducationId = AddOrUpdateFormalEducation(familyResearch.Minor.FormalEducation);

            int? minorId = AddOrUpdateMinor(familyResearch.Minor, formalEducationId);

            int? previousFoundationId = AddOrUpdatePreviousFoundation(familyResearch.PreviousFoundation);

            int? familyHealthId = AddOrUpdateFamilyHealth(familyResearch.FamilyHealth);
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
                IgnoreNullValues = true
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

        private int? AddOrUpdateLegalGuardian(LegalGuardian legalGuardian, int? spouseId, int? addressId)
        {
            int legalGuardianId;

            if (legalGuardian == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
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
                    Value = spouseId
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "AddressId",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = addressId
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

        private int? AddOrUpdateAddress(Address address)
        {
            int addressId;

            if (address == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(address, typeof(Address), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateAddress";
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
                    ParameterName = "AddressId",
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

                addressId = Convert.ToInt32(cmd.Parameters["AddressId"].Value);
            }

            return addressId;
        }

        private int? AddOrUpdateFormalEducation(FormalEducation formalEducation)
        {
            int formalEducationId;

            if (formalEducation == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(formalEducation, typeof(FormalEducation), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateFormalEducation";
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
                    ParameterName = "FormalEducationId",
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

                formalEducationId = Convert.ToInt32(cmd.Parameters["FormalEducationId"].Value);
            }

            return formalEducationId;
        }

        private int? AddOrUpdateMinor(Minor minor, int? formalEducationId)
        {
            int minorId;

            if (minor == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(minor, typeof(Minor), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateMinor";
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
                    ParameterName = "FormalEducationId",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = formalEducationId
                });

                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "MinorId",
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

                minorId = Convert.ToInt32(cmd.Parameters["MinorId"].Value);
            }

            return minorId;
        }

        private int? AddOrUpdatePreviousFoundation(PreviousFoundation previousFoundation)
        {
            int previousFoundationId;

            if (previousFoundation == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(previousFoundation, typeof(PreviousFoundation), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdatePreviousFoundation";
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
                    ParameterName = "PreviousFoundationId",
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

                previousFoundationId = Convert.ToInt32(cmd.Parameters["PreviousFoundationId"].Value);
            }

            return previousFoundationId;
        }

        private int? AddOrUpdateFamilyHealth(FamilyHealth familyHealth)
        {
            int familyHealthId;

            if (familyHealth == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(familyHealth, typeof(FamilyHealth), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateFamilyHealth";
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
                    ParameterName = "FamilyHealthId",
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

                familyHealthId = Convert.ToInt32(cmd.Parameters["FamilyHealthId"].Value);
            }

            return familyHealthId;
        }
    }
}