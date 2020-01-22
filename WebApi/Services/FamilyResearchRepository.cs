using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Transactions;
using Dapper;
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

        public void Add(FamilyResearch _familyResearch)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int? spouseId = AddOrUpdateSpouse(_familyResearch.LegalGuardian.Spouse);

                int? addressId = AddOrUpdateAddress(_familyResearch.LegalGuardian.Address);

                int? legalGuardianId = AddOrUpdateLegalGuardian(_familyResearch.LegalGuardian, spouseId, addressId);

                int? formalEducationId = AddOrUpdateFormalEducation(_familyResearch.Minor.FormalEducation);

                int? minorId = AddOrUpdateMinor(_familyResearch.Minor, formalEducationId);

                int? previousFoundationId = AddOrUpdatePreviousFoundation(_familyResearch.PreviousFoundation);

                int? familyHealthId = AddOrUpdateFamilyHealth(_familyResearch.FamilyHealth);

                int? familyMembersId = AddOrUpdateFamilyMembers(_familyResearch.FamilyMembers);

                int? socioEconomicStudyId = AddOrUpdateSocioEconomicStudy(_familyResearch.SocioEconomicStudy);

                int? districtId = AddOrUpdateDistrict(_familyResearch.District);

                int? economicSituationId = AddOrUpdateEconomicSituation(_familyResearch.EconomicSituation);

                int? familyNutritionId = AddOrUpdateFamilyNutrition(_familyResearch.FamilyNutrition);

                int? benefitsProvidedId = AddOrUpdateBenefitsProvided(_familyResearch.BenefitsProvided);

                int? ingresosEgresosMensualesId = AddOrUpdateIngresosEgresosMensuales(_familyResearch.IngresosEgresosMensuales);

                _familyResearch.LegalGuardianId = legalGuardianId ?? default;
                _familyResearch.MinorId = minorId ?? default;
                _familyResearch.PreviousFoundationId = previousFoundationId ?? default;
                _familyResearch.FamilyHealthId = familyHealthId ?? default;
                _familyResearch.FamilyMembersId = familyMembersId ?? default;
                _familyResearch.SocioEconomicStudyId = socioEconomicStudyId ?? default;
                _familyResearch.DistrictId = districtId ?? default;
                _familyResearch.EconomicSituationId = economicSituationId ?? default;
                _familyResearch.FamilyNutritionId = familyNutritionId ?? default;
                _familyResearch.BenefitsProvidedId = benefitsProvidedId ?? default;
                _familyResearch.IngresosEgresosMensualesId = ingresosEgresosMensualesId ?? default;

                int? familyResearchId = AddOrUpdateFamilyResearch(_familyResearch);

                scope.Complete();
            }
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

        private int? AddOrUpdateFamilyMembers(FamilyMembers FamilyMembers)
        {
            int familyMembersId;

            if (FamilyMembers == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(FamilyMembers, typeof(FamilyMembers), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateFamilyMembers";
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
                    ParameterName = "FamilyMembersId",
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

                familyMembersId = Convert.ToInt32(cmd.Parameters["FamilyMembersId"].Value);
            }

            return familyMembersId;
        }

        private int? AddOrUpdateSocioEconomicStudy(SocioEconomicStudy socioEconomicStudy)
        {
            int socioEconomicStudyId;

            if (socioEconomicStudy == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(socioEconomicStudy, typeof(SocioEconomicStudy), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateSocioEconomicStudy";
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
                    ParameterName = "SocioEconomicStudyId",
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

                socioEconomicStudyId = Convert.ToInt32(cmd.Parameters["SocioEconomicStudyId"].Value);
            }

            return socioEconomicStudyId;
        }

        private int? AddOrUpdateDistrict(District district)
        {
            int districtId;

            if (district == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(district, typeof(District), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateDistrict";
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
                    ParameterName = "DistrictId",
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

                districtId = Convert.ToInt32(cmd.Parameters["DistrictId"].Value);
            }

            return districtId;
        }

        private int? AddOrUpdateEconomicSituation(EconomicSituation economicSituation)
        {
            int economicSituationId;

            if (economicSituation == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(economicSituation, typeof(EconomicSituation), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateEconomicSituation";
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
                    ParameterName = "EconomicSituationId",
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

                economicSituationId = Convert.ToInt32(cmd.Parameters["EconomicSituationId"].Value);
            }

            return economicSituationId;
        }

        private int? AddOrUpdateFamilyNutrition(FamilyNutrition familyNutrition)
        {
            int familyNutritionId;

            if (familyNutrition == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(familyNutrition, typeof(FamilyNutrition), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateFamilyNutrition";
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
                    ParameterName = "FamilyNutritionId",
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

                familyNutritionId = Convert.ToInt32(cmd.Parameters["FamilyNutritionId"].Value);
            }

            return familyNutritionId;
        }

        private int? AddOrUpdateBenefitsProvided(BenefitsProvided benefitsProvided)
        {
            int benefitsProvidedId;

            if (benefitsProvided == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(benefitsProvided, typeof(BenefitsProvided), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateBenefitsProvided";
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
                    ParameterName = "BenefitsProvidedId",
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

                benefitsProvidedId = Convert.ToInt32(cmd.Parameters["BenefitsProvidedId"].Value);
            }

            return benefitsProvidedId;
        }

        private int? AddOrUpdateIngresosEgresosMensuales(IngresosEgresosMensuales ingresosEgresosMensuales)
        {
            int ingresosEgresosMensualesId;

            if (ingresosEgresosMensuales == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(ingresosEgresosMensuales, typeof(IngresosEgresosMensuales), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateIngresosEgresosMensuales";
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
                    ParameterName = "IngresosEgresosMensualesId",
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

                ingresosEgresosMensualesId = Convert.ToInt32(cmd.Parameters["IngresosEgresosMensualesId"].Value);
            }

            return ingresosEgresosMensualesId;
        }

        private int? AddOrUpdateFamilyResearch(FamilyResearch familyResearch)
        {
            int familyResearchId;

            if (familyResearch == null)
            {
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            var serializedResult = JsonSerializer.Serialize(familyResearch, typeof(FamilyResearch), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateFamilyResearch";
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
                    ParameterName = "FamilyResearchId",
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

                familyResearchId = Convert.ToInt32(cmd.Parameters["FamilyResearchId"].Value);
            }

            return familyResearchId;
        }

        public IEnumerable<MaritalStatus> GetMaritalStatuses()
        {
            IEnumerable<MaritalStatus> maritalStatuses;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {

                string sql = "SELECT * FROM MaritalStatus order by Name;";

                maritalStatuses = connection.Query<MaritalStatus>(sql);
            }

            return maritalStatuses;
        }

        public IEnumerable<Relationship> GetRelationships()
        {
            IEnumerable<Relationship> relationships;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {

                string sql = "SELECT * FROM Relationship order by Name;";

                relationships = connection.Query<Relationship>(sql);
            }

            return relationships;
        }

        public IEnumerable<StatesOfMexico> GetStatesOfMexico()
        {
            IEnumerable<StatesOfMexico> statesOfMexicoList;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM StatesOfMexico ORDER BY nombre;";

                statesOfMexicoList = connection.Query<StatesOfMexico>(sql);
            }

            return statesOfMexicoList;
        }

        public IEnumerable<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            IEnumerable<MunicipalitiesOfMexico> municipalities;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"
                   SELECT m.*,s.*
                    FROM MunicipalitiesOfMexico m
                    JOIN StatesOfMexico s ON s.id = m.estado_id
                    WHERE s.nombre = @nameOfState
                    ORDER BY m.nombre;";

                municipalities = connection.Query<MunicipalitiesOfMexico, StatesOfMexico, MunicipalitiesOfMexico>(sql,
                (m, s) =>
                {
                    m.StatesOfMexico = s;
                    m.EstadoId = s.Id;
                    return m;

                }, new { nameOfState });
            }

            return municipalities;
        }

        public IEnumerable<HomeAcquisition> GetHomeAcquisitions()
        {
            IEnumerable<HomeAcquisition> acquisitions;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM HomeAcquisition ORDER BY Name;";

                acquisitions = connection.Query<HomeAcquisition>(sql);
            }

            return acquisitions;
        }

        public IEnumerable<TypesOfHouses> GetTypesOfHouses()
        {
            IEnumerable<TypesOfHouses> typeOfHouses;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM TypesOfHouses ORDER BY Name;";

                typeOfHouses = connection.Query<TypesOfHouses>(sql);
            }

            return typeOfHouses;
        }

        public IEnumerable<TipoDeMobiliario> GetTipoDeMobiliarios()
        {
            IEnumerable<TipoDeMobiliario> tipoDeMobiliarios;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM TipoDeMobiliario ORDER BY Name;";

                tipoDeMobiliarios = connection.Query<TipoDeMobiliario>(sql);
            }

            return tipoDeMobiliarios;
        }

        public IEnumerable<TypeOfDistrict> GetTypeOfDistricts()
        {
            IEnumerable<TypeOfDistrict> typeOfDistricts;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM TypeOfDistrict ORDER BY Name;";

                typeOfDistricts = connection.Query<TypeOfDistrict>(sql);
            }

            return typeOfDistricts;
        }

        public IEnumerable<Patrimony> GetPatrimonies()
        {
            IEnumerable<Patrimony> patrimonies;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM Patrimony ORDER BY Name;";

                patrimonies = connection.Query<Patrimony>(sql);
            }

            return patrimonies;
        }

        public IEnumerable<Food> GetFoods()
        {
            IEnumerable<Food> foods;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM Food ORDER BY Name;";

                foods = connection.Query<Food>(sql);
            }

            return foods;
        }

         public IEnumerable<Frequency> GetFrequencies()
        {
            IEnumerable<Frequency> frequencies;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM Frequency ORDER BY Name;";

                frequencies = connection.Query<Frequency>(sql);
            }

            return frequencies;
        }

        public FamilyResearch GetFamilyResearchById(int id)
        {
           FamilyResearch familyResearches = null;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT `FamilyResearch`.Id, -- 0
                                `FamilyResearch`.VisitDate, -- 1
                                `FamilyResearch`.VisitTime, -- 2
                                `FamilyResearch`.Family, -- 3
                                `FamilyResearch`.RequestReasons, -- 4
                                `FamilyResearch`.SituationsOfDomesticViolence, -- 5
                                `FamilyResearch`.FamilyExpectations, -- 6
                                `FamilyResearch`.FamilyDiagnostic, -- 7
                                `FamilyResearch`.CaseStudyConclusion, -- 8
                                `FamilyResearch`.Recommendations, -- 9
                                `FamilyResearch`.VisualSupports, -- 10
                                `FamilyResearch`.Sketch, -- 11
                                `FamilyResearch`.LegalGuardianId, -- 12
                                `FamilyResearch`.MinorId, -- 13
                                `FamilyResearch`.PreviousFoundationId, -- 14
                                `FamilyResearch`.FamilyHealthId, -- 15
                                `FamilyResearch`.FamilyMembersId, -- 16
                                `FamilyResearch`.SocioEconomicStudyId, -- 17
                                `FamilyResearch`.DistrictId, -- 18
                                `FamilyResearch`.EconomicSituationId, -- 19
                                `FamilyResearch`.FamilyNutritionId, -- 20
                                `FamilyResearch`.BenefitsProvidedId, -- 21
                                `FamilyResearch`.IngresosEgresosMensualesId, -- 22
                                `LegalGuardian`.Id, -- 23
                                `LegalGuardian`.FullName, -- 24
                                `LegalGuardian`.Age, -- 25
                                `LegalGuardian`.PlaceOfBirth, -- 26
                                `LegalGuardian`.MaritalStatusId, -- 27
                                `LegalGuardian`.Education, -- 28
                                `LegalGuardian`.CurrentOccupation, -- 29
                                `LegalGuardian`.RelationshipId, -- 30
                                `LegalGuardian`.AddressId, -- 31
                                `LegalGuardian`.CellPhoneNumber, -- 32
                                `LegalGuardian`.PhoneNumber, -- 33
                                `LegalGuardian`.Errand, -- 34
                                `LegalGuardian`.SpouseId, -- 35 
                                `LegalGuardian`.DateOfBirth, -- 36
                                `MaritalStatus`.Id, -- 37
                                `MaritalStatus`.Name, -- 38
                                `Relationship`.Id, -- 39
                                `Relationship`.Name, -- 40
                                `Address`.Id, -- 41
                                `Address`.Street, -- 42
                                `Address`.HouseNumber, -- 43
                                `Address`.PoBox, -- 44
                                `Address`.PhoneNumber, -- 45
                                `Address`.City, -- 46
                                `Address`.Zip, -- 47
                                `Address`.State, -- 48
                                `Address`.Neighborhood, -- 49
                                `Address`.Reference, -- 50
                                `Spouse`.Id, -- 51,
                                `Spouse`.FullName, -- 52
                                `Spouse`.Age, -- 53
                                `Spouse`.CurrentOccupation, -- 54
                                `Spouse`.Comments, -- 55
                                `Minor`.Id, -- 56
                                `Minor`.FullName, -- 57
                                `Minor`.DateOfBirth, -- 58
                                `Minor`.PlaceOfBirth, -- 59
                                `Minor`.Age, -- 60
                                `Minor`.Education, -- 61
                                `Minor`.CurrentOccupation, -- 62
                                `Minor`.FormalEducationId, -- 63
                                `FormalEducation`.Id, -- 64
                                `FormalEducation`.CanItRead, -- 65
                                `FormalEducation`.CanItWrite, -- 66
                                `FormalEducation`.IsItStudyingNow, -- 67
                                `FormalEducation`.CurrentGrade, -- 68
                                `FormalEducation`.ReasonsToStopStudying, -- 69
                                `PreviousFoundation`.Id, -- 70
                                `PreviousFoundation`.Familiar, -- 71
                                `PreviousFoundation`.Procuraduria, -- 72
                                `PreviousFoundation`.Dif, -- 73
                                `PreviousFoundation`.Otro, -- 74
                                `PreviousFoundation`.InstitucionAnterior, -- 75
                                `PreviousFoundation`.TiempoDeEstadia, -- 76
                                `PreviousFoundation`.MotivoDeEgreso, -- 77
                                `FamilyHealth`.Id, -- 78
                                `FamilyHealth`.FamilyHealthStatus, -- 79
                                `FamilyHealth`.DerechoHambienteAServiciosDeSalud, -- 80
                                `FamilyHealth`.Tipo, -- 81
                                `FamilyHealth`.EnfermedadesCronicasDegenerativas, -- 82
                                `FamilyHealth`.ConsumoDeTabaco, -- 83
                                `FamilyHealth`.ConsumoDeAlcohol, -- 84
                                `FamilyHealth`.ConsumoDeDrogas, -- 85
                                `FamilyHealth`.Comments, -- 86
                                `SocioEconomicStudy`.Id, -- 87
                                `SocioEconomicStudy`.HomeAcquisitionId, -- 88
                                `SocioEconomicStudy`.NombrePropietario, -- 89
                                `SocioEconomicStudy`.MedioAdquisicion, -- 90
                                `SocioEconomicStudy`.TypesOfHousesId, -- 91
                                `SocioEconomicStudy`.HouseLayoutId, -- 92
                                `District`.Id, -- 93
                                `District`.TypeOfDistrictId, -- 94
                                `District`.AguaPotable, -- 95
                                `District`.Telefono, -- 96
                                `District`.Electricidad, -- 97
                                `District`.Drenaje, -- 98
                                `District`.Hospital, -- 99
                                `District`.Correo, -- 100
                                `District`.Escuela, -- 101
                                `District`.Policia, -- 102
                                `District`.AlumbradoPublico, -- 103
                                `District`.ViasDeAcceso, -- 104
                                `District`.TransportePublico, -- 105
                                `District`.AseoPublico, -- 106
                                `District`.Iglesia, -- 107
                                `District`.Otros, -- 108
                                `District`.Description -- 109

                            FROM `FamilyResearch`
                            LEFT JOIN `LegalGuardian` ON `LegalGuardian`.Id =  `FamilyResearch`.LegalGuardianId
                            LEFT JOIN `MaritalStatus` ON `MaritalStatus`.Id = `LegalGuardian`.MaritalStatusId
                            LEFT JOIN `Relationship` ON `Relationship`.Id = `LegalGuardian`.RelationshipId
                            LEFT JOIN `Address` ON `Address`.Id = `LegalGuardian`.AddressId
                            LEFT JOIN `Spouse` ON `Spouse`.Id = `LegalGuardian`.SpouseId
                            LEFT JOIN `Minor` ON `Minor`.Id = `FamilyResearch`.MinorId
                            LEFT JOIN `FormalEducation` ON `FormalEducation`.Id = `Minor`.FormalEducationId
                            LEFT JOIN `PreviousFoundation` ON `PreviousFoundation`.Id = `FamilyResearch`.PreviousFoundationId
                            LEFT JOIN `FamilyHealth` ON `FamilyHealth`.Id = `FamilyResearch`.FamilyHealthId
                            LEFT JOIN `SocioEconomicStudy` ON `SocioEconomicStudy`.Id = `FamilyResearch`.SocioEconomicStudyId
                            LEFT JOIN `District` ON `District`.Id = `FamilyResearch`.DistrictId
                            WHERE `FamilyResearch`.Id = @id;";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "id",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = id
                });

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                    {
                        return null;
                    }
                    int index;
                    while (reader.Read())
                    {
                        index = 0;

                        familyResearches = new FamilyResearch();
                        familyResearches.Id = reader.GetInt32(index);
                        familyResearches.VisitDate = reader.GetDateTime(index);
                        familyResearches.VisitTime = reader.GetDateTime(index);
                        familyResearches.Family = reader.GetString(index);
                        familyResearches.RequestReasons = reader.GetString(index);
                        familyResearches.SituationsOfDomesticViolence = reader.GetString(index);
                        familyResearches.FamilyExpectations = reader.GetString(index);
                        familyResearches.FamilyDiagnostic = reader.GetString(index);
                        familyResearches.CaseStudyConclusion = reader.GetString(index);
                        familyResearches.Recommendations = reader.GetString(index);
                        familyResearches.VisualSupports = reader.GetString(index);
                        familyResearches.Sketch = reader.GetString(index);
                        familyResearches.LegalGuardianId = reader.GetInt32(index);
                        familyResearches.MinorId = reader.GetInt32(index);
                        familyResearches.PreviousFoundationId = reader.GetInt32(index);
                        familyResearches.FamilyHealthId = reader.GetInt32(index);
                        familyResearches.FamilyMembersId = reader.GetInt32(index);
                        familyResearches.SocioEconomicStudyId = reader.GetInt32(index);
                        familyResearches.DistrictId = reader.GetInt32(index);
                        familyResearches.EconomicSituationId = reader.GetInt32(index);
                        familyResearches.FamilyNutritionId = reader.GetInt32(index);
                        familyResearches.BenefitsProvidedId = reader.GetInt32(index);
                        familyResearches.IngresosEgresosMensualesId = reader.GetInt32(index);

                        familyResearches.LegalGuardian = new LegalGuardian();
                        familyResearches.LegalGuardian.Id = reader.GetInt32(index);
                        familyResearches.LegalGuardian.FullName = reader.GetString(index);
                        familyResearches.LegalGuardian.Age = reader.GetInt32(index);
                        familyResearches.LegalGuardian.PlaceOfBirth = reader.GetString(index);
                        familyResearches.LegalGuardian.MaritalStatusId = reader.GetInt32(index);
                        familyResearches.LegalGuardian.Education = reader.GetString(index);
                        familyResearches.LegalGuardian.CurrentOccupation = reader.GetString(index);
                        familyResearches.LegalGuardian.RelationshipId = reader.GetInt32(index);
                        familyResearches.LegalGuardian.AddressId = reader.GetInt32(index);
                        familyResearches.LegalGuardian.CellPhoneNumber = reader.GetString(index);
                        familyResearches.LegalGuardian.PhoneNumber = reader.GetString(index);
                        familyResearches.LegalGuardian.Errand = reader.GetString(index);
                        familyResearches.LegalGuardian.SpouseId = reader.GetInt32(index);
                        familyResearches.LegalGuardian.DateOfBirth = reader.GetDateTime(index);

                        familyResearches.LegalGuardian.MaritalStatus = new MaritalStatus();
                        familyResearches.LegalGuardian.MaritalStatus.Id = reader.GetInt32(index);
                        familyResearches.LegalGuardian.MaritalStatus.Name = reader.GetString(index);

                        familyResearches.LegalGuardian.Relationship = new Relationship();
                        familyResearches.LegalGuardian.Relationship.Id = reader.GetInt32(index);
                        familyResearches.LegalGuardian.Relationship.Name = reader.GetString(index);

                        familyResearches.LegalGuardian.Address = new Address();
                        familyResearches.LegalGuardian.Address.Id = reader.GetInt32(index);


                        
                        


                    }
                }
                
            }
            
            return familyResearches;
        }
    }
}