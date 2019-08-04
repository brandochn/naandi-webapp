CREATE DATABASE  IF NOT EXISTS `naandi`;
USE `naandi`;


--
-- Table structure for table `RegistrationRequestStatus`
--

DROP TABLE IF EXISTS `RegistrationRequestStatus`;

CREATE TABLE `RegistrationRequestStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


--
-- Table structure for table `Address`
--

DROP TABLE IF EXISTS `Address`;

CREATE TABLE `Address` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Street` varchar(200) NOT NULL DEFAULT '0',
  `HouseNumber` varchar(100) DEFAULT '0',
  `PoBox` varchar(10) DEFAULT '0' COMMENT 'Numero interno',
  `PhoneNumber` varchar(12) DEFAULT '0',
  `City` varchar(100) NOT NULL DEFAULT '0',
  `Zip` varchar(100) DEFAULT '0',
  `State` varchar(100) DEFAULT '0',
  `Neighborhood` varchar(100) DEFAULT '0',
  `Reference` varchar(200) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


--
-- Table structure for table `Document`
--

DROP TABLE IF EXISTS `Document`;

CREATE TABLE `Document` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `Job`
--

DROP TABLE IF EXISTS `Job`;

CREATE TABLE `Job` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Location` varchar(100) DEFAULT '0',
  `JobTitle` varchar(100) DEFAULT '0',
  `OfficialHours` varchar(100) DEFAULT '0' COMMENT 'Horario laboral',
  `YearsOfService` int(11) DEFAULT 0,
  `Salary` decimal(18,2) DEFAULT 0.00,
  `AddressId` int(11) DEFAULT 0,
  `ManagerName` varchar(50) DEFAULT '0',
  `ManagerPosition` varchar(50) DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK_Job_Address` (`AddressId`),
  CONSTRAINT `FK_Job_Address` FOREIGN KEY (`AddressId`) REFERENCES `Address` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='DATOS LABORALES';


--
-- Table structure for table `Minor`
--

DROP TABLE IF EXISTS `Minor`;

CREATE TABLE `Minor` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(50) NOT NULL,
  `DateOfBirth` datetime NOT NULL,
  `PlaceOfBirth` varchar(50) NOT NULL,
  `Age` int(11) NOT NULL,
  `Education` varchar(100) DEFAULT NULL,
  `CurrentOccupation` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `MaritalStatus`
--

DROP TABLE IF EXISTS `MaritalStatus`;

