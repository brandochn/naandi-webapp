using System;
using System.Collections.Generic;
using System.Text.Json;
using MySql.Data.MySqlClient;
using Naandi.Shared;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using WebApi.Data;
using Dapper;
using System.Linq;

namespace WebApi.Services
{
    public class RegistrationRequestRepository : IRegistrationRequest
    {
        private ApplicationDbContext applicationDbContext;

        public RegistrationRequestRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public void Add(RegistrationRequest registrationRequest)
        {

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true
            };

            var serializedResult = JsonSerializer.Serialize(registrationRequest, typeof(RegistrationRequest), options)
                .ConvertJsonSpecialCharactersToAscii();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "AddOrUpdateRegistrationRequest";
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
            }
        }

        public void DeleteById(int Id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            IEnumerable<MunicipalitiesOfMexico> municipalities;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = @"
                   SELECT m.*
                    FROM MunicipalitiesOfMexico m
                    JOIN StatesOfMexico s ON s.id = m.estado_id
                    WHERE s.nombre = @nameOfState
                    ORDER BY m.nombre;";

                municipalities = connection.Query<MunicipalitiesOfMexico>(sql, new { nameOfState });
            }

            return municipalities;
        }

        public RegistrationRequest GetRegistrationRequestById(int id)
        {
            RegistrationRequest registrationRequest = null;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                    SELECT rr.Id
                        , IFNULL(rr.HowYouHearAboutUs, '') HowYouHearAboutUs
                        , rr.CreationDate
                        , IFNULL(rr.RequestorId, 0) RequestorId
                        , IFNULL(rr.MinorId, 0) MinorId
                        , IFNULL(rr.Reasons,'') Reasons
                        , IFNULL(rr.FamilyComposition, '') FamilyComposition
                        , IFNULL(rr.FamilyInteraction, '') FamilyInteraction
                        , IFNULL(rr.EconomicSituation, '') EconomicSituation
                        , IFNULL(rr.SituationsOfDomesticViolence, '') SituationsOfDomesticViolence
                        , IFNULL(rr.FamilyHealthStatus, '') FamilyHealthStatus
                        , IFNULL(rr.Comments, '') Comments
                        , rr.RegistrationRequestStatusId
                        , rrs.Id
                        , rrs.Name
                        , r.Id
                        , r.FullName
                        , r.Age
                        , r.DateOfBirth
                        , r.PlaceOfBirth
                        , r.MaritalStatusId
                        , IFNULL(r.Education, '') Education
                        , IFNULL(r.CurrentOccupation, '') CurrentOccupation
                        , r.RelationshipId
                        , r.AddressId
                        , IFNULL(r.JobId, 0) JobId
                        , ms.Id
                        , ms.Name
                        , rs.Id
                        , rs.Name
                        , IFNULL(a.Id, 0 ) Id
                        , IFNULL(a.Street, '') Street
                        , IFNULL(a.HouseNumber, '') HouseNumber
                        , IFNULL(a.PoBox, '') PoBox
                        , IFNULL(a.PhoneNumber, '') PhoneNumber
                        , IFNULL(a.City, '') City
                        , IFNULL(a.ZIP, '') ZIP
                        , IFNULL(a.State, '') State
                        , IFNULL(a.Neighborhood, '') Neighborhood
                        , IFNULL(a.Reference, '') Reference
                        , IFNULL(j.Id, 0) Id
                        , IFNULL(j.Location, '') Location
                        , IFNULL(j.JobTitle, '') JobTitle
                        , IFNULL(j.OfficialHours, '') OfficialHours
                        , IFNULL(j.YearsOfService, 0) YearsOfService
                        , IFNULL(j.Salary, 0) Salary
                        , IFNULL(j.AddressId, 0) AddressId
                        , IFNULL(j.ManagerName, '') ManagerName
                        , IFNULL(j.ManagerPosition, '') ManagerPosition 
                        , IFNULL(aj.Id, 0) Id
                        , IFNULL(aj.Street,'') Street
                        , IFNULL(aj.HouseNumber, '') HouseNumber
                        , IFNULL(aj.PoBox, '') PoBox
                        , IFNULL(aj.PhoneNumber, '') PhoneNumber
                        , IFNULL(aj.City ,'') City
                        , IFNULL(aj.ZIP, '') ZIP
                        , IFNULL(aj.State, '') State
                        , IFNULL(aj.Neighborhood, '') Neighborhood
                        , IFNULL(aj.Reference, '') Reference
                        , m.Id
                        , m.FullName
                        , m.DateOfBirth
                        , m.PlaceOfBirth
                        , m.Age
                        , IFNULL(m.Education, '') Education
                        , IFNULL(m.CurrentOccupation, '') CurrentOccupation
                    FROM RegistrationRequest rr
                    LEFT JOIN Requestor r ON r.Id = rr.RequestorId
                    LEFT JOIN MaritalStatus ms ON ms.Id = r.MaritalStatusId
                    LEFT JOIN Relationship rs ON rs.Id = r.RelationshipId
                    LEFT JOIN Address a ON a.Id = r.AddressId
                    LEFT JOIN Job j ON j.Id = r.JobId
                    LEFT JOIN Address aj ON aj.Id = j.AddressId
                    LEFT JOIN Minor m ON m.Id = rr.MinorId
                    LEFT JOIN RegistrationRequestStatus rrs on rr.RegistrationRequestStatusId = rrs.Id
                    WHERE rr.Id = @id;";
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

                        registrationRequest = new RegistrationRequest
                        {
                            Id = reader.GetInt32(index++),
                            HowYouHearAboutUs = reader.GetString(index++),
                            CreationDate = reader.GetDateTime(index++),
                            RequestorId = reader.GetInt32(index++),
                            MinorId = reader.GetInt32(index++),
                            Reasons = reader.GetString(index++),
                            FamilyComposition = reader.GetString(index++),
                            FamilyInteraction = reader.GetString(index++),
                            EconomicSituation = reader.GetString(index++),
                            SituationsOfDomesticViolence = reader.GetString(index++),
                            FamilyHealthStatus = reader.GetString(index++),
                            Comments = reader.GetString(index++),
                            RegistrationRequestStatusId = reader.GetInt32(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetInt32(index++),
                                Name = reader.GetString(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                MaritalStatusId = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++),
                                RelationshipId = reader.GetInt32(index++),
                                AddressId = reader.GetInt32(index++),
                                JobId = reader.GetInt32(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetInt32(index++),
                                    Street = reader.GetString(index++),
                                    HouseNumber = reader.GetString(index++),
                                    PoBox = reader.GetString(index++),
                                    PhoneNumber = reader.GetString(index++),
                                    City = reader.GetString(index++),
                                    Zip = reader.GetString(index++),
                                    State = reader.GetString(index++),
                                    Neighborhood = reader.GetString(index++),
                                    Reference = reader.GetString(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetInt32(index++),
                                    Location = reader.GetString(index++),
                                    JobTitle = reader.GetString(index++),
                                    OfficialHours = reader.GetString(index++),
                                    YearsOfService = reader.GetInt32(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetInt32(index++),
                                    ManagerName = reader.GetString(index++),
                                    ManagerPosition = reader.GetString(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetInt32(index++),
                                        Street = reader.GetString(index++),
                                        HouseNumber = reader.GetString(index++),
                                        PoBox = reader.GetString(index++),
                                        PhoneNumber = reader.GetString(index++),
                                        City = reader.GetString(index++),
                                        Zip = reader.GetString(index++),
                                        State = reader.GetString(index++),
                                        Neighborhood = reader.GetString(index++),
                                        Reference = reader.GetString(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++)
                            }
                        };

                    }
                }
            }

            return registrationRequest;
        }

        public IEnumerable<RegistrationRequest> GetRegistrationRequestsByMinorName(string minorName)
        {
            RegistrationRequest registrationRequest;
            IList<RegistrationRequest> registrationRequests = new List<RegistrationRequest>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                    SELECT rr.Id
                        , IFNULL(rr.HowYouHearAboutUs, '') HowYouHearAboutUs
                        , rr.CreationDate
                        , IFNULL(rr.RequestorId, 0) RequestorId
                        , IFNULL(rr.MinorId, 0) MinorId
                        , IFNULL(rr.Reasons,'') Reasons
                        , IFNULL(rr.FamilyComposition, '') FamilyComposition
                        , IFNULL(rr.FamilyInteraction, '') FamilyInteraction
                        , IFNULL(rr.EconomicSituation, '') EconomicSituation
                        , IFNULL(rr.SituationsOfDomesticViolence, '') SituationsOfDomesticViolence
                        , IFNULL(rr.FamilyHealthStatus, '') FamilyHealthStatus
                        , IFNULL(rr.Comments, '') Comments
                        , rr.RegistrationRequestStatusId
                        , rrs.Id
                        , rrs.Name
                        , r.Id
                        , r.FullName
                        , r.Age
                        , r.DateOfBirth
                        , r.PlaceOfBirth
                        , r.MaritalStatusId
                        , IFNULL(r.Education, '') Education
                        , IFNULL(r.CurrentOccupation, '') CurrentOccupation
                        , r.RelationshipId
                        , r.AddressId
                        , IFNULL(r.JobId, 0) JobId
                        , ms.Id
                        , ms.Name
                        , rs.Id
                        , rs.Name
                        , IFNULL(a.Id, 0 ) Id
                        , IFNULL(a.Street, '') Street
                        , IFNULL(a.HouseNumber, '') HouseNumber
                        , IFNULL(a.PoBox, '') PoBox
                        , IFNULL(a.PhoneNumber, '') PhoneNumber
                        , IFNULL(a.City, '') City
                        , IFNULL(a.ZIP, '') ZIP
                        , IFNULL(a.State, '') State
                        , IFNULL(a.Neighborhood, '') Neighborhood
                        , IFNULL(a.Reference, '') Reference
                        , IFNULL(j.Id, 0) Id
                        , IFNULL(j.Location, '') Location
                        , IFNULL(j.JobTitle, '') JobTitle
                        , IFNULL(j.OfficialHours, '') OfficialHours
                        , IFNULL(j.YearsOfService, 0) YearsOfService
                        , IFNULL(j.Salary, 0) Salary
                        , IFNULL(j.AddressId, 0) AddressId
                        , IFNULL(j.ManagerName, '') ManagerName
                        , IFNULL(j.ManagerPosition, '') ManagerPosition 
                        , IFNULL(aj.Id, 0) Id
                        , IFNULL(aj.Street,'') Street
                        , IFNULL(aj.HouseNumber, '') HouseNumber
                        , IFNULL(aj.PoBox, '') PoBox
                        , IFNULL(aj.PhoneNumber, '') PhoneNumber
                        , IFNULL(aj.City ,'') City
                        , IFNULL(aj.ZIP, '') ZIP
                        , IFNULL(aj.State, '') State
                        , IFNULL(aj.Neighborhood, '') Neighborhood
                        , IFNULL(aj.Reference, '') Reference
                        , m.Id
                        , m.FullName
                        , m.DateOfBirth
                        , m.PlaceOfBirth
                        , m.Age
                        , IFNULL(m.Education, '') Education
                        , IFNULL(m.CurrentOccupation, '') CurrentOccupation
                    FROM RegistrationRequest rr
                    LEFT JOIN Requestor r ON r.Id = rr.RequestorId
                    LEFT JOIN MaritalStatus ms ON ms.Id = r.MaritalStatusId
                    LEFT JOIN Relationship rs ON rs.Id = r.RelationshipId
                    LEFT JOIN Address a ON a.Id = r.AddressId
                    LEFT JOIN Job j ON j.Id = r.JobId
                    LEFT JOIN Address aj ON aj.Id = j.AddressId
                    LEFT JOIN Minor m ON m.Id = rr.MinorId
                    LEFT JOIN RegistrationRequestStatus rrs on rr.RegistrationRequestStatusId = rrs.Id  
                    WHERE m.FullName like @minorName
                    ORDER BY rr.Id DESC;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "minorName",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.String,
                    Value = string.Concat("%", minorName, "%")
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

                        registrationRequest = new RegistrationRequest
                        {
                            Id = reader.GetInt32(index++),
                            HowYouHearAboutUs = reader.GetString(index++),
                            CreationDate = reader.GetDateTime(index++),
                            RequestorId = reader.GetInt32(index++),
                            MinorId = reader.GetInt32(index++),
                            Reasons = reader.GetString(index++),
                            FamilyComposition = reader.GetString(index++),
                            FamilyInteraction = reader.GetString(index++),
                            EconomicSituation = reader.GetString(index++),
                            SituationsOfDomesticViolence = reader.GetString(index++),
                            FamilyHealthStatus = reader.GetString(index++),
                            Comments = reader.GetString(index++),
                            RegistrationRequestStatusId = reader.GetInt32(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetInt32(index++),
                                Name = reader.GetString(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                MaritalStatusId = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++),
                                RelationshipId = reader.GetInt32(index++),
                                AddressId = reader.GetInt32(index++),
                                JobId = reader.GetInt32(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetInt32(index++),
                                    Street = reader.GetString(index++),
                                    HouseNumber = reader.GetString(index++),
                                    PoBox = reader.GetString(index++),
                                    PhoneNumber = reader.GetString(index++),
                                    City = reader.GetString(index++),
                                    Zip = reader.GetString(index++),
                                    State = reader.GetString(index++),
                                    Neighborhood = reader.GetString(index++),
                                    Reference = reader.GetString(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetInt32(index++),
                                    Location = reader.GetString(index++),
                                    JobTitle = reader.GetString(index++),
                                    OfficialHours = reader.GetString(index++),
                                    YearsOfService = reader.GetInt32(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetInt32(index++),
                                    ManagerName = reader.GetString(index++),
                                    ManagerPosition = reader.GetString(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetInt32(index++),
                                        Street = reader.GetString(index++),
                                        HouseNumber = reader.GetString(index++),
                                        PoBox = reader.GetString(index++),
                                        PhoneNumber = reader.GetString(index++),
                                        City = reader.GetString(index++),
                                        Zip = reader.GetString(index++),
                                        State = reader.GetString(index++),
                                        Neighborhood = reader.GetString(index++),
                                        Reference = reader.GetString(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++)
                            }
                        };

                        registrationRequests.Add(registrationRequest);
                    }
                }
            }

            return registrationRequests;
        }

        public IEnumerable<RegistrationRequest> GetRegistrationRequests(int limitRequest)
        {
            RegistrationRequest registrationRequest;
            IList<RegistrationRequest> registrationRequests = new List<RegistrationRequest>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                    SELECT rr.Id
                        , IFNULL(rr.HowYouHearAboutUs, '') HowYouHearAboutUs
                        , rr.CreationDate
                        , IFNULL(rr.RequestorId, 0) RequestorId
                        , IFNULL(rr.MinorId, 0) MinorId
                        , IFNULL(rr.Reasons,'') Reasons
                        , IFNULL(rr.FamilyComposition, '') FamilyComposition
                        , IFNULL(rr.FamilyInteraction, '') FamilyInteraction
                        , IFNULL(rr.EconomicSituation, '') EconomicSituation
                        , IFNULL(rr.SituationsOfDomesticViolence, '') SituationsOfDomesticViolence
                        , IFNULL(rr.FamilyHealthStatus, '') FamilyHealthStatus
                        , IFNULL(rr.Comments, '') Comments
                        , rr.RegistrationRequestStatusId
                        , rrs.Id
                        , rrs.Name
                        , r.Id
                        , r.FullName
                        , r.Age
                        , r.DateOfBirth
                        , r.PlaceOfBirth
                        , r.MaritalStatusId
                        , IFNULL(r.Education, '') Education
                        , IFNULL(r.CurrentOccupation, '') CurrentOccupation
                        , r.RelationshipId
                        , r.AddressId
                        , IFNULL(r.JobId, 0) JobId
                        , ms.Id
                        , ms.Name
                        , rs.Id
                        , rs.Name
                        , IFNULL(a.Id, 0 ) Id
                        , IFNULL(a.Street, '') Street
                        , IFNULL(a.HouseNumber, '') HouseNumber
                        , IFNULL(a.PoBox, '') PoBox
                        , IFNULL(a.PhoneNumber, '') PhoneNumber
                        , IFNULL(a.City, '') City
                        , IFNULL(a.ZIP, '') ZIP
                        , IFNULL(a.State, '') State
                        , IFNULL(a.Neighborhood, '') Neighborhood
                        , IFNULL(a.Reference, '') Reference
                        , IFNULL(j.Id, 0) Id
                        , IFNULL(j.Location, '') Location
                        , IFNULL(j.JobTitle, '') JobTitle
                        , IFNULL(j.OfficialHours, '') OfficialHours
                        , IFNULL(j.YearsOfService, 0) YearsOfService
                        , IFNULL(j.Salary, 0) Salary
                        , IFNULL(j.AddressId, 0) AddressId
                        , IFNULL(j.ManagerName, '') ManagerName
                        , IFNULL(j.ManagerPosition, '') ManagerPosition 
                        , IFNULL(aj.Id, 0) Id
                        , IFNULL(aj.Street,'') Street
                        , IFNULL(aj.HouseNumber, '') HouseNumber
                        , IFNULL(aj.PoBox, '') PoBox
                        , IFNULL(aj.PhoneNumber, '') PhoneNumber
                        , IFNULL(aj.City ,'') City
                        , IFNULL(aj.ZIP, '') ZIP
                        , IFNULL(aj.State, '') State
                        , IFNULL(aj.Neighborhood, '') Neighborhood
                        , IFNULL(aj.Reference, '') Reference
                        , m.Id
                        , m.FullName
                        , m.DateOfBirth
                        , m.PlaceOfBirth
                        , m.Age
                        , IFNULL(m.Education, '') Education
                        , IFNULL(m.CurrentOccupation, '') CurrentOccupation
                    FROM RegistrationRequest rr
                    LEFT JOIN Requestor r ON r.Id = rr.RequestorId
                    LEFT JOIN MaritalStatus ms ON ms.Id = r.MaritalStatusId
                    LEFT JOIN Relationship rs ON rs.Id = r.RelationshipId
                    LEFT JOIN Address a ON a.Id = r.AddressId
                    LEFT JOIN Job j ON j.Id = r.JobId
                    LEFT JOIN Address aj ON aj.Id = j.AddressId
                    LEFT JOIN Minor m ON m.Id = rr.MinorId
                    LEFT JOIN RegistrationRequestStatus rrs on rr.RegistrationRequestStatusId = rrs.Id
                    LIMIT @limitRequest;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "limitRequest",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.Int32,
                    Value = limitRequest
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

                        registrationRequest = new RegistrationRequest
                        {
                            Id = reader.GetInt32(index++),
                            HowYouHearAboutUs = reader.GetString(index++),
                            CreationDate = reader.GetDateTime(index++),
                            RequestorId = reader.GetInt32(index++),
                            MinorId = reader.GetInt32(index++),
                            Reasons = reader.GetString(index++),
                            FamilyComposition = reader.GetString(index++),
                            FamilyInteraction = reader.GetString(index++),
                            EconomicSituation = reader.GetString(index++),
                            SituationsOfDomesticViolence = reader.GetString(index++),
                            FamilyHealthStatus = reader.GetString(index++),
                            Comments = reader.GetString(index++),
                            RegistrationRequestStatusId = reader.GetInt32(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetInt32(index++),
                                Name = reader.GetString(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                MaritalStatusId = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++),
                                RelationshipId = reader.GetInt32(index++),
                                AddressId = reader.GetInt32(index++),
                                JobId = reader.GetInt32(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetInt32(index++),
                                    Name = reader.GetString(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetInt32(index++),
                                    Street = reader.GetString(index++),
                                    HouseNumber = reader.GetString(index++),
                                    PoBox = reader.GetString(index++),
                                    PhoneNumber = reader.GetString(index++),
                                    City = reader.GetString(index++),
                                    Zip = reader.GetString(index++),
                                    State = reader.GetString(index++),
                                    Neighborhood = reader.GetString(index++),
                                    Reference = reader.GetString(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetInt32(index++),
                                    Location = reader.GetString(index++),
                                    JobTitle = reader.GetString(index++),
                                    OfficialHours = reader.GetString(index++),
                                    YearsOfService = reader.GetInt32(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetInt32(index++),
                                    ManagerName = reader.GetString(index++),
                                    ManagerPosition = reader.GetString(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetInt32(index++),
                                        Street = reader.GetString(index++),
                                        HouseNumber = reader.GetString(index++),
                                        PoBox = reader.GetString(index++),
                                        PhoneNumber = reader.GetString(index++),
                                        City = reader.GetString(index++),
                                        Zip = reader.GetString(index++),
                                        State = reader.GetString(index++),
                                        Neighborhood = reader.GetString(index++),
                                        Reference = reader.GetString(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                                DateOfBirth = reader.GetDateTime(index++),
                                PlaceOfBirth = reader.GetString(index++),
                                Age = reader.GetInt32(index++),
                                Education = reader.GetString(index++),
                                CurrentOccupation = reader.GetString(index++)
                            }
                        };

                        registrationRequests.Add(registrationRequest);
                    }
                }
            }

            return registrationRequests;

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

        public IEnumerable<RegistrationRequestStatus> RegistrationRequestStatuses()
        {
            IEnumerable<RegistrationRequestStatus> registrationRequestStatuses;

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                string sql = "SELECT * FROM RegistrationRequestStatus;";

                registrationRequestStatuses = connection.Query<RegistrationRequestStatus>(sql);
            }

            return registrationRequestStatuses;
        }

        public void Update(RegistrationRequest registrationRequest)
        {
            Add(registrationRequest);
        }
    }
}