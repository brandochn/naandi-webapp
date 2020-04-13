using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Transactions;
using Dapper;
using MySql.Data.MySqlClient;
using Naandi.Shared;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using WebApi.Data;
using WebApi.ExtensionMethods;

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

                _familyResearch.LegalGuardianId = legalGuardianId;
                _familyResearch.MinorId = minorId;
                _familyResearch.PreviousFoundationId = previousFoundationId;
                _familyResearch.FamilyHealthId = familyHealthId;
                _familyResearch.FamilyMembersId = familyMembersId;
                _familyResearch.SocioEconomicStudyId = socioEconomicStudyId;
                _familyResearch.DistrictId = districtId;
                _familyResearch.EconomicSituationId = economicSituationId;
                _familyResearch.FamilyNutritionId = familyNutritionId;
                _familyResearch.BenefitsProvidedId = benefitsProvidedId;
                _familyResearch.IngresosEgresosMensualesId = ingresosEgresosMensualesId;

                int? familyResearchId = AddOrUpdateFamilyResearch(_familyResearch);

                scope.Complete();
            }
        }

        private int? AddOrUpdateSpouse(Spouse spouse)
        {
            int spouseId;

            if (spouse == null ||
                spouse.FullName == null)
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
            FamilyResearch familyResearch = new FamilyResearch();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT `FamilyResearch`.Id, -- 1
                                `FamilyResearch`.VisitDate, -- 2
                                `FamilyResearch`.VisitTime, -- 3
                                `FamilyResearch`.Family, -- 4
                                `FamilyResearch`.RequestReasons, -- 5
                                `FamilyResearch`.SituationsOfDomesticViolence, -- 6
                                `FamilyResearch`.FamilyExpectations, -- 7
                                `FamilyResearch`.FamilyDiagnostic, -- 8
                                `FamilyResearch`.CaseStudyConclusion, -- 9
                                `FamilyResearch`.Recommendations, -- 10
                                `FamilyResearch`.VisualSupports, -- 11
                                `FamilyResearch`.Sketch, -- 12
                                `FamilyResearch`.LegalGuardianId, -- 13
                                `FamilyResearch`.MinorId, -- 14
                                `FamilyResearch`.PreviousFoundationId, -- 15
                                `FamilyResearch`.FamilyHealthId, -- 16
                                `FamilyResearch`.FamilyMembersId, -- 17
                                `FamilyResearch`.SocioEconomicStudyId, -- 18
                                `FamilyResearch`.DistrictId, -- 19
                                `FamilyResearch`.EconomicSituationId, -- 20
                                `FamilyResearch`.FamilyNutritionId, -- 21
                                `FamilyResearch`.BenefitsProvidedId, -- 22
                                `FamilyResearch`.IngresosEgresosMensualesId, -- 23
                                `LegalGuardian`.Id, -- 24
                                `LegalGuardian`.FullName, -- 25
                                `LegalGuardian`.Age, -- 26
                                `LegalGuardian`.PlaceOfBirth, -- 27
                                `LegalGuardian`.MaritalStatusId, -- 28
                                `LegalGuardian`.Education, -- 29
                                `LegalGuardian`.CurrentOccupation, -- 30
                                `LegalGuardian`.RelationshipId, -- 31
                                `LegalGuardian`.AddressId, -- 32
                                `LegalGuardian`.CellPhoneNumber, -- 33
                                `LegalGuardian`.PhoneNumber, -- 34
                                `LegalGuardian`.Errand, -- 35
                                `LegalGuardian`.SpouseId, -- 36
                                `LegalGuardian`.DateOfBirth, -- 37
                                `MaritalStatus`.Id, -- 38
                                `MaritalStatus`.Name, -- 39
                                `Relationship`.Id, -- 40
                                `Relationship`.Name, -- 41
                                `Address`.Id, -- 42
                                `Address`.Street, -- 43
                                `Address`.HouseNumber, -- 44
                                `Address`.PoBox, -- 45
                                `Address`.PhoneNumber, -- 46
                                `Address`.City, -- 47
                                `Address`.Zip, -- 48
                                `Address`.State, -- 49
                                `Address`.Neighborhood, -- 50
                                `Address`.Reference, -- 51
                                `Spouse`.Id, -- 52,
                                `Spouse`.FullName, -- 53
                                `Spouse`.Age, -- 54
                                `Spouse`.CurrentOccupation, -- 55
                                `Spouse`.Comments, -- 56
                                `Minor`.Id, -- 57
                                `Minor`.FullName, -- 58
                                `Minor`.DateOfBirth, -- 59
                                `Minor`.PlaceOfBirth, -- 60
                                `Minor`.Age, -- 61
                                `Minor`.Education, -- 62
                                `Minor`.CurrentOccupation, -- 63
                                `Minor`.FormalEducationId, -- 64
                                `FormalEducation`.Id, -- 65
                                `FormalEducation`.CanItRead, -- 66
                                `FormalEducation`.CanItWrite, -- 67
                                `FormalEducation`.IsItStudyingNow, -- 68
                                `FormalEducation`.CurrentGrade, -- 69
                                `FormalEducation`.ReasonsToStopStudying, -- 70
                                `PreviousFoundation`.Id, -- 71
                                `PreviousFoundation`.Familiar, -- 72
                                `PreviousFoundation`.Procuraduria, -- 73
                                `PreviousFoundation`.Dif, -- 74
                                `PreviousFoundation`.Otro, -- 75
                                `PreviousFoundation`.InstitucionAnterior, -- 76
                                `PreviousFoundation`.TiempoDeEstadia, -- 77
                                `PreviousFoundation`.MotivoDeEgreso, -- 78
                                `FamilyHealth`.Id, -- 79
                                `FamilyHealth`.FamilyHealthStatus, -- 80
                                `FamilyHealth`.DerechoHambienteAServiciosDeSalud, -- 81
                                `FamilyHealth`.Tipo, -- 82
                                `FamilyHealth`.EnfermedadesCronicasDegenerativas, -- 83
                                `FamilyHealth`.ConsumoDeTabaco, -- 84
                                `FamilyHealth`.ConsumoDeAlcohol, -- 85
                                `FamilyHealth`.ConsumoDeDrogas, -- 86
                                `FamilyHealth`.Comments, -- 87
                                `SocioEconomicStudy`.Id, -- 88
                                `SocioEconomicStudy`.HomeAcquisitionId, -- 89
                                `SocioEconomicStudy`.NombrePropietario, -- 90
                                `SocioEconomicStudy`.MedioAdquisicion, -- 91
                                `SocioEconomicStudy`.TypesOfHousesId, -- 92
                                `SocioEconomicStudy`.HouseLayoutId, -- 93
                                `HomeAcquisition`.Id, -- 94
                                `HomeAcquisition`.Name, -- 95
                                `TypesOfHouses`.Id, -- 96
                                `TypesOfHouses`.Name, -- 97
                                `HouseLayout`.Id,  -- 98
                                `HouseLayout`.Bedroom, -- 99
                                `HouseLayout`.Dinningroom, -- 100
                                `HouseLayout`.Kitchen, -- 101
                                `HouseLayout`.Livingroom, -- 102
                                `HouseLayout`.Bathroom, -- 103
                                `HouseLayout`.Patio, -- 104
                                `HouseLayout`.Garage, -- 105
                                `HouseLayout`.Backyard, -- 106
                                `HouseLayout`.Other, -- 107
                                `HouseLayout`.Ground, -- 108
                                `HouseLayout`.Walls, -- 109
                                `HouseLayout`.Roof, -- 110
                                `HouseLayout`.Description, -- 111
                                `HouseLayout`.TipoDeMobiliarioId, -- 112
                                `HouseLayout`.CharacteristicsOfFurniture, -- 113
                                `TipoDeMobiliario`.Id, -- 114
                                `TipoDeMobiliario`.Name, -- 115
                                `District`.Id, -- 116
                                `District`.TypeOfDistrictId, -- 117
                                `District`.AguaPotable, -- 118
                                `District`.Telefono, -- 119
                                `District`.Electricidad, -- 120
                                `District`.Drenaje, -- 121
                                `District`.Hospital, -- 122
                                `District`.Correo, -- 123
                                `District`.Escuela, -- 124
                                `District`.Policia, -- 125
                                `District`.AlumbradoPublico, -- 126
                                `District`.ViasDeAcceso, -- 127
                                `District`.TransportePublico, -- 128
                                `District`.AseoPublico, -- 129
                                `District`.Iglesia, -- 130
                                `District`.Otros, -- 131
                                `District`.Description -- 132

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
                            LEFT JOIN `HomeAcquisition` on `HomeAcquisition`.Id = `SocioEconomicStudy`.HouseLayoutId
                            LEFT JOIN `TypesOfHouses` on `TypesOfHouses`.Id = `SocioEconomicStudy`.TypesOfHousesId
                            LEFT JOIN `HouseLayout` on `HouseLayout`.Id = `SocioEconomicStudy`.HouseLayoutId
                            LEFT JOIN `TipoDeMobiliario` on `TipoDeMobiliario`.Id = `HouseLayout`.TipoDeMobiliarioId
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

                        familyResearch.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.VisitDate = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.VisitTime = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.Family = reader.GetValueOrNull<string>(index++);
                        familyResearch.RequestReasons = reader.GetValueOrNull<string>(index++);
                        familyResearch.SituationsOfDomesticViolence = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyExpectations = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyDiagnostic = reader.GetValueOrNull<string>(index++);
                        familyResearch.CaseStudyConclusion = reader.GetValueOrNull<string>(index++);
                        familyResearch.Recommendations = reader.GetValueOrNull<string>(index++);
                        familyResearch.VisualSupports = reader.GetValueOrNull<string>(index++);
                        familyResearch.Sketch = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardianId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.MinorId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.PreviousFoundationId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyHealthId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyMembersId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.SocioEconomicStudyId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.DistrictId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.EconomicSituationId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyNutritionId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.BenefitsProvidedId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.IngresosEgresosMensualesId = reader.GetValueOrNullable<int>(index++);

                        familyResearch.LegalGuardian = new LegalGuardian();
                        familyResearch.LegalGuardian.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.PlaceOfBirth = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.MaritalStatusId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Education = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.RelationshipId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.AddressId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.CellPhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.PhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Errand = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.SpouseId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.DateOfBirth = reader.GetValueOrDefault<DateTime>(index++);

                        familyResearch.LegalGuardian.MaritalStatus = new MaritalStatus();
                        familyResearch.LegalGuardian.MaritalStatus.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.MaritalStatus.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.LegalGuardian.Relationship = new Relationship();
                        familyResearch.LegalGuardian.Relationship.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Relationship.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.LegalGuardian.Address = new Address();
                        familyResearch.LegalGuardian.Address.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Address.Street = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.HouseNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.PoBox = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.PhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.City = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Zip = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.State = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Neighborhood = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Reference = reader.GetValueOrNull<string>(index++);


                        familyResearch.LegalGuardian.Spouse = new Spouse();
                        familyResearch.LegalGuardian.Spouse.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Spouse.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Spouse.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Spouse.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Spouse.Comments = reader.GetValueOrNull<string>(index++);

                        familyResearch.Minor = new Minor();
                        familyResearch.Minor.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.DateOfBirth = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.Minor.PlaceOfBirth = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.Education = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.FormalEducationId = reader.GetValueOrDefault<int>(index++);

                        familyResearch.Minor.FormalEducation = new FormalEducation();
                        familyResearch.Minor.FormalEducation.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.FormalEducation.CanItRead = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.CanItWrite = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.IsItStudyingNow = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.CurrentGrade = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.FormalEducation.ReasonsToStopStudying = reader.GetValueOrNull<string>(index++);

                        familyResearch.PreviousFoundation = new PreviousFoundation();
                        familyResearch.PreviousFoundation.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.PreviousFoundation.Familiar = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Procuraduria = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Dif = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Otro = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.InstitucionAnterior = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.TiempoDeEstadia = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.MotivoDeEgreso = reader.GetValueOrNull<string>(index++);

                        familyResearch.FamilyHealth = new FamilyHealth();
                        familyResearch.FamilyHealth.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.FamilyHealth.FamilyHealthStatus = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.DerechoHambienteAServiciosDeSalud = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.Tipo = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.EnfermedadesCronicasDegenerativas = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeTabaco = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeAlcohol = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeDrogas = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.Comments = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy = new SocioEconomicStudy();
                        familyResearch.SocioEconomicStudy.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HomeAcquisitionId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.NombrePropietario = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.MedioAdquisicion = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.TypesOfHousesId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayoutId = reader.GetValueOrDefault<int>(index++);

                        familyResearch.SocioEconomicStudy.HomeAcquisition = new HomeAcquisition();
                        familyResearch.SocioEconomicStudy.HomeAcquisition.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HomeAcquisition.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.TypesOfHouses = new TypesOfHouses();
                        familyResearch.SocioEconomicStudy.TypesOfHouses.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.TypesOfHouses.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.HouseLayout = new HouseLayout();
                        familyResearch.SocioEconomicStudy.HouseLayout.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Bedroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Dinningroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Kitchen = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Livingroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Bathroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Patio = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Garage = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Backyard = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Other = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Ground = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Walls = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Roof = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Description = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliarioId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.CharacteristicsOfFurniture = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario = new TipoDeMobiliario();
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.District = new District();
                        familyResearch.District.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.District.TypeOfDistrictId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.District.AguaPotable = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Telefono = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Electricidad = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Drenaje = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Hospital = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Correo = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Escuela = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Policia = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.AlumbradoPublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.ViasDeAcceso = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.TransportePublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.AseoPublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Iglesia = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Otros = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Description = reader.GetValueOrNull<string>(index++);

                    }
                }
            }

            familyResearch.FamilyMembers = GetFamilyFamilyMembersById(familyResearch.FamilyMembersId);
            familyResearch.EconomicSituation = GetEconomicSituationById(familyResearch.EconomicSituationId);
            familyResearch.FamilyNutrition = GetFamilyNutritionById(familyResearch.FamilyNutritionId);
            familyResearch.BenefitsProvided = GetBenefitsProvidedById(familyResearch.BenefitsProvidedId);
            familyResearch.IngresosEgresosMensuales = GetIngresosEgresosMensualesById(familyResearch.IngresosEgresosMensualesId);


            return familyResearch;
        }

        public IEnumerable<FamilyResearch> GetFamilyResearches()
        {
            IList<FamilyResearch> familyResearches = new List<FamilyResearch>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT `FamilyResearch`.Id, -- 1
                                `FamilyResearch`.VisitDate, -- 2
                                `FamilyResearch`.VisitTime, -- 3
                                `FamilyResearch`.Family, -- 4
                                `FamilyResearch`.RequestReasons, -- 5
                                `FamilyResearch`.SituationsOfDomesticViolence, -- 6
                                `FamilyResearch`.FamilyExpectations, -- 7
                                `FamilyResearch`.FamilyDiagnostic, -- 8
                                `FamilyResearch`.CaseStudyConclusion, -- 9
                                `FamilyResearch`.Recommendations, -- 10
                                `FamilyResearch`.VisualSupports, -- 11
                                `FamilyResearch`.Sketch, -- 12
                                `FamilyResearch`.LegalGuardianId, -- 13
                                `FamilyResearch`.MinorId, -- 14
                                `FamilyResearch`.PreviousFoundationId, -- 15
                                `FamilyResearch`.FamilyHealthId, -- 16
                                `FamilyResearch`.FamilyMembersId, -- 17
                                `FamilyResearch`.SocioEconomicStudyId, -- 18
                                `FamilyResearch`.DistrictId, -- 19
                                `FamilyResearch`.EconomicSituationId, -- 20
                                `FamilyResearch`.FamilyNutritionId, -- 21
                                `FamilyResearch`.BenefitsProvidedId, -- 22
                                `FamilyResearch`.IngresosEgresosMensualesId, -- 23
                                `LegalGuardian`.Id, -- 24
                                `LegalGuardian`.FullName, -- 25
                                `LegalGuardian`.Age, -- 26
                                `LegalGuardian`.PlaceOfBirth, -- 27
                                `LegalGuardian`.MaritalStatusId, -- 28
                                `LegalGuardian`.Education, -- 29
                                `LegalGuardian`.CurrentOccupation, -- 30
                                `LegalGuardian`.RelationshipId, -- 31
                                `LegalGuardian`.AddressId, -- 32
                                `LegalGuardian`.CellPhoneNumber, -- 33
                                `LegalGuardian`.PhoneNumber, -- 34
                                `LegalGuardian`.Errand, -- 35
                                `LegalGuardian`.SpouseId, -- 36
                                `LegalGuardian`.DateOfBirth, -- 37
                                `MaritalStatus`.Id, -- 38
                                `MaritalStatus`.Name, -- 39
                                `Relationship`.Id, -- 40
                                `Relationship`.Name, -- 41
                                `Address`.Id, -- 42
                                `Address`.Street, -- 43
                                `Address`.HouseNumber, -- 44
                                `Address`.PoBox, -- 45
                                `Address`.PhoneNumber, -- 46
                                `Address`.City, -- 47
                                `Address`.Zip, -- 48
                                `Address`.State, -- 49
                                `Address`.Neighborhood, -- 50
                                `Address`.Reference, -- 51
                                `Spouse`.Id, -- 52,
                                `Spouse`.FullName, -- 53
                                `Spouse`.Age, -- 54
                                `Spouse`.CurrentOccupation, -- 55
                                `Spouse`.Comments, -- 56
                                `Minor`.Id, -- 57
                                `Minor`.FullName, -- 58
                                `Minor`.DateOfBirth, -- 59
                                `Minor`.PlaceOfBirth, -- 60
                                `Minor`.Age, -- 61
                                `Minor`.Education, -- 62
                                `Minor`.CurrentOccupation, -- 63
                                `Minor`.FormalEducationId, -- 64
                                `FormalEducation`.Id, -- 65
                                `FormalEducation`.CanItRead, -- 66
                                `FormalEducation`.CanItWrite, -- 67
                                `FormalEducation`.IsItStudyingNow, -- 68
                                `FormalEducation`.CurrentGrade, -- 69
                                `FormalEducation`.ReasonsToStopStudying, -- 70
                                `PreviousFoundation`.Id, -- 71
                                `PreviousFoundation`.Familiar, -- 72
                                `PreviousFoundation`.Procuraduria, -- 73
                                `PreviousFoundation`.Dif, -- 74
                                `PreviousFoundation`.Otro, -- 75
                                `PreviousFoundation`.InstitucionAnterior, -- 76
                                `PreviousFoundation`.TiempoDeEstadia, -- 77
                                `PreviousFoundation`.MotivoDeEgreso, -- 78
                                `FamilyHealth`.Id, -- 79
                                `FamilyHealth`.FamilyHealthStatus, -- 80
                                `FamilyHealth`.DerechoHambienteAServiciosDeSalud, -- 81
                                `FamilyHealth`.Tipo, -- 82
                                `FamilyHealth`.EnfermedadesCronicasDegenerativas, -- 83
                                `FamilyHealth`.ConsumoDeTabaco, -- 84
                                `FamilyHealth`.ConsumoDeAlcohol, -- 85
                                `FamilyHealth`.ConsumoDeDrogas, -- 86
                                `FamilyHealth`.Comments, -- 87
                                `SocioEconomicStudy`.Id, -- 88
                                `SocioEconomicStudy`.HomeAcquisitionId, -- 89
                                `SocioEconomicStudy`.NombrePropietario, -- 90
                                `SocioEconomicStudy`.MedioAdquisicion, -- 91
                                `SocioEconomicStudy`.TypesOfHousesId, -- 92
                                `SocioEconomicStudy`.HouseLayoutId, -- 93
                                `HomeAcquisition`.Id, -- 94
                                `HomeAcquisition`.Name, -- 95
                                `TypesOfHouses`.Id, -- 96
                                `TypesOfHouses`.Name, -- 97
                                `HouseLayout`.Id,  -- 98
                                `HouseLayout`.Bedroom, -- 99
                                `HouseLayout`.Dinningroom, -- 100
                                `HouseLayout`.Kitchen, -- 101
                                `HouseLayout`.Livingroom, -- 102
                                `HouseLayout`.Bathroom, -- 103
                                `HouseLayout`.Patio, -- 104
                                `HouseLayout`.Garage, -- 105
                                `HouseLayout`.Backyard, -- 106
                                `HouseLayout`.Other, -- 107
                                `HouseLayout`.Ground, -- 108
                                `HouseLayout`.Walls, -- 109
                                `HouseLayout`.Roof, -- 110
                                `HouseLayout`.Description, -- 111
                                `HouseLayout`.TipoDeMobiliarioId, -- 112
                                `HouseLayout`.CharacteristicsOfFurniture, -- 113
                                `TipoDeMobiliario`.Id, -- 114
                                `TipoDeMobiliario`.Name, -- 115
                                `District`.Id, -- 116
                                `District`.TypeOfDistrictId, -- 117
                                `District`.AguaPotable, -- 118
                                `District`.Telefono, -- 119
                                `District`.Electricidad, -- 120
                                `District`.Drenaje, -- 121
                                `District`.Hospital, -- 122
                                `District`.Correo, -- 123
                                `District`.Escuela, -- 124
                                `District`.Policia, -- 125
                                `District`.AlumbradoPublico, -- 126
                                `District`.ViasDeAcceso, -- 127
                                `District`.TransportePublico, -- 128
                                `District`.AseoPublico, -- 129
                                `District`.Iglesia, -- 130
                                `District`.Otros, -- 131
                                `District`.Description -- 132

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
                            LEFT JOIN `HomeAcquisition` on `HomeAcquisition`.Id = `SocioEconomicStudy`.HouseLayoutId
                            LEFT JOIN `TypesOfHouses` on `TypesOfHouses`.Id = `SocioEconomicStudy`.TypesOfHousesId
                            LEFT JOIN `HouseLayout` on `HouseLayout`.Id = `SocioEconomicStudy`.HouseLayoutId
                            LEFT JOIN `TipoDeMobiliario` on `TipoDeMobiliario`.Id = `HouseLayout`.TipoDeMobiliarioId
                            LEFT JOIN `District` ON `District`.Id = `FamilyResearch`.DistrictId";

                cmd.CommandType = System.Data.CommandType.Text;

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

                        FamilyResearch familyResearch = new FamilyResearch();
                        familyResearch.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.VisitDate = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.VisitTime = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.Family = reader.GetValueOrNull<string>(index++);
                        familyResearch.RequestReasons = reader.GetValueOrNull<string>(index++);
                        familyResearch.SituationsOfDomesticViolence = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyExpectations = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyDiagnostic = reader.GetValueOrNull<string>(index++);
                        familyResearch.CaseStudyConclusion = reader.GetValueOrNull<string>(index++);
                        familyResearch.Recommendations = reader.GetValueOrNull<string>(index++);
                        familyResearch.VisualSupports = reader.GetValueOrNull<string>(index++);
                        familyResearch.Sketch = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardianId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.MinorId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.PreviousFoundationId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyHealthId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyMembersId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.SocioEconomicStudyId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.DistrictId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.EconomicSituationId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.FamilyNutritionId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.BenefitsProvidedId = reader.GetValueOrNullable<int>(index++);
                        familyResearch.IngresosEgresosMensualesId = reader.GetValueOrNullable<int>(index++);

                        familyResearch.LegalGuardian = new LegalGuardian();
                        familyResearch.LegalGuardian.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.PlaceOfBirth = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.MaritalStatusId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Education = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.RelationshipId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.AddressId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.CellPhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.PhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Errand = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.SpouseId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.DateOfBirth = reader.GetValueOrDefault<DateTime>(index++);

                        familyResearch.LegalGuardian.MaritalStatus = new MaritalStatus();
                        familyResearch.LegalGuardian.MaritalStatus.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.MaritalStatus.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.LegalGuardian.Relationship = new Relationship();
                        familyResearch.LegalGuardian.Relationship.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Relationship.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.LegalGuardian.Address = new Address();
                        familyResearch.LegalGuardian.Address.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Address.Street = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.HouseNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.PoBox = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.PhoneNumber = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.City = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Zip = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.State = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Neighborhood = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Address.Reference = reader.GetValueOrNull<string>(index++);


                        familyResearch.LegalGuardian.Spouse = new Spouse();
                        familyResearch.LegalGuardian.Spouse.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Spouse.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Spouse.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.LegalGuardian.Spouse.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.LegalGuardian.Spouse.Comments = reader.GetValueOrNull<string>(index++);

                        familyResearch.Minor = new Minor();
                        familyResearch.Minor.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.FullName = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.DateOfBirth = reader.GetValueOrDefault<DateTime>(index++);
                        familyResearch.Minor.PlaceOfBirth = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.Age = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.Education = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.CurrentOccupation = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.FormalEducationId = reader.GetValueOrDefault<int>(index++);

                        familyResearch.Minor.FormalEducation = new FormalEducation();
                        familyResearch.Minor.FormalEducation.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.Minor.FormalEducation.CanItRead = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.CanItWrite = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.IsItStudyingNow = reader.GetValueOrDefault<bool>(index++);
                        familyResearch.Minor.FormalEducation.CurrentGrade = reader.GetValueOrNull<string>(index++);
                        familyResearch.Minor.FormalEducation.ReasonsToStopStudying = reader.GetValueOrNull<string>(index++);

                        familyResearch.PreviousFoundation = new PreviousFoundation();
                        familyResearch.PreviousFoundation.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.PreviousFoundation.Familiar = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Procuraduria = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Dif = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.Otro = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.InstitucionAnterior = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.TiempoDeEstadia = reader.GetValueOrNull<string>(index++);
                        familyResearch.PreviousFoundation.MotivoDeEgreso = reader.GetValueOrNull<string>(index++);

                        familyResearch.FamilyHealth = new FamilyHealth();
                        familyResearch.FamilyHealth.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.FamilyHealth.FamilyHealthStatus = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.DerechoHambienteAServiciosDeSalud = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.Tipo = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.EnfermedadesCronicasDegenerativas = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeTabaco = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeAlcohol = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.ConsumoDeDrogas = reader.GetValueOrNull<string>(index++);
                        familyResearch.FamilyHealth.Comments = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy = new SocioEconomicStudy();
                        familyResearch.SocioEconomicStudy.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HomeAcquisitionId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.NombrePropietario = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.MedioAdquisicion = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.TypesOfHousesId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayoutId = reader.GetValueOrDefault<int>(index++);

                        familyResearch.SocioEconomicStudy.HomeAcquisition = new HomeAcquisition();
                        familyResearch.SocioEconomicStudy.HomeAcquisition.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HomeAcquisition.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.TypesOfHouses = new TypesOfHouses();
                        familyResearch.SocioEconomicStudy.TypesOfHouses.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.TypesOfHouses.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.HouseLayout = new HouseLayout();
                        familyResearch.SocioEconomicStudy.HouseLayout.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Bedroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Dinningroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Kitchen = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Livingroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Bathroom = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Patio = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Garage = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Backyard = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Other = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Ground = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Walls = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Roof = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.Description = reader.GetValueOrNull<string>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliarioId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.CharacteristicsOfFurniture = reader.GetValueOrNull<string>(index++);

                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario = new TipoDeMobiliario();
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.SocioEconomicStudy.HouseLayout.TipoDeMobiliario.Name = reader.GetValueOrNull<string>(index++);

                        familyResearch.District = new District();
                        familyResearch.District.Id = reader.GetValueOrDefault<int>(index++);
                        familyResearch.District.TypeOfDistrictId = reader.GetValueOrDefault<int>(index++);
                        familyResearch.District.AguaPotable = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Telefono = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Electricidad = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Drenaje = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Hospital = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Correo = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Escuela = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Policia = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.AlumbradoPublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.ViasDeAcceso = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.TransportePublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.AseoPublico = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Iglesia = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Otros = reader.GetValueOrNull<string>(index++);
                        familyResearch.District.Description = reader.GetValueOrNull<string>(index++);

                        familyResearches.Add(familyResearch);

                    }
                }
            }

            if (familyResearches != null)
            {
                foreach (var fm in familyResearches)
                {
                    fm.FamilyMembers = GetFamilyFamilyMembersById(fm.FamilyMembersId);
                    fm.EconomicSituation = GetEconomicSituationById(fm.EconomicSituationId);
                    fm.FamilyNutrition = GetFamilyNutritionById(fm.FamilyNutritionId);
                    fm.BenefitsProvided = GetBenefitsProvidedById(fm.BenefitsProvidedId);
                    fm.IngresosEgresosMensuales = GetIngresosEgresosMensualesById(fm.IngresosEgresosMensualesId);
                }
            }

            return familyResearches;
        }

        public void Update(FamilyResearch familyResearch)
        {
            Add(familyResearch);
        }

        public IEnumerable<FamilyResearch> GetFamilyResearchByMinorName(string minorName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movimiento> GetMovimientosByTipoMovimiento(string tipoMovimiento)
        {
            IEnumerable<Movimiento> items;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"SELECT * FROM Movimiento m
                                JOIN TipoMovimiento tm ON tm.Id = m.TipoMovimientoId 
                                WHERE tm.Name = @tipoMovimiento ORDER BY m.Name;";

                items = connection.Query<Movimiento, TipoMovimiento, Movimiento>(sql,
                (m, tm) =>
                {
                    m.TipoMovimiento = tm;
                    m.TipoMovimientoId = tm.Id;
                    return m;

                }, new { tipoMovimiento });
            }

            return items;
        }

        public FamilyMembers GetFamilyFamilyMembersById(int? familyMembersId)
        {
            if (familyMembersId == null)
            {
                return null;
            }

            FamilyMembers familyMembers = new FamilyMembers();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select fm.*
                                from familymembers fm                               
                                where fm.Id = @familyMembersId;";

                familyMembers = connection.QueryFirstOrDefault<FamilyMembers>(sql, new { familyMembersId });

                if (familyMembers != null)
                {
                    sql = @"select fmd.*
                            from familymembersdetails fmd                               
                            where fmd.FamilyMembersId = @familyMembersId;";

                    familyMembers.FamilyMembersDetails = connection.Query<FamilyMembersDetails>(sql, new { familyMembersId }).ToArray();
                }
            }

            return familyMembers;
        }

        public EconomicSituation GetEconomicSituationById(int? economicSituationId)
        {
            if (economicSituationId == null)
            {
                return null;
            }

            EconomicSituation economicSituation = new EconomicSituation();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from EconomicSituation where Id = @economicSituationId;";

                economicSituation = connection.QueryFirstOrDefault<EconomicSituation>(sql, new { economicSituationId });

                if (economicSituation != null)
                {
                    sql = @"select * from EconomicSituationPatrimonyRelation where economicsituationId = @economicSituationId;";

                    economicSituation.EconomicSituationPatrimonyRelation = connection.Query<EconomicSituationPatrimonyRelation>(sql, new { economicSituationId }).ToArray();
                }
            }

            return economicSituation;
        }

        public FamilyNutrition GetFamilyNutritionById(int? familyNutritionId)
        {
            if (familyNutritionId == null)
            {
                return null;
            }

            FamilyNutrition familyNutrition = new FamilyNutrition();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from FamilyNutrition where Id = @familyNutritionId;";

                familyNutrition = connection.QueryFirstOrDefault<FamilyNutrition>(sql, new { familyNutritionId });

                if (familyNutrition != null)
                {
                    sql = @"select * from FamilyNutritionFoodRelation where FamilyNutritionId = @familyNutritionId;";

                    familyNutrition.FamilyNutritionFoodRelation = connection.Query<FamilyNutritionFoodRelation>(sql, new { familyNutritionId }).ToArray();
                }
            }

            return familyNutrition;
        }

        public BenefitsProvided GetBenefitsProvidedById(int? benefitsProvidedId)
        {
            if (benefitsProvidedId == null)
            {
                return null;
            }

            BenefitsProvided benefitsProvided = new BenefitsProvided();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from BenefitsProvided where Id = @benefitsProvidedId;";

                benefitsProvided = connection.QueryFirstOrDefault<BenefitsProvided>(sql, new { benefitsProvidedId });

                if (benefitsProvided != null)
                {
                    sql = @"select * from BenefitsProvidedDetails where BenefitsProvidedId = @benefitsProvidedId;";

                    benefitsProvided.BenefitsProvidedDetails = connection.Query<BenefitsProvidedDetails>(sql, new { benefitsProvidedId }).ToArray();
                }
            }

            return benefitsProvided;
        }

        public IngresosEgresosMensuales GetIngresosEgresosMensualesById(int? Id)
        {
            if (Id == null)
            {
                return null;
            }

            IngresosEgresosMensuales ingresosEgresosMensuales = new IngresosEgresosMensuales();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"select * from IngresosEgresosMensuales where Id = @Id;";

                ingresosEgresosMensuales = connection.QueryFirstOrDefault<IngresosEgresosMensuales>(sql, new { Id });

                if (ingresosEgresosMensuales != null)
                {
                    sql = @"select i.*
	                            ,m.*
                                ,tm.*
                            from ingresosegresosmensualesmovimientorelation i
                            join movimiento m on m.Id = i.MovimientoId
                            join tipomovimiento tm on tm.Id = m.TipoMovimientoId 
                            where IngresosEgresosMensualesId = @Id;";

                    ingresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation = connection.Query<IngresosEgresosMensualesMovimientoRelation, Movimiento, TipoMovimiento, IngresosEgresosMensualesMovimientoRelation>(sql,
                        (i, m, tm) =>
                         {
                             if (i == null)
                                 return null;

                             i.Movimiento = m;
                             i.MovimientoId = m.Id;
                             i.Movimiento.TipoMovimiento = tm;
                             i.Movimiento.TipoMovimientoId = tm.Id;
                             return i;
                         }, new { Id }).ToArray();
                }
            }

            return ingresosEgresosMensuales;
        }
    }
}