CREATE TABLE `MaritalStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='MaritalStatus are MARRIED, single etc..';

--
-- Table structure for table `Relationship`
--

DROP TABLE IF EXISTS `Relationship`;

CREATE TABLE `Relationship` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Parentesco con la menor';

--
-- Table structure for table `Requestor`
--

DROP TABLE IF EXISTS `Requestor`;

CREATE TABLE `Requestor` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) NOT NULL,
  `Age` int(11) NOT NULL,
  `DateOfBirth` datetime NOT NULL,
  `PlaceOfBirth` varchar(200) NOT NULL,
  `MaritalStatusId` int(11) NOT NULL,
  `Education` varchar(100) DEFAULT NULL,
  `CurrentOccupation` varchar(100) DEFAULT NULL,
  `RelationshipId` int(11) NOT NULL,
  `AddressId` int(11) NOT NULL,
  `JobId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Requestor_MaritalStatus` (`MaritalStatusId`),
  KEY `FK_Requestor_Relationship` (`RelationshipId`),
  KEY `FK_Requestor_Address` (`AddressId`),
  KEY `FK_Requestor_Job` (`JobId`),
  CONSTRAINT `FK_Requestor_Address` FOREIGN KEY (`AddressId`) REFERENCES `Address` (`Id`),
  CONSTRAINT `FK_Requestor_Job` FOREIGN KEY (`JobId`) REFERENCES `Job` (`Id`),
  CONSTRAINT `FK_Requestor_MaritalStatus` FOREIGN KEY (`MaritalStatusId`) REFERENCES `MaritalStatus` (`Id`),
  CONSTRAINT `FK_Requestor_Relationship` FOREIGN KEY (`RelationshipId`) REFERENCES `Relationship` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Solicitante como padre, madre, tutor etc...';

--
-- Table structure for table `EntryRegister`
--

DROP TABLE IF EXISTS `EntryRegister`;

CREATE TABLE `EntryRegister` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RequestorId` int(11) DEFAULT NULL,
  `MinorId` int(11) DEFAULT NULL,
  `CreationDate` datetime NOT NULL,
  `SignedBy` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RequestorId_MinorId` (`RequestorId`,`MinorId`),
  KEY `FK_EntryRegister_Minor` (`MinorId`),
  CONSTRAINT `FK_EntryRegister_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`),
  CONSTRAINT `FK_EntryRegister_Requestor` FOREIGN KEY (`RequestorId`) REFERENCES `Requestor` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Registro de ingreso';

--
-- Table structure for table `MinorDocumentRelation`
--

DROP TABLE IF EXISTS `MinorDocumentRelation`;

CREATE TABLE `MinorDocumentRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MinorId` int(11) NOT NULL DEFAULT 0,
  `DocumentId` int(11) NOT NULL DEFAULT 0,
  `DocumentReceived` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MinorId_DocumentId` (`MinorId`,`DocumentId`),
  KEY `FK_MinorDocuemntRelation_Document` (`DocumentId`),
  CONSTRAINT `FK_MinorDocuemntRelation_Document` FOREIGN KEY (`DocumentId`) REFERENCES `Document` (`Id`),
  CONSTRAINT `FK_MinorDocumentRelation_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


--
-- Table structure for table `MunicipalitiesOfMexico`
--

DROP TABLE IF EXISTS `MunicipalitiesOfMexico`;

CREATE TABLE `MunicipalitiesOfMexico` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `estado_id` int(11) NOT NULL COMMENT 'Relación: estados -> id',
  `clave` varchar(3) NOT NULL COMMENT 'CVE_MUN – Clave del municipio',
  `nombre` varchar(100) NOT NULL COMMENT 'NOM_MUN – Nombre del municipio',
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  KEY `estado_id` (`estado_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Municipios de la República Mexicana';

--
-- Table structure for table `RegistrationRequest`
--

DROP TABLE IF EXISTS `RegistrationRequest`;

CREATE TABLE `RegistrationRequest` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HowYouHearAboutUs` varchar(100) DEFAULT NULL,
  `CreationDate` datetime NOT NULL,
  `RequestorId` int(11) DEFAULT NULL,
  `MinorId` int(11) DEFAULT NULL,
  `Reasons` varchar(400) DEFAULT NULL,
  `FamilyComposition` varchar(400) DEFAULT NULL,
  `FamilyInteraction` varchar(400) DEFAULT NULL,
  `EconomicSituation` varchar(200) DEFAULT NULL,
  `SituationsOfDomesticViolence` varchar(200) DEFAULT NULL,
  `FamilyHealthStatus` varchar(200) DEFAULT NULL,
  `Comments` varchar(400) DEFAULT NULL,
  `RegistrationRequestStatusId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RequestorId_MinorId` (`RequestorId`,`MinorId`),
  KEY `FK_RegistrationRequest_Minor` (`MinorId`),
  KEY `FK_RegistrationRequest_RegistrationRequestStatus` (`RegistrationRequestStatusId`),
  CONSTRAINT `FK_RegistrationRequest_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`),
  CONSTRAINT `FK_RegistrationRequest_RegistrationRequestStatus` FOREIGN KEY (`RegistrationRequestStatusId`) REFERENCES `RegistrationRequestStatus` (`Id`),
  CONSTRAINT `FK_RegistrationRequest_Requestor` FOREIGN KEY (`RequestorId`) REFERENCES `Requestor` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='SOLICITUD DE INGRESO';

--
-- Table structure for table `RequestorDocumentRelation`
--

DROP TABLE IF EXISTS `RequestorDocumentRelation`;

CREATE TABLE `RequestorDocumentRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RequestorId` int(11) NOT NULL DEFAULT 0,
  `DocumentId` int(11) NOT NULL DEFAULT 0,
  `DocumentReceived` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RequestorId_DocumentId` (`RequestorId`,`DocumentId`),
  KEY `FK_RquetorDocument_Document` (`DocumentId`),
  CONSTRAINT `FK_RequestorDocument_Requestor` FOREIGN KEY (`RequestorId`) REFERENCES `Requestor` (`Id`),
  CONSTRAINT `FK_RquetorDocument_Document` FOREIGN KEY (`DocumentId`) REFERENCES `Document` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


