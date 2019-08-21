using MySql.Data.MySqlClient;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApp.Data;
using Naandi.Shared;
using Naandi.Shared.Exceptions;
using System.Text.Json;

namespace WebApp.Services
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

        public IList<MaritalStatus> GetMaritalStatuses()
        {
            IList<MaritalStatus> maritalStatuses = new List<MaritalStatus>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {

                MySqlCommand cmd = new MySqlCommand("SELECT Id, Name FROM MaritalStatus order by Name;", connection);

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maritalStatuses.Add(new MaritalStatus()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return maritalStatuses;
        }

        public IList<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            IList<MunicipalitiesOfMexico> municipalities = new List<MunicipalitiesOfMexico>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                   SELECT m.nombre
                    FROM MunicipalitiesOfMexico m
                    JOIN StatesOfMexico s ON s.id = m.estado_id
                    WHERE s.nombre = @nameOfState
                    ORDER BY m.nombre;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new MySqlParameter()
                {
                    ParameterName = "nameOfState",
                    Direction = System.Data.ParameterDirection.Input,
                    MySqlDbType = MySqlDbType.String,
                    Value = nameOfState
                });

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        municipalities.Add(new MunicipalitiesOfMexico()
                        {
                            Name = reader.GetString(0),
                        });
                    }
                }
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

        public IList<RegistrationRequest> GetRegistrationRequestsByMinorName(string minorName)
        {
            IList<RegistrationRequest> registrationRequests = new List<RegistrationRequest>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                    SELECT rr.Id
                        , rr.CreationDate
                        , r.Id
                        , r.FullName
                        , m.Id
                        , m.FullName
                        , rrs.Id
                        , rrs.Name
                    FROM RegistrationRequest rr
                    LEFT JOIN Requestor r ON r.Id = rr.RequestorId
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

                    int index;
                    while (reader.Read())
                    {
                        index = 0;
                        registrationRequests.Add(new RegistrationRequest()
                        {
                            Id = reader.GetInt32(index++),
                            CreationDate = reader.GetDateTime(index++).ToLocalTime()
                           ,
                            Requestor = new Requestor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++)
                            },
                            Minor = new Minor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                            },
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetInt32(index++),
                                Name = reader.GetString(index++)
                            }
                        });
                    }
                }
            }

            return registrationRequests;
        }

        public IList<RegistrationRequest> GetRegistrationRequests(int limitRequest)
        {
            IList<RegistrationRequest> registrationRequests = new List<RegistrationRequest>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"
                    SELECT rr.Id
                        , rr.CreationDate
                        , r.Id
                        , r.FullName
                        , m.Id
                        , m.FullName
                        , rrs.Id
                        , rrs.Name
                    FROM RegistrationRequest rr
                    LEFT JOIN Requestor r ON r.Id = rr.RequestorId
                    LEFT JOIN Minor m ON m.Id = rr.MinorId
                    LEFT JOIN RegistrationRequestStatus rrs on rr.RegistrationRequestStatusId = rrs.Id  
                    ORDER BY rr.Id DESC
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

                    int index;
                    while (reader.Read())
                    {
                        index = 0;
                        registrationRequests.Add(new RegistrationRequest()
                        {
                            Id = reader.GetInt32(index++),
                            CreationDate = reader.GetDateTime(index++).ToLocalTime()
                           ,
                            Requestor = new Requestor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++)
                            },
                            Minor = new Minor
                            {
                                Id = reader.GetInt32(index++),
                                FullName = reader.GetString(index++),
                            },
                            RegistrationRequestStatus = new RegistrationRequestStatus
                            {
                                Id = reader.GetInt32(index++),
                                Name = reader.GetString(index++)
                            }
                        });
                    }
                }
            }

            return registrationRequests;
        }

        public IList<Relationship> GetRelationships()
        {
            IList<Relationship> relationships = new List<Relationship>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {

                MySqlCommand cmd = new MySqlCommand("SELECT Id, `Name` FROM Relationship order by Name;", connection);

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        relationships.Add(new Relationship()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return relationships;
        }

        public IList<StatesOfMexico> GetStatesOfMexico()
        {
            IList<StatesOfMexico> statesOfMexicoList = new List<StatesOfMexico>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT nombre FROM StatesOfMexico ORDER BY nombre;", connection);

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        statesOfMexicoList.Add(new StatesOfMexico()
                        {
                            Name = reader.GetString(0),
                        });
                    }
                }
            }

            return statesOfMexicoList;
        }

        public IList<RegistrationRequestStatus> RegistrationRequestStatuses()
        {
            IList<RegistrationRequestStatus> registrationRequestStatuses = new List<RegistrationRequestStatus>();

            using (MySqlConnection connection = applicationDbContext.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT Id, Name FROM RegistrationRequestStatus;", connection);

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        registrationRequestStatuses.Add(new RegistrationRequestStatus()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                    }
                }
            }

            return registrationRequestStatuses;
        }

        public void Update(RegistrationRequest registrationRequest)
        {
            Add(registrationRequest);
        }
    }
}
