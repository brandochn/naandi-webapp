using Microsoft.AspNetCore.Mvc.ModelBinding;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Areas.SocialWork.Models
{
    public class RegistrationRequestViewModel : RegistrationRequest, IBaseViewModel
    {
        public IList<MaritalStatus> MaritalStatusList { get; set; }
        public IList<Relationship> RelationshipList { get; set; }
        public IList<StatesOfMexico> StatesOfMexico { get; set; }
        public IList<MunicipalitiesOfMexico> MunicipalitiesOfMexico { get; set; }
        public IList<RegistrationRequest> RegistrationRequests { get; set; }
        public IList<RegistrationRequestStatus> RegistrationRequestStatusList {get; set;}

        public bool IsValid(object value)
        {
            var valid = true;
            var modelState = value as ModelStateDictionary;
            if (modelState != null)
            {
                if (string.IsNullOrEmpty(Requestor?.FullName))
                {
                    modelState.AddModelError(string.Empty, "El nombre del solicitante es requerido");
                    valid = false;
                }

                if (Requestor == null || Requestor.Age <= 0 || Requestor.Age > 100)
                {
                    modelState.AddModelError(string.Empty, "La edad del solicitante deber ser un número entre 1 al 100");
                    valid = false;
                }

                if (Requestor == null || Requestor.MaritalStatusId == 0)
                {
                    modelState.AddModelError(string.Empty, "El estado civil es requerido");
                    valid = false;
                }

                if (Requestor == null || Requestor.RelationshipId == 0)
                {
                    modelState.AddModelError(string.Empty, "El parentesco es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Requestor?.PlaceOfBirth))
                {
                    modelState.AddModelError(string.Empty, "El lugar de nacimiento del solicitante es requerido");
                    valid = false;
                }

                if (Requestor == null || (DateTime.Now - Requestor.DateOfBirth).TotalDays > 27375 // 75 years old
                   || (DateTime.Now - Requestor.DateOfBirth).TotalDays < 1)
                {
                    modelState.AddModelError(string.Empty, "La fecha de nacimiento del solicitante no es valida");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Requestor?.Address?.Street))
                {
                    modelState.AddModelError(string.Empty, "La dirección del solcitante es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Requestor?.Address?.City))
                {
                    modelState.AddModelError(string.Empty, "El municipio del solcitante es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Minor?.FullName))
                {
                    modelState.AddModelError(string.Empty, "El nombre de la menor es requerido");
                    valid = false;
                }

                if (Minor == null || (DateTime.Now - Minor.DateOfBirth).TotalDays > 6570 // 18 years old
                    || (DateTime.Now - Minor.DateOfBirth).TotalDays < 1)
                {
                    modelState.AddModelError(string.Empty, "La fecha de nacimiento de la menor no es valida");
                    valid = false;
                }

                if (Minor == null || Minor.Age <= 0 || Minor.Age > 100)
                {
                    modelState.AddModelError(string.Empty, "La edad de la menor deber ser un número entre 1 al 100");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Minor?.PlaceOfBirth))
                {
                    modelState.AddModelError(string.Empty, "El lugar de nacimiento de la menor es requerido");
                    valid = false;
                }

                 if (RegistrationRequestStatusId == 0)
                {
                    modelState.AddModelError(string.Empty, "El estatus de la solicitud es requerido");
                    valid = false;
                }
            }

            return valid;
        }

        public void LoadMaritalStatuses(IRegistrationRequest registrationRequestRepository)
        {
            MaritalStatusList = registrationRequestRepository.GetMaritalStatuses().ToList();
            MaritalStatusList.Insert(0, new MaritalStatus()
            {
                Id = 0,
                Name = "Selecciona estado civil"
            });
        }

        public void LoadRelationships(IRegistrationRequest registrationRequestRepository)
        {
            RelationshipList = registrationRequestRepository.GetRelationships().ToList();
            RelationshipList.Insert(0, new Relationship()
            {
                Id = 0,
                Name = "Selecciona parentesco"
            });
        }

        public void LoadStatesOfMexico(IRegistrationRequest registrationRequestRepository)
        {
            StatesOfMexico = registrationRequestRepository.GetStatesOfMexico().ToList();
            StatesOfMexico.Insert(0, new StatesOfMexico()
            {
                Nombre = "Selecciona un estado"
            });
        }

        public void LoadMunicipalitiesOfMexico(IRegistrationRequest registrationRequestRepository)
        {
            if (string.IsNullOrEmpty(Requestor?.Address?.State))
            {
                MunicipalitiesOfMexico = new List<MunicipalitiesOfMexico>();
                MunicipalitiesOfMexico.Add(new MunicipalitiesOfMexico()
                {
                    Nombre = "Selecciona un municipio"
                });
            }
            else
            {
                MunicipalitiesOfMexico = registrationRequestRepository
                    .GetMunicipalitiesOfMexicoByStateOfMexicoName(Requestor.Address.State).ToList();
            }
        }

        public void LoadRegistrationRequestStatuses(IRegistrationRequest registrationRequestRepository)
        {
            RegistrationRequestStatusList = registrationRequestRepository.RegistrationRequestStatuses().ToList();
            RegistrationRequestStatusList.Insert(0, new RegistrationRequestStatus()
            {
                Id = 0,
                Name = "Selecciona un estatus"
            });
        }
    }
}