--
-- Table structure for table `StatesOfMexico`
--

DROP TABLE IF EXISTS `StatesOfMexico`;

CREATE TABLE `StatesOfMexico` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `clave` varchar(2) NOT NULL COMMENT 'CVE_ENT - Clave de la entidad',
  `nombre` varchar(40) NOT NULL COMMENT 'NOM_ENT - Nombre de la entidad',
  `abrev` varchar(10) NOT NULL COMMENT 'NOM_ABR - Nombre abreviado de la entidad',
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Estados de la República Mexicana';


--
-- Routines for database 'naandi'
--

DROP PROCEDURE IF EXISTS `AddOrUpdateRegistrationRequest`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateRegistrationRequest`(
	IN `JSONData` LONGTEXT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
    DECLARE RegistrationRequestId INT;
	DECLARE MinorId INT;
	DECLARE JobId INT;
	DECLARE rowExists INT;
	DECLARE AddressId INT;
	DECLARE RequestorId INT;
	DECLARE MaritalStatusId INT;
	DECLARE RelationshipId INT;
	DECLARE JobAddressId INT;
	DECLARE ExistentPath INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
   
	START TRANSACTION;
	SET autocommit = 0;
 
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.Id') INTO RegistrationRequestId
	FROM JSON_TABLE;
 
	
	IF RegistrationRequestId = 0 THEN
		
		
		INSERT INTO Minor (FullName, DateOfBirth, PlaceOfBirth, Age, Education, CurrentOccupation)
		SELECT
			JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.FullName'))
			,CAST(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.DateOfBirth')) AS datetime)
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.PlaceOfBirth'))
			,JSON_EXTRACT(Data, '$.Minor.Age')
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.CurrentOccupation'))
		FROM JSON_TABLE;
		SET MinorId = LAST_INSERT_ID();
		
		
		SELECT 1 INTO ExistentPath
		FROM JSON_TABLE
		WHERE JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street') IS NOT NULL;
		
		IF ExistentPath = 1 THEN
			INSERT INTO Address (Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference )
			SELECT
				 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.HouseNumber'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PoBox'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PhoneNumber'))
				,IFNULL(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.City')), '')
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Zip'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.State'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Neighborhood'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Reference'))
			FROM JSON_TABLE;
			SET JobAddressId = LAST_INSERT_ID();
		ELSE
			SET JobAddressId = NULL;
		END IF;
		
		
		SELECT 1 INTO ExistentPath
		FROM JSON_TABLE
		WHERE JSON_EXTRACT(Data, ' $.Requestor.Job.Location') IS NOT NULL 
		OR JSON_EXTRACT(Data, ' $.Requestor.Job.JobTitle') IS NOT NULL;
		
		IF ExistentPath = 1 THEN
			INSERT INTO Job(Location ,JobTitle ,OfficialHours ,YearsOfService ,Salary ,AddressId ,ManagerName ,ManagerPosition)
			SELECT
				 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Location'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.JobTitle'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.OfficialHours'))
				,JSON_EXTRACT(Data, '$.Requestor.Job.YearsOfService')
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Salary'))
				,JobAddressId
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerName'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerPosition'))
			FROM JSON_TABLE;
			SET JobId = LAST_INSERT_ID();
		ELSE
			SET JobId = NULL;
		END IF;
		
				
		
		INSERT INTO Address(Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Address.Street'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.HouseNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.PoBox'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.PhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.City'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Zip'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.State'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Neighborhood'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Reference'))
		FROM JSON_TABLE;
		SET AddressId = LAST_INSERT_ID();
		
		INSERT INTO Requestor
		(FullName ,Age ,DateOfBirth ,PlaceOfBirth ,MaritalStatusId ,Education ,CurrentOccupation ,RelationshipId ,AddressId ,JobId)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.FullName'))
			,JSON_EXTRACT(Data, '$.Requestor.Age')
			,CAST(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.DateOfBirth')) AS datetime)
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.PlaceOfBirth'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.MaritalStatusId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.RelationshipId'))
			,AddressId
			,JobId
		FROM JSON_TABLE;
		SET RequestorId = LAST_INSERT_ID();
		
		INSERT INTO RegistrationRequest
		(HowYouHearAboutUs, CreationDate, RequestorId, MinorId, Reasons, FamilyComposition, FamilyInteraction, EconomicSituation
		,SituationsOfDomesticViolence, FamilyHealthStatus, Comments, RegistrationRequestStatusId)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.HowYouHearAboutUs'))
			,UTC_TIMESTAMP()
			,RequestorId
			,MinorId
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Reasons'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyComposition'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyInteraction'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SituationsOfDomesticViolence'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealthStatus'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Comments'))
            ,JSON_EXTRACT(Data, '$.RegistrationRequestStatusId')
		FROM JSON_TABLE;
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM RegistrationRequest WHERE Id = RegistrationRequestId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Registration request not found';
		ELSE			
			
			SELECT
				rr.RequestorId INTO RequestorId
			FROM RegistrationRequest rr
			WHERE rr.Id = RegistrationRequestId;
				
			SELECT rr.MinorId INTO MinorId
			FROM RegistrationRequest rr
			WHERE rr.Id = RegistrationRequestId;
			
			SELECT r.MaritalStatusId INTO MaritalStatusId
			FROM RegistrationRequest rr
			JOIN Requestor r on r.Id = rr.RequestorId
			WHERE rr.Id = RegistrationRequestId;

			SELECT r.RelationshipId INTO RelationshipId
			FROM RegistrationRequest rr
			JOIN Requestor r on r.Id = rr.RequestorId
			WHERE rr.Id = RegistrationRequestId;

			SELECT r.AddressId INTO AddressId
			FROM RegistrationRequest rr
			JOIN Requestor r on r.Id = rr.RequestorId
			WHERE rr.Id = RegistrationRequestId;
			
			SELECT IFNULL(r.JobId,0) INTO JobId
			FROM RegistrationRequest rr
			JOIN Requestor r on r.Id = rr.RequestorId
			WHERE rr.Id = RegistrationRequestId;
			
			SELECT IFNULL(j.AddressId,0) INTO JobAddressId
			FROM RegistrationRequest rr
			JOIN Requestor r on r.Id = rr.RequestorId
			LEFT JOIN Job j on j.Id = r.JobId
			WHERE rr.Id = RegistrationRequestId;
			
			
			UPDATE Minor
			SET
				FullName =           (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.FullName')) FROM JSON_TABLE)
				,DateOfBirth =       (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.DateOfBirth')) AS datetime) FROM JSON_TABLE) 
				,PlaceOfBirth =      (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.PlaceOfBirth')) FROM JSON_TABLE)
				,Age =               (SELECT JSON_EXTRACT(Data, '$.Minor.Age') FROM JSON_TABLE)
				,Education =         (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.Education')) FROM JSON_TABLE)
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.CurrentOccupation')) FROM JSON_TABLE)
			WHERE Id = MinorId;
			
			
			IF JobAddressId = 0 THEN
			
				SELECT 1 INTO ExistentPath
				FROM JSON_TABLE
				WHERE JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street') IS NOT NULL;
		
				IF ExistentPath = 1 THEN
					INSERT INTO Address (Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference )
					SELECT
						 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.HouseNumber'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PoBox'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PhoneNumber'))
						,IFNULL(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.City')), '')
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Zip'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.State'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Neighborhood'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Reference'))
					FROM JSON_TABLE;
					SET JobAddressId = LAST_INSERT_ID();
				END IF;
			ELSE
				UPDATE Address
				SET
					Street = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street')) FROM JSON_TABLE)
					,HouseNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.HouseNumber')) FROM JSON_TABLE)
					,PoBox = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PoBox')) FROM JSON_TABLE)
					,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.PhoneNumber')) FROM JSON_TABLE)
					,City = (SELECT IFNULL(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.City')), '') FROM JSON_TABLE)
					,ZIP = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Zip')) FROM JSON_TABLE)
					,State = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.State')) FROM JSON_TABLE)
					,Neighborhood = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Neighborhood')) FROM JSON_TABLE)
					,Reference = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Address.Reference')) FROM JSON_TABLE)
				WHERE Id = JobAddressId;
			END IF;
			
			
			IF JobId = 0 THEN
				
				SELECT 1 INTO ExistentPath
				FROM JSON_TABLE
				WHERE JSON_EXTRACT(Data, ' $.Requestor.Job.Location') IS NOT NULL 
				OR JSON_EXTRACT(Data, ' $.Requestor.Job.JobTitle') IS NOT NULL;
		
				IF ExistentPath = 1 THEN
					INSERT INTO Job (Location ,JobTitle ,OfficialHours ,YearsOfService ,Salary ,AddressId ,ManagerName ,ManagerPosition)
					SELECT
						 JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Location'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.JobTitle'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.OfficialHours'))
						,JSON_EXTRACT(Data, '$.Requestor.Job.YearsOfService')
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Salary'))
						,JobAddressId
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerName'))
						,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerPosition'))
					FROM JSON_TABLE;
					SET JobId = LAST_INSERT_ID();
				END IF;
			ELSE
				UPDATE Job
					SET
					Location = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Job.Location')) FROM JSON_TABLE)
					,JobTitle = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.JobTitle')) FROM JSON_TABLE)
					,OfficialHours = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.OfficialHours')) FROM JSON_TABLE)
					,YearsOfService = (SELECT JSON_EXTRACT(Data, '$.Requestor.Job.YearsOfService') FROM JSON_TABLE)
					,Salary = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.Salary')) FROM JSON_TABLE)
					,ManagerName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerName')) FROM JSON_TABLE)
					,ManagerPosition = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Job.ManagerPosition')) FROM JSON_TABLE)
				WHERE Id = JobId;
			END IF;
						
			
			UPDATE Address
			SET
				Street = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.Address.Street')) FROM JSON_TABLE)
				,HouseNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.HouseNumber')) FROM JSON_TABLE)
				,PoBox =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.PoBox')) FROM JSON_TABLE)
				,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.PhoneNumber')) FROM JSON_TABLE)
				,City = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.City')) FROM JSON_TABLE)
				,ZIP = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Zip')) FROM JSON_TABLE)
				,State = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.State')) FROM JSON_TABLE)
				,Neighborhood = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Neighborhood')) FROM JSON_TABLE)
				,Reference = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Address.Reference'))  FROM JSON_TABLE)
			WHERE Id = AddressId;
			
			
			UPDATE Requestor
			SET
				FullName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Requestor.FullName')) FROM JSON_TABLE)
				,Age = (SELECT JSON_EXTRACT(Data, '$.Requestor.Age') FROM JSON_TABLE)
				,DateOfBirth = (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.DateOfBirth')) as datetime) FROM JSON_TABLE)
				,PlaceOfBirth = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.PlaceOfBirth')) FROM JSON_TABLE)
				,MaritalStatusId = (SELECT JSON_EXTRACT(Data, '$.Requestor.MaritalStatusId') FROM JSON_TABLE)
				,Education = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.Education')) FROM JSON_TABLE)
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Requestor.CurrentOccupation')) FROM JSON_TABLE)
				,RelationshipId = (SELECT JSON_EXTRACT(Data, '$.Requestor.RelationshipId') FROM JSON_TABLE)
			WHERE Id = RequestorId;
			
			
			UPDATE RegistrationRequest
			SET
				HowYouHearAboutUs = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.HowYouHearAboutUs')) FROM JSON_TABLE)
				,Reasons = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Reasons')) FROM JSON_TABLE)
				,FamilyComposition = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyComposition')) FROM JSON_TABLE)
				,FamilyInteraction = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyInteraction')) FROM JSON_TABLE)
				,EconomicSituation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation')) FROM JSON_TABLE)
				,SituationsOfDomesticViolence = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SituationsOfDomesticViolence')) FROM JSON_TABLE)
				,FamilyHealthStatus = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealthStatus')) FROM JSON_TABLE)
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Comments')) FROM JSON_TABLE)
                ,RegistrationRequestStatusId = (SELECT JSON_EXTRACT(Data, '$.RegistrationRequestStatusId') FROM JSON_TABLE)
			WHERE Id = RegistrationRequestId;
	
		END IF;
	END IF;
	COMMIT; 
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `GenCSharpModel`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GenCSharpModel`(in pTableName VARCHAR(255) )
BEGIN
DECLARE vClassName varchar(255);
declare vClassCode mediumtext;
declare v_codeChunk varchar(1024);
DECLARE v_finished INTEGER DEFAULT 0;
DEClARE code_cursor CURSOR FOR
    SELECT code FROM temp1; 

