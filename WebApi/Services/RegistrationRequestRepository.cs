using System;
using System.Collections.Generic;
using System.Text.Json;
using MySql.Data.MySqlClient;
using Naandi.Shared;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using Dapper;
using WebApi.ExtensionMethods;
using Naandi.Shared.DataBase;

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
                IgnoreNullValues = true
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
                            Id = reader.GetValueOrDefault<int>(index++),
                            HowYouHearAboutUs = reader.GetValueOrNull<string>(index++),
                            CreationDate = reader.GetValueOrDefault<DateTime>(index++),
                            RequestorId = reader.GetValueOrDefault<int>(index++),
                            MinorId = reader.GetValueOrDefault<int>(index++),
                            Reasons = reader.GetValueOrNull<string>(index++),
                            FamilyComposition = reader.GetValueOrNull<string>(index++),
                            FamilyInteraction = reader.GetValueOrNull<string>(index++),
                            EconomicSituation = reader.GetValueOrNull<string>(index++),
                            SituationsOfDomesticViolence = reader.GetValueOrNull<string>(index++),
                            FamilyHealthStatus = reader.GetValueOrNull<string>(index++),
                            Comments = reader.GetValueOrNull<string>(index++),
                            RegistrationRequestStatusId = reader.GetValueOrDefault<int>(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                Name = reader.GetValueOrNull<string>(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                MaritalStatusId = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++),
                                RelationshipId = reader.GetValueOrDefault<int>(index++),
                                AddressId = reader.GetValueOrDefault<int>(index++),
                                JobId = reader.GetValueOrDefault<int>(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Street = reader.GetValueOrNull<string>(index++),
                                    HouseNumber = reader.GetValueOrNull<string>(index++),
                                    PoBox = reader.GetValueOrNull<string>(index++),
                                    PhoneNumber = reader.GetValueOrNull<string>(index++),
                                    City = reader.GetValueOrNull<string>(index++),
                                    Zip = reader.GetValueOrNull<string>(index++),
                                    State = reader.GetValueOrNull<string>(index++),
                                    Neighborhood = reader.GetValueOrNull<string>(index++),
                                    Reference = reader.GetValueOrNull<string>(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Location = reader.GetValueOrNull<string>(index++),
                                    JobTitle = reader.GetValueOrNull<string>(index++),
                                    OfficialHours = reader.GetValueOrNull<string>(index++),
                                    YearsOfService = reader.GetValueOrDefault<int>(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetValueOrDefault<int>(index++),
                                    ManagerName = reader.GetValueOrNull<string>(index++),
                                    ManagerPosition = reader.GetValueOrNull<string>(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetValueOrDefault<int>(index++),
                                        Street = reader.GetValueOrNull<string>(index++),
                                        HouseNumber = reader.GetValueOrNull<string>(index++),
                                        PoBox = reader.GetValueOrNull<string>(index++),
                                        PhoneNumber = reader.GetValueOrNull<string>(index++),
                                        City = reader.GetValueOrNull<string>(index++),
                                        Zip = reader.GetValueOrNull<string>(index++),
                                        State = reader.GetValueOrNull<string>(index++),
                                        Neighborhood = reader.GetValueOrNull<string>(index++),
                                        Reference = reader.GetValueOrNull<string>(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++)
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
                            Id = reader.GetValueOrDefault<int>(index++),
                            HowYouHearAboutUs = reader.GetValueOrNull<string>(index++),
                            CreationDate = reader.GetValueOrDefault<DateTime>(index++),
                            RequestorId = reader.GetValueOrDefault<int>(index++),
                            MinorId = reader.GetValueOrDefault<int>(index++),
                            Reasons = reader.GetValueOrNull<string>(index++),
                            FamilyComposition = reader.GetValueOrNull<string>(index++),
                            FamilyInteraction = reader.GetValueOrNull<string>(index++),
                            EconomicSituation = reader.GetValueOrNull<string>(index++),
                            SituationsOfDomesticViolence = reader.GetValueOrNull<string>(index++),
                            FamilyHealthStatus = reader.GetValueOrNull<string>(index++),
                            Comments = reader.GetValueOrNull<string>(index++),
                            RegistrationRequestStatusId = reader.GetValueOrDefault<int>(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                Name = reader.GetValueOrNull<string>(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                MaritalStatusId = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++),
                                RelationshipId = reader.GetValueOrDefault<int>(index++),
                                AddressId = reader.GetValueOrDefault<int>(index++),
                                JobId = reader.GetValueOrDefault<int>(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Street = reader.GetValueOrNull<string>(index++),
                                    HouseNumber = reader.GetValueOrNull<string>(index++),
                                    PoBox = reader.GetValueOrNull<string>(index++),
                                    PhoneNumber = reader.GetValueOrNull<string>(index++),
                                    City = reader.GetValueOrNull<string>(index++),
                                    Zip = reader.GetValueOrNull<string>(index++),
                                    State = reader.GetValueOrNull<string>(index++),
                                    Neighborhood = reader.GetValueOrNull<string>(index++),
                                    Reference = reader.GetValueOrNull<string>(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Location = reader.GetValueOrNull<string>(index++),
                                    JobTitle = reader.GetValueOrNull<string>(index++),
                                    OfficialHours = reader.GetValueOrNull<string>(index++),
                                    YearsOfService = reader.GetValueOrDefault<int>(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetValueOrDefault<int>(index++),
                                    ManagerName = reader.GetValueOrNull<string>(index++),
                                    ManagerPosition = reader.GetValueOrNull<string>(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetValueOrDefault<int>(index++),
                                        Street = reader.GetValueOrNull<string>(index++),
                                        HouseNumber = reader.GetValueOrNull<string>(index++),
                                        PoBox = reader.GetValueOrNull<string>(index++),
                                        PhoneNumber = reader.GetValueOrNull<string>(index++),
                                        City = reader.GetValueOrNull<string>(index++),
                                        Zip = reader.GetValueOrNull<string>(index++),
                                        State = reader.GetValueOrNull<string>(index++),
                                        Neighborhood = reader.GetValueOrNull<string>(index++),
                                        Reference = reader.GetValueOrNull<string>(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++)
                            }
                        };

                        registrationRequests.Add(registrationRequest);
                    }
                }
            }

            return registrationRequests;
        }

        public IEnumerable<RegistrationRequest> GetRegistrationRequests()
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
                    LEFT JOIN RegistrationRequestStatus rrs on rr.RegistrationRequestStatusId = rrs.Id;";
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

                        registrationRequest = new RegistrationRequest
                        {
                            Id = reader.GetValueOrDefault<int>(index++),
                            HowYouHearAboutUs = reader.GetValueOrNull<string>(index++),
                            CreationDate = reader.GetValueOrDefault<DateTime>(index++),
                            RequestorId = reader.GetValueOrDefault<int>(index++),
                            MinorId = reader.GetValueOrDefault<int>(index++),
                            Reasons = reader.GetValueOrNull<string>(index++),
                            FamilyComposition = reader.GetValueOrNull<string>(index++),
                            FamilyInteraction = reader.GetValueOrNull<string>(index++),
                            EconomicSituation = reader.GetValueOrNull<string>(index++),
                            SituationsOfDomesticViolence = reader.GetValueOrNull<string>(index++),
                            FamilyHealthStatus = reader.GetValueOrNull<string>(index++),
                            Comments = reader.GetValueOrNull<string>(index++),
                            RegistrationRequestStatusId = reader.GetValueOrDefault<int>(index++),
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                Name = reader.GetValueOrNull<string>(index++)
                            },

                            Requestor = new Requestor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                MaritalStatusId = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++),
                                RelationshipId = reader.GetValueOrDefault<int>(index++),
                                AddressId = reader.GetValueOrDefault<int>(index++),
                                JobId = reader.GetValueOrDefault<int>(index++),

                                Maritalstatus = new MaritalStatus
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++)
                                },

                                Relationship = new Relationship
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Name = reader.GetValueOrNull<string>(index++),
                                },

                                Address = new Address
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Street = reader.GetValueOrNull<string>(index++),
                                    HouseNumber = reader.GetValueOrNull<string>(index++),
                                    PoBox = reader.GetValueOrNull<string>(index++),
                                    PhoneNumber = reader.GetValueOrNull<string>(index++),
                                    City = reader.GetValueOrNull<string>(index++),
                                    Zip = reader.GetValueOrNull<string>(index++),
                                    State = reader.GetValueOrNull<string>(index++),
                                    Neighborhood = reader.GetValueOrNull<string>(index++),
                                    Reference = reader.GetValueOrNull<string>(index++)
                                },

                                Job = new Job
                                {
                                    Id = reader.GetValueOrDefault<int>(index++),
                                    Location = reader.GetValueOrNull<string>(index++),
                                    JobTitle = reader.GetValueOrNull<string>(index++),
                                    OfficialHours = reader.GetValueOrNull<string>(index++),
                                    YearsOfService = reader.GetValueOrDefault<int>(index++),
                                    Salary = reader.GetDecimal(index++),
                                    AddressId = reader.GetValueOrDefault<int>(index++),
                                    ManagerName = reader.GetValueOrNull<string>(index++),
                                    ManagerPosition = reader.GetValueOrNull<string>(index++),

                                    Address = new Address
                                    {
                                        Id = reader.GetValueOrDefault<int>(index++),
                                        Street = reader.GetValueOrNull<string>(index++),
                                        HouseNumber = reader.GetValueOrNull<string>(index++),
                                        PoBox = reader.GetValueOrNull<string>(index++),
                                        PhoneNumber = reader.GetValueOrNull<string>(index++),
                                        City = reader.GetValueOrNull<string>(index++),
                                        Zip = reader.GetValueOrNull<string>(index++),
                                        State = reader.GetValueOrNull<string>(index++),
                                        Neighborhood = reader.GetValueOrNull<string>(index++),
                                        Reference = reader.GetValueOrNull<string>(index++)
                                    }
                                }
                            },

                            Minor = new Minor
                            {
                                Id = reader.GetValueOrDefault<int>(index++),
                                FullName = reader.GetValueOrNull<string>(index++),
                                DateOfBirth = reader.GetValueOrDefault<DateTime>(index++),
                                PlaceOfBirth = reader.GetValueOrNull<string>(index++),
                                Age = reader.GetValueOrDefault<int>(index++),
                                Education = reader.GetValueOrNull<string>(index++),
                                CurrentOccupation = reader.GetValueOrNull<string>(index++)
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