set @jsondata  = '{
    "Id": 1,
    "HowYouHearAboutUs": "Supo del albergue por",
    "CreationDate": "0001-01-01T00:00:00",
    "RequestorId": 0,
    "Requestor": {
      "Address": {
        "Id": 0,
        "Street": "Calle",
        "HouseNumber": "Número",
        "PhoneNumber": "3336327025",
        "City": "Guadalajara",
        "Zip": "44260",
        "State": "Jalisco",
        "Neighborhood": "Colonia",
        "Reference": "Referencia"
      },
      "AddressId": 0,
      "Age": 22,
      "DateOfBirth": "1981-01-01T00:00:00",
      "Education": "Escolaridad",
      "FullName": "Nombre de solicitante",
      "Id": 0,
      "Job": {
        "Id": 0,
        "Location": "Lugar de trabajo",
        "JobTitle": "Puesto",
        "OfficialHours": "Horario",
        "YearsOfService": 2,
        "Salary": 100,
        "AddressId": 0,
        "Address": {
          "Id": 0,
          "Street": "Domicilio laboral"
        },
        "ManagerName": "Nombre de jefe directo",
        "ManagerPosition": "Puesto de jefe"
      },
      "JobId": 0,
      "MaritalStatusId": 2,
      "PlaceOfBirth": "Chavinda Michoacán",
      "RelationshipId": 2,
      "CurrentOccupation": "Ocupación"
    },
    "MinorId": 0,
    "Minor": {
      "Id": 0,
      "FullName": "Nombre",
      "DateOfBirth": "2015-01-01T00:00:00",
      "PlaceOfBirth": "Lugar de nacimiento",
      "Age": 2,
      "Education": "Escolaridad",
      "CurrentOccupation": "Ocupación"
    },
    "Reasons": "Motivo de solicitud ",
    "FamilyComposition": "Composición familiar ",
    "FamilyInteraction": "Dinámica familiar ",
    "EconomicSituation": "Situación economica ",
    "SituationsOfDomesticViolence": "Situaciones de violencia ",
    "FamilyHealthStatus": "Estado de salud familiar ",
    "Comments": "Observaciones",
    "RegistrationRequestStatusId": 1
  }';

	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT @jsondata AS'Data';
	
		SELECT 1  ExistentPath
		FROM JSON_TABLE
		WHERE JSON_KEYS(Data, ' $.Requestor.Job.Address.Street') IS NOT NULL;

	
	SELECT 1  ExistentPath
		FROM JSON_TABLE
		WHERE JSON_KEYS(Data, '$.Requestor.Job.Location') IS NOT NULL 
		OR JSON_KEYS(Data, ' $.Requestor.Job.JobTitle') IS NOT NULL;
		