DECLARE CONTINUE HANDLER 
        FOR NOT FOUND SET v_finished = 1;

set vClassCode ='';

    SELECT (CASE WHEN col1 = col2 THEN col1 ELSE concat(col1,col2)  END) into vClassName
    FROM(
    SELECT CONCAT(UCASE(MID(ColumnName1,1,1)),LCASE(MID(ColumnName1,2))) as col1,
    CONCAT(UCASE(MID(ColumnName2,1,1)),LCASE(MID(ColumnName2,2))) as col2
    FROM
    (SELECT SUBSTRING_INDEX(pTableName, '_', -1) as ColumnName2,
        SUBSTRING_INDEX(pTableName, '_', 1) as ColumnName1) A) B;

    
    CREATE TEMPORARY TABLE IF NOT EXISTS  temp1 ENGINE=MyISAM  
    as (
    select concat( 'public ', ColumnType , ' ' , FieldName,' { get; set; }') code
    FROM(
    SELECT (CASE WHEN col1 = col2 THEN col1 ELSE concat(col1,col2)  END) AS FieldName, 
    case DATA_TYPE 
            when 'bigint' then 'long'
            when 'binary' then 'byte[]'
            when 'bit' then 'bool'
            when 'char' then 'string'
            when 'date' then 'DateTime'
            when 'datetime' then 'DateTime'
            when 'datetime2' then 'DateTime'
            when 'datetimeoffset' then 'DateTimeOffset'
            when 'decimal' then 'decimal'
            when 'float' then 'float'
            when 'image' then 'byte[]'
            when 'int' then 'int'
            when 'money' then 'decimal'
            when 'nchar' then 'char'
            when 'ntext' then 'string'
            when 'numeric' then 'decimal'
            when 'nvarchar' then 'string'
            when 'real' then 'double'
            when 'smalldatetime' then 'DateTime'
            when 'smallint' then 'short'
            when 'mediumint' then 'INT'
            when 'smallmoney' then 'decimal'
            when 'text' then 'string'
            when 'time' then 'TimeSpan'
            when 'timestamp' then 'DateTime'
            when 'tinyint' then 'byte'
            when 'uniqueidentifier' then 'Guid'
            when 'varbinary' then 'byte[]'
            when 'varchar' then 'string'
            when 'year' THEN 'UINT'
            else 'UNKNOWN_' + DATA_TYPE
        end ColumnType
    FROM(
    select CONCAT(UCASE(MID(ColumnName1,1,1)),LCASE(MID(ColumnName1,2))) as col1,
    CONCAT(UCASE(MID(ColumnName2,1,1)),LCASE(MID(ColumnName2,2))) as col2, DATA_TYPE
    from
    (SELECT SUBSTRING_INDEX(COLUMN_NAME, '_', -1) as ColumnName2,
    SUBSTRING_INDEX(COLUMN_NAME, '_', 1) as ColumnName1,
    DATA_TYPE, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS  WHERE table_name = pTableName) A) B)C);

    set vClassCode = '';
    
    OPEN code_cursor;

            get_code: LOOP

                FETCH code_cursor INTO v_codeChunk;

                IF v_finished = 1 THEN
                    LEAVE get_code;
                END IF;

                
                select  CONCAT(vClassCode,'\r\n', v_codeChunk) into  vClassCode ;

            END LOOP get_code;

        CLOSE code_cursor;

drop table temp1;

select concat('public class ',vClassName,'\r\n{', vClassCode,'\r\n}');
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `test_mysql_while_loop`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `test_mysql_while_loop`()
BEGIN
 DECLARE x  INT;
 DECLARE str  VARCHAR(255);
 DECLARE ErrorMessage VARCHAR(2000);
 DECLARE jsondata LONGTEXT;
 
 SET x = 1;

 
 WHILE x  <= 1000 DO
 
	set jsondata  = '{
	    "Id": 0,
	    "HowYouHearAboutUs": "por internet",
	    "CreationDate": "0001-01-01T00:00:00",
	    "RequestorId": 0,
	    "Requestor": {
	        "Address": {
	            "Id": 0,
	            "Street": "Los Altos",
	            "HouseNumber": "1786",
	            "PhoneNumber": "3336327025",
	            "City": "Guadalajara",
	            "Zip": "44260",
	            "State": "Jalisco",
	            "Neighborhood": "Mezquitan Country",
	            "Reference": "Ingnacio Ramirez y Jaime Nuno"
	        },
	        "AddressId": 0,
	        "Age": 36,
	        "DateOfBirth": "1981-06-26T00:00:00",
	        "Education": "Posgrado",
	        "FullName": "Hildebrando Chávez Núñez",
	        "Id": 0,
	        "Job": {
	            "Id": 0,
	            "Location": "Flex",
	            "JobTitle": "Programador",
	            "OfficialHours": "8 a 5",
	            "YearsOfService": 4,
	            "Salary": 5000,
	            "AddressId": 0,
	            "Address": {
	                "Id": 0,
	                "Street": "Av Brad Knight"
	            },
	            "ManagerName": "Jose Chávez",
	            "ManagerPosition": "Director"
	        },
	        "JobId": 0,
	        "MaritalStatusId": 2,
	        "PlaceOfBirth": "Jiquilpan Michoacán",
	        "RelationshipId": 3,
	        "CurrentOccupation": "Programador"
	    },
	    "MinorId": 0,
	    "Minor": {
	        "Id": 0,
	        "FullName": "Luis Matías Sanchez",
	        "DateOfBirth": "2015-05-05T00:00:00",
	        "PlaceOfBirth": "Zamora Michoacán",
	        "Age": 4,
	        "Education": "kinder",
	        "CurrentOccupation": "estudiante"
	    },
	    "Reasons": "Motivo de solicitud ",
	    "FamilyComposition": "Composición familiar ",
	    "FamilyInteraction": "Dinámica familiar ",
	    "EconomicSituation": "Situación economica ",
	    "SituationsOfDomesticViolence": "Situaciones de violencia ",
	    "FamilyHealthStatus": "Estado de salud familiar ",
	    "Comments": "Observaciones",
	    "IsActive": false
	}';

	CALL `AddOrUpdateRegistrationRequest`(jsondata, ErrorMessage);


 	SET  x = x + 1; 
 END WHILE;
 
 END ;;
DELIMITER ;
