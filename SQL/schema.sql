CREATE DATABASE IF NOT EXISTS `naandi` CHARACTER SET = 'utf8mb4' COLLATE = 'utf8mb4_unicode_ci';
USE `naandi`;

SET FOREIGN_KEY_CHECKS = 0;

-- ALTER DATABASE naandi  CHARACTER SET = 'utf8mb4' COLLATE = 'utf8mb4_unicode_ci';

--
-- Table structure for table `RegistrationRequestStatus`
--

DROP TABLE IF EXISTS `RegistrationRequestStatus`;

CREATE TABLE `RegistrationRequestStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;


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
) ENGINE=InnoDB;


--
-- Table structure for table `Document`
--

DROP TABLE IF EXISTS `Document`;

CREATE TABLE `Document` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

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
) ENGINE=InnoDB COMMENT='DATOS LABORALES';

--
-- Table structure for table `FormalEducation`
--

DROP TABLE IF EXISTS `FormalEducation`;

CREATE TABLE `FormalEducation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CanItRead` tinyint(1)  DEFAULT 0,
  `CanItWrite` tinyint(1) DEFAULT 0,
  `IsItStudyingNow` tinyint(1) DEFAULT 0,
  `CurrentGrade` varchar(50) DEFAULT NULL,
  `ReasonsToStopStudying` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Escolaridad';

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
  `FormalEducationId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Minor_FormalEducation` (`FormalEducationId`),
  CONSTRAINT `FK_Minor_FormalEducation` FOREIGN KEY (`FormalEducationId`) REFERENCES `FormalEducation` (`Id`)
) ENGINE=InnoDB;

--
-- Table structure for table `MaritalStatus`
--

DROP TABLE IF EXISTS `MaritalStatus`;

CREATE TABLE `MaritalStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='MaritalStatus are MARRIED, single etc..';

--
-- Table structure for table `Relationship`
--

DROP TABLE IF EXISTS `Relationship`;

CREATE TABLE `Relationship` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Parentesco con la menor';

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
) ENGINE=InnoDB COMMENT='Solicitante como padre, madre, tutor etc...';

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
) ENGINE=InnoDB COMMENT='Registro de ingreso';

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
) ENGINE=InnoDB;


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
) ENGINE=InnoDB COMMENT='Municipios de la República Mexicana';

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
  `SocialWorkerName` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RequestorId_MinorId` (`RequestorId`,`MinorId`),
  KEY `FK_RegistrationRequest_Minor` (`MinorId`),
  KEY `FK_RegistrationRequest_RegistrationRequestStatus` (`RegistrationRequestStatusId`),
  CONSTRAINT `FK_RegistrationRequest_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`),
  CONSTRAINT `FK_RegistrationRequest_RegistrationRequestStatus` FOREIGN KEY (`RegistrationRequestStatusId`) REFERENCES `RegistrationRequestStatus` (`Id`),
  CONSTRAINT `FK_RegistrationRequest_Requestor` FOREIGN KEY (`RequestorId`) REFERENCES `Requestor` (`Id`)
) ENGINE=InnoDB COMMENT='SOLICITUD DE INGRESO';

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
) ENGINE=InnoDB;


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
) ENGINE=InnoDB COMMENT='Estados de la República Mexicana';

--
-- Table structure for table `Spouse`
--

DROP TABLE IF EXISTS `Spouse`;

CREATE TABLE `Spouse` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) NOT NULL,
  `Age` int(11) NOT NULL,
  `CurrentOccupation` varchar(100) DEFAULT NULL,
  `Comments`	varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Conyuge info';

--
-- Table structure for table `LegalGuardian`
--

DROP TABLE IF EXISTS `LegalGuardian`;

CREATE TABLE `LegalGuardian` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) NOT NULL,
  `Age` int(11) NOT NULL,
  `PlaceOfBirth` varchar(200) NOT NULL,
  `MaritalStatusId` int(11) NOT NULL,
  `Education` varchar(100) DEFAULT NULL,
  `CurrentOccupation` varchar(100) DEFAULT NULL,
  `RelationshipId` int(11) NOT NULL,
  `AddressId` int(11) NOT NULL,
  `CellPhoneNumber` varchar(12) DEFAULT NULL,
  `PhoneNumber` varchar(12) DEFAULT NULL,
  `Errand` varchar(100) DEFAULT NULL COMMENT 'Recados', 
  `SpouseId` int(11) NULL,
  `DateOfBirth` datetime,
  PRIMARY KEY (`Id`),
  KEY `FK_LegalGuardian_MaritalStatus` (`MaritalStatusId`),
  KEY `FK_LegalGuardian_Relationship` (`RelationshipId`),
  KEY `FK_LegalGuardian_Address` (`AddressId`),
  KEY `FK_LegalGuardian_Spouse` (SpouseId),
  CONSTRAINT `FK_LegalGuardian_Address` FOREIGN KEY (`AddressId`) REFERENCES `Address` (`Id`),
  CONSTRAINT `FK_LegalGuardian_MaritalStatus` FOREIGN KEY (`MaritalStatusId`) REFERENCES `MaritalStatus` (`Id`),
  CONSTRAINT `FK_LegalGuardian_Relationship` FOREIGN KEY (`RelationshipId`) REFERENCES `Relationship` (`Id`),
  CONSTRAINT `FK_LegalGuardian_Spouse` FOREIGN KEY (`SpouseId`) REFERENCES `Spouse` ( `Id`)
) ENGINE=InnoDB COMMENT='Tutor legal info';

--
-- Table structure for table `PreviousFoundation`
--

DROP TABLE IF EXISTS `PreviousFoundation`;

CREATE TABLE `PreviousFoundation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Familiar` varchar(300) DEFAULT NULL,
  `Procuraduria` varchar(300) DEFAULT NULL,
  `Dif` varchar(300) DEFAULT NULL,
  `Otro` varchar(300) DEFAULT NULL,
  `InstitucionAnterior` varchar(300) DEFAULT NULL,
  `TiempoDeEstadia` varchar(300) DEFAULT NULL,
  `MotivoDeEgreso` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='No encontre traducción la ingles para esta tabla (DerivadaPor)';


--
-- Table structure for table `FamilyHealth`
--

DROP TABLE IF EXISTS `FamilyHealth`;

CREATE TABLE `FamilyHealth` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyHealthStatus` varchar(300) DEFAULT NULL,
  `DerechoHambienteAServiciosDeSalud` varchar(300) DEFAULT NULL,
  `Tipo` varchar(300) DEFAULT NULL,
  `EnfermedadesCronicasDegenerativas` varchar(300) DEFAULT NULL,
  `ConsumoDeTabaco` varchar(300) DEFAULT NULL,
  `ConsumoDeAlcohol` varchar(300) DEFAULT NULL,
  `ConsumoDeDrogas` varchar(300) DEFAULT NULL,
  `Comments` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Salud familiar no tengo la traducción correcta para algunas columnas';


--
-- Table structure for table `FamilyMembers`
--

DROP TABLE IF EXISTS `FamilyMembers`;

CREATE TABLE `FamilyMembers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyInteraction` varchar(400) DEFAULT NULL,
  `Comments` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Composicion Familiar no tengo la traducción correcta para algunas columnas';

--
-- Table structure for table `FamilyMembersDetails`
--

DROP TABLE IF EXISTS `FamilyMembersDetails`;

CREATE TABLE `FamilyMembersDetails` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(50) NOT NULL,
  `Age` int(11) NOT NULL,
  `MaritalStatusId` int(11) NOT NULL,
  `RelationshipId` int(11) NOT NULL,
  `Education` varchar(100) DEFAULT NULL,
  `CurrentOccupation` varchar(100) DEFAULT NULL,
  `FamilyMembersId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_FamilyMembersDetails_MaritalStatus` (`MaritalStatusId`),
  CONSTRAINT `FK_FamilyMembersDetails_MaritalStatus` FOREIGN KEY (`MaritalStatusId`) REFERENCES `MaritalStatus` (`Id`),
  KEY `FK_FamilyMembersDetails_Relationship` (`RelationshipId`),
  CONSTRAINT `FK_FamilyMembersDetails_Relationship` FOREIGN KEY (`RelationshipId`) REFERENCES `Relationship` (`Id`),
  KEY `FK_FamilyMembersDetails_FamilyMembers` (`FamilyMembersId`),
  CONSTRAINT `FK_FamilyMembersDetails_FamilyMembers` FOREIGN KEY (`FamilyMembersId`) REFERENCES `FamilyMembers` (`Id`)
) ENGINE=InnoDB COMMENT='Detalles de Composicion Familiar';


--
-- Table structure for table `TypesOfHouses`
--

DROP TABLE IF EXISTS `TypesOfHouses`;

CREATE TABLE `TypesOfHouses` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Tipo de vivienda';


--
-- Table structure for table `TipoDeMobiliario`
--

DROP TABLE IF EXISTS `TipoDeMobiliario`;

CREATE TABLE `TipoDeMobiliario` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Tipo de Mobiliario, no tengo la traducción correcta para esta tabla';

--
-- Table structure for table `TypeOfDistrict`
--

DROP TABLE IF EXISTS `TypeOfDistrict`;

CREATE TABLE `TypeOfDistrict` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Tipo de colonia';

--
-- Table structure for table `District`
--

DROP TABLE IF EXISTS `District`;

CREATE TABLE `District` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeOfDistrictId` int(11) NOT NULL,
  `AguaPotable` varchar(100)  NULL,
  `Telefono` varchar(10)  NULL,
  `Electricidad` varchar(100)  NULL,
  `Drenaje` varchar(100)  NULL,
  `Hospital` varchar(100)  NULL,
  `Correo` varchar(100)  NULL,
  `Escuela` varchar(100)  NULL,
  `Policia` varchar(100)  NULL,
  `AlumbradoPublico` varchar(100)  NULL,
  `ViasDeAcceso` varchar(100)  NULL,
  `TransportePublico` varchar(100)  NULL,
  `AseoPublico` varchar(100)  NULL,
  `Iglesia` varchar(100)  NULL,
  `Mercado` varchar(100)  NULL,
  `Otros` varchar(100)  NULL,
  `Description` varchar(100)  NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_District_TypeOfDistrict` (`TypeOfDistrictId`),
  CONSTRAINT `FK_District_TypeOfDistrict` FOREIGN KEY (`TypeOfDistrictId`) REFERENCES `TypeOfDistrict` (`Id`)
) ENGINE=InnoDB COMMENT='COLONIA, no tengo la traducción correcta para algunas columnas';

--
-- Table structure for table `HouseLayout`
--

DROP TABLE IF EXISTS `HouseLayout`;

CREATE TABLE `HouseLayout` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Bedroom` varchar(100) NULL,
  `Dinningroom` varchar(100) NULL,
  `Kitchen` varchar(100) NULL,
  `Livingroom` varchar(100) NULL,
  `Bathroom` varchar(100) NULL,
  `Patio` varchar(100) NULL,
  `Garage` varchar(100) NULL,
  `Backyard` varchar(100) NULL,
  `Other` varchar(100) NULL,
  `Ground` varchar(100) NULL,
  `Walls` varchar(100) NULL,
  `Roof` varchar(100) NULL,
  `Description` varchar(400) NULL,
  `TipoDeMobiliarioId` INT NULL,
  `CharacteristicsOfFurniture` varchar(400) NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_HouseLayout_TipoDeMobiliario` (`TipoDeMobiliarioId`),
  CONSTRAINT `FK_HouseLayout_TipoDeMobiliario` FOREIGN KEY (`TipoDeMobiliarioId`) REFERENCES `TipoDeMobiliario` (`Id`)
) ENGINE=InnoDB COMMENT='Distribución de la vivienda';


--
-- Table structure for table `EconomicSituation`
--

DROP TABLE IF EXISTS `EconomicSituation`;

CREATE TABLE `EconomicSituation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NivelSocioEconomico` varchar(200),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='SITUACIÓN ECONÓMICA no tengo la traducción correcta para algunas columnas';


--
-- Table structure for table `Patrimony `
--

DROP TABLE IF EXISTS `Patrimony`;

CREATE TABLE `Patrimony` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Patrimonio familiar';

--
-- Table structure for table `EconomicSituationPatrimonyRelation`
--

DROP TABLE IF EXISTS `EconomicSituationPatrimonyRelation`;

CREATE TABLE `EconomicSituationPatrimonyRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EconomicSituationId` int(11) NOT NULL ,
  `PatrimonyId`  int(11) NOT NULL,
  `Value` varchar(100) NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `EconomicSituationId_PatrimonyId` (`EconomicSituationId`,`PatrimonyId`),
  KEY `FK_EconomicSituationPatrimonyRelation_Patrimony` (`PatrimonyId`),
  CONSTRAINT `FK_EconomicSituationPatrimonyRelation_Patrimony` FOREIGN KEY (`PatrimonyId`) REFERENCES `Patrimony` (`Id`),
  KEY `FK_EconomicSituationPatrimonyRelation_EconomicSituation` (`EconomicSituationId`),
  CONSTRAINT `FK_EconomicSituationPatrimonyRelation_EconomicSituation` FOREIGN KEY (`EconomicSituationId`) REFERENCES `EconomicSituation` (`Id`)
) ENGINE=InnoDB;


--
-- Table structure for table `HomeAcquisition`
--

DROP TABLE IF EXISTS `HomeAcquisition`;

CREATE TABLE `HomeAcquisition` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Adquisición de la vivienda (prestada, rentada o propia)';

--
-- Table structure for table `SocioEconomicStudy`
--

DROP TABLE IF EXISTS `SocioEconomicStudy`;

CREATE TABLE `SocioEconomicStudy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `HomeAcquisitionId` int(11) DEFAULT NULL,
  `NombrePropietario` varchar(100) DEFAULT NULL,
  `MedioAdquisicion` varchar(100) DEFAULT NULL,
  `TypesOfHousesId` int(11) DEFAULT NULL,
  `HouseLayoutId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SocioeconomicStudy_TypesOfHouses` (`TypesOfHousesId`),
  CONSTRAINT `FK_SocioeconomicStudy_TypesOfHouses` FOREIGN KEY (`TypesOfHousesId`) REFERENCES `TypesOfHouses` (`Id`),
  KEY `FK_SocioeconomicStudy_HouseLayout` (`HouseLayoutId`),
  CONSTRAINT `FK_SocioeconomicStudy_HouseLayout` FOREIGN KEY (`HouseLayoutId`) REFERENCES `HouseLayout` (`Id`),
  KEY `FK_SocioeconomicStudy_HomeAcquisition` (`HomeAcquisitionId`),
  CONSTRAINT `FK_SocioeconomicStudy_HomeAcquisition` FOREIGN KEY (`HomeAcquisitionId`) REFERENCES `HomeAcquisition` (`Id`)
) ENGINE=InnoDB COMMENT='SOCIOECONOMICO no tengo la traducción correcta para algunas columnas';

--
-- Table structure for table `FamilyNutrition`
--

DROP TABLE IF EXISTS `FamilyNutrition`;

CREATE TABLE `FamilyNutrition` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Comments` varchar(400) DEFAULT NULL,
  `FoodAllergy` varchar(200),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='ALIMENTACION FAMILIAR';


--
-- Table structure for table `Food `
--

DROP TABLE IF EXISTS `Food`;

CREATE TABLE `Food` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Alimentos';

--
-- Table structure for table `Frequency `
--

DROP TABLE IF EXISTS `Frequency`;

CREATE TABLE `Frequency` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Frecuencia de Alimentos';

--
-- Table structure for table `FamilyNutritionFoodRelation`
--

DROP TABLE IF EXISTS `FamilyNutritionFoodRelation`;

CREATE TABLE `FamilyNutritionFoodRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyNutritionId` int(11) NOT NULL ,
  `FoodId`  int(11) NOT NULL,
  `FrequencyId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `FamilyNutritionId_FoodId` (`FamilyNutritionId`,`FoodId`),
  KEY `FK_FamilyNutritionFoodRelation_Food` (`FoodId`),
  CONSTRAINT `FK_FamilyNutritionFoodRelation_Food` FOREIGN KEY (`FoodId`) REFERENCES `Food` (`Id`),
  KEY `FK_FamilyNutritionFoodRelation_FamilyNutrition` (`FamilyNutritionId`),
  CONSTRAINT `FK_FamilyNutritionFoodRelation_FamilyNutrition` FOREIGN KEY (`FamilyNutritionId`) REFERENCES `FamilyNutrition` (`Id`),
  KEY `FK_FamilyNutritionFoodRelation_Frequency` (`FrequencyId`),
  CONSTRAINT `FK_FamilyNutritionFoodRelation_Frequency` FOREIGN KEY (`FrequencyId`) REFERENCES `Frequency` (`Id`)
) ENGINE=InnoDB;


--
-- Table structure for table `BenefitsProvided`
--

DROP TABLE IF EXISTS `BenefitsProvided`;

CREATE TABLE `BenefitsProvided` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Apoyos y Servicios Otorgados no tengo la traducción correcta para algunas columnas';

--
-- Table structure for table `BenefitsProvidedDetails`
--

DROP TABLE IF EXISTS `BenefitsProvidedDetails`;

CREATE TABLE `BenefitsProvidedDetails` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Institucion` varchar(100),
  `ApoyoRecibido` varchar(200),
  `Monto`   decimal(13, 2),
  `Periodo` varchar(200),
  `BenefitsProvidedId` int(11),
  PRIMARY KEY (`Id`),
  KEY `FK_BenefitsProvided_BenefitsProvidedDetails` (`BenefitsProvidedId`),
  CONSTRAINT `FK_BenefitsProvided_BenefitsProvidedDetails` FOREIGN KEY (`BenefitsProvidedId`) REFERENCES `BenefitsProvided` (`Id`)
) ENGINE=InnoDB COMMENT='Details about BenefitsProvided';

--
-- Table structure for table `TipoMovimiento`
--

DROP TABLE IF EXISTS `TipoMovimiento`;

CREATE TABLE `TipoMovimiento` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Tipo de movimiento (Ingreso o Egreso) no tengo la traducción correcta al ingles para esta tabla';

--
-- Table structure for table `Movimiento`
--

DROP TABLE IF EXISTS `Movimiento`;

CREATE TABLE `Movimiento` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `TipoMovimientoId` int(11),
  PRIMARY KEY (`Id`),
   KEY `FK_Movimiento_TipoMovimiento` (`TipoMovimientoId`),
  CONSTRAINT `FK_Movimiento_TipoMovimiento` FOREIGN KEY (`TipoMovimientoId`) REFERENCES `TipoMovimiento` (`Id`)
) ENGINE=InnoDB COMMENT='Movimiento (conceptos de movimientos de Ingreso o Egreso) no tengo la traducción correcta al ingles para esta tabla';

--
-- Table structure for table `IngresosEgresosMensuales`
--

DROP TABLE IF EXISTS `IngresosEgresosMensuales`;

CREATE TABLE `IngresosEgresosMensuales` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Comments` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='INGRESOS Y EGRESOS  MENSUALES no tengo la traducción correcta al ingles para esta tabla';

--
-- Table structure for table `IngresosEgresosMensualesMovimientoRelation`
--

DROP TABLE IF EXISTS `IngresosEgresosMensualesMovimientoRelation`;

CREATE TABLE `IngresosEgresosMensualesMovimientoRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IngresosEgresosMensualesId` int(11) NOT NULL ,
  `MovimientoId`  int(11) NOT NULL,
  `Monto`   decimal(13, 2) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IngresosEgresosMensualesId_MovimientoId` (`IngresosEgresosMensualesId`,`MovimientoId`),
  KEY `FK_IngresosEgresosMensualesMovimientoRelation_Movimiento` (`MovimientoId`),
  CONSTRAINT `FK_IngresosEgresosMensualesMovimientoRelation_Movimiento` FOREIGN KEY (`MovimientoId`) REFERENCES `Movimiento` (`Id`),
  KEY `FK_IngresosEgresosMensualesMovimientoRelation_IngEgrMens` (`IngresosEgresosMensualesId`),
  CONSTRAINT `FK_IngresosEgresosMensualesMovimientoRelation_IngEgrMens` FOREIGN KEY (`IngresosEgresosMensualesId`) REFERENCES `IngresosEgresosMensuales` (`Id`)
) ENGINE=InnoDB;

--
-- Table structure for table `FamilyResearch`
--

DROP TABLE IF EXISTS `FamilyResearch`;

CREATE TABLE `FamilyResearch` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VisitDate` datetime NOT NULL,
  `VisitTime` datetime NOT NULL,
  `Family` varchar(100) NULL,
  `RequestReasons` varchar(300)  NULL,
  `RedesDeApoyoFamiliares` varchar(400) DEFAULT NULL,
  `SituationsOfDomesticViolence` varchar(200) DEFAULT NULL,
  `FamilyExpectations` varchar(300) DEFAULT NULL,
  `FamilyDiagnostic` varchar(300) DEFAULT NULL,
  `ProblemsIdentified` varchar(300) DEFAULT NULL,
  `CaseStudyConclusion` varchar(300) DEFAULT NULL,
  `Recommendations` varchar(300) DEFAULT NULL,
  `VisualSupports` varchar(300) DEFAULT NULL,
  `Sketch` varchar(300) DEFAULT NULL,
  `LegalGuardianId` int(11) DEFAULT NULL,
  `MinorId` int(11) DEFAULT NULL,
  `PreviousFoundationId` int(11) DEFAULT NULL,
  `FamilyHealthId` int(11) DEFAULT NULL,
  `FamilyMembersId` int(11) DEFAULT NULL,
  `SocioEconomicStudyId` int(11) DEFAULT NULL,
  `DistrictId` int(11) DEFAULT NULL,
  `EconomicSituationId` int(11) DEFAULT NULL,
  `FamilyNutritionId` int(11) DEFAULT NULL,
  `BenefitsProvidedId` int(11) DEFAULT NULL,
  `IngresosEgresosMensualesId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_FamilyResearch_Minor` (`MinorId`),
  CONSTRAINT `FK_FamilyResearch_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`),
  KEY `FK_FamilyResearch_LegalGuardian` (`LegalGuardianId`),
  CONSTRAINT `FK_FamilyResearch_LegalGuardian` FOREIGN KEY (`LegalGuardianId`) REFERENCES `LegalGuardian` (`Id`),
  KEY `FK_FamilyResearch_PreviousFoundation` (`PreviousFoundationId`),
  CONSTRAINT `FK_FamilyResearch_PreviousFoundation` FOREIGN KEY (`PreviousFoundationId`) REFERENCES `PreviousFoundation` (`Id`),
  KEY `FK_FamilyResearch_FamilyHealth` (`FamilyHealthId`),
  CONSTRAINT `FK_FamilyResearch_FamilyHealth` FOREIGN KEY (`FamilyHealthId`) REFERENCES `FamilyHealth` (`Id`),
  KEY `FK_FamilyResearch_FamilyMembers` (`FamilyMembersId`),
  CONSTRAINT `FK_FamilyResearch_FamilyMembers` FOREIGN KEY (`FamilyMembersId`) REFERENCES `FamilyMembers` (`Id`),
  KEY `FK_FamilyResearch_SocioeconomicStudy` (`SocioEconomicStudyId`),
  CONSTRAINT `FK_FamilyResearch_SocioeconomicStudy` FOREIGN KEY (`SocioEconomicStudyId`) REFERENCES `SocioEconomicStudy` (`Id`),
  KEY `FK_FamilyResearch_District` (`DistrictId`),
  CONSTRAINT `FK_FamilyResearch_District` FOREIGN KEY (`DistrictId`) REFERENCES `District` (`Id`),
  KEY `FK_FamilyResearch_EconomicSituation` (`EconomicSituationId`),
  CONSTRAINT `FK_FamilyResearch_EconomicSituation` FOREIGN KEY (`EconomicSituationId`) REFERENCES `EconomicSituation` (`Id`),
  KEY `FK_FamilyResearch_FamilyNutrition` (`FamilyNutritionId`),
  CONSTRAINT `FK_FamilyResearch_FamilyNutrition` FOREIGN KEY (`FamilyNutritionId`) REFERENCES `FamilyNutrition` (`Id`),
  KEY `FK_FamilyResearch_BenefitsProvided` (`BenefitsProvidedId`),
  CONSTRAINT `FK_FamilyResearch_BenefitsProvided` FOREIGN KEY (`BenefitsProvidedId`) REFERENCES `BenefitsProvided` (`Id`),
  KEY `FK_FamilyResearch_IngresosEgresosMensuales` (`IngresosEgresosMensualesId`),
  CONSTRAINT `FK_FamilyResearch_IngresosEgresosMensuales` FOREIGN KEY (`IngresosEgresosMensualesId`) REFERENCES `IngresosEgresosMensuales` (`Id`)
) ENGINE=InnoDB COMMENT='Investigacion Familiar no tengo la traducción correcta al ingles para algunas columnas';

--
-- Routines for database 'naandi'
--

DROP PROCEDURE IF EXISTS `AddOrUpdateRegistrationRequest`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateRegistrationRequest`(
	IN JSONData LONGTEXT,
	OUT ErrorMessage VARCHAR(2000)
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
   
	SET autocommit = 0;
	
	SELECT JSON_EXTRACT(JSONData, '$.Id') INTO RegistrationRequestId;
 
	START TRANSACTION;
	
	IF RegistrationRequestId = 0 THEN
		
		
		INSERT INTO Minor (FullName, DateOfBirth, PlaceOfBirth, Age, Education, CurrentOccupation)
		SELECT
			JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.FullName'))
			,CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.DateOfBirth')) AS datetime)
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.PlaceOfBirth'))
			,JSON_EXTRACT(JSONData, '$.Minor.Age')
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.CurrentOccupation'));
		
        SET MinorId = LAST_INSERT_ID();
					
        SELECT CASE WHEN JSON_EXTRACT(JSONData, ' $.Requestor.Job.Address.Street') IS NOT NULL THEN 1 ELSE 0 END INTO ExistentPath;
		
		IF ExistentPath = 1 THEN
			INSERT INTO Address (Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference )
			SELECT
				 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Address.Street'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.HouseNumber'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PoBox'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PhoneNumber'))
				,IFNULL(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.City')), '')
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Zip'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.State'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Neighborhood'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Reference'));
			
            SET JobAddressId = LAST_INSERT_ID();
		ELSE
			SET JobAddressId = NULL;
		END IF;
		
        
        SELECT CASE WHEN JSON_EXTRACT(JSONData, ' $.Requestor.Job.Location') IS NOT NULL 
					OR JSON_EXTRACT(JSONData, ' $.Requestor.Job.JobTitle') IS NOT NULL THEN 1 ELSE 0 END INTO ExistentPath;
		
		IF ExistentPath = 1 THEN
			INSERT INTO Job(Location ,JobTitle ,OfficialHours ,YearsOfService ,Salary ,AddressId ,ManagerName ,ManagerPosition)
			SELECT
				 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Location'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.JobTitle'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.OfficialHours'))
				,JSON_EXTRACT(JSONData, '$.Requestor.Job.YearsOfService')
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Salary'))
				,JobAddressId
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerName'))
				,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerPosition'));
			
            SET JobId = LAST_INSERT_ID();
		ELSE
			SET JobId = NULL;
		END IF;
		
				
		
		INSERT INTO Address(Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Address.Street'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.HouseNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.PoBox'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.PhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.City'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Zip'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.State'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Neighborhood'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Reference'));
            
		SET AddressId = LAST_INSERT_ID();
		
		INSERT INTO Requestor
		(FullName ,Age ,DateOfBirth ,PlaceOfBirth ,MaritalStatusId ,Education ,CurrentOccupation ,RelationshipId ,AddressId ,JobId)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.FullName'))
			,JSON_EXTRACT(JSONData, '$.Requestor.Age')
			,CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.DateOfBirth')) AS datetime)
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.PlaceOfBirth'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.MaritalStatusId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.RelationshipId'))
			,AddressId
			,JobId;
		
        SET RequestorId = LAST_INSERT_ID();
		
		INSERT INTO RegistrationRequest
		(HowYouHearAboutUs, CreationDate, RequestorId, MinorId, Reasons, FamilyComposition, FamilyInteraction, EconomicSituation
		,SituationsOfDomesticViolence, FamilyHealthStatus, Comments, RegistrationRequestStatusId, SocialWorkerName)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.HowYouHearAboutUs'))
			,UTC_TIMESTAMP()
			,RequestorId
			,MinorId
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Reasons'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyComposition'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyInteraction'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EconomicSituation'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SituationsOfDomesticViolence'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealthStatus'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'))
            ,JSON_EXTRACT(JSONData, '$.RegistrationRequestStatusId')
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SocialWorkerName'));
	
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
				FullName =           (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.FullName')))
				,DateOfBirth =       (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.DateOfBirth')) AS datetime)) 
				,PlaceOfBirth =      (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.PlaceOfBirth')))
				,Age =               (SELECT JSON_EXTRACT(JSONData, '$.Minor.Age'))
				,Education =         (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.Education')))
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Minor.CurrentOccupation')))
			WHERE Id = MinorId;
			
			
			IF JobAddressId = 0 THEN
					
                SELECT CASE WHEN JSON_EXTRACT(Data, ' $.Requestor.Job.Address.Street') IS NOT NULL THEN 1 ELSE 0 END INTO ExistentPath;
		
				IF ExistentPath = 1 THEN
					INSERT INTO Address (Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference )
					SELECT
						 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Address.Street'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.HouseNumber'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PoBox'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PhoneNumber'))
						,IFNULL(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.City')), '')
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Zip'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.State'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Neighborhood'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Reference'));
					
                    SET JobAddressId = LAST_INSERT_ID();
				END IF;
			ELSE
				UPDATE Address
				SET
					Street = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Address.Street')))
					,HouseNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.HouseNumber')))
					,PoBox = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PoBox')))
					,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.PhoneNumber')))
					,City = (SELECT IFNULL(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.City')), ''))
					,ZIP = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Zip')))
					,State = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.State')))
					,Neighborhood = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Neighborhood')))
					,Reference = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Address.Reference')))
				WHERE Id = JobAddressId;
			END IF;
			
			
			IF JobId = 0 THEN
				               
                SELECT CASE WHEN JSON_EXTRACT(JSONData, ' $.Requestor.Job.Location') IS NOT NULL 
				OR JSON_EXTRACT(JSONData, ' $.Requestor.Job.JobTitle') IS NOT NULL THEN 1 ELSE 0 END INTO ExistentPath;
		
				IF ExistentPath = 1 THEN
					INSERT INTO Job (Location ,JobTitle ,OfficialHours ,YearsOfService ,Salary ,AddressId ,ManagerName ,ManagerPosition)
					SELECT
						 JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Location'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.JobTitle'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.OfficialHours'))
						,JSON_EXTRACT(JSONData, '$.Requestor.Job.YearsOfService')
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Salary'))
						,JobAddressId
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerName'))
						,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerPosition'));
					
                    SET JobId = LAST_INSERT_ID();
				END IF;
			ELSE
				UPDATE Job
					SET
					Location = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Job.Location')))
					,JobTitle = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.JobTitle')))
					,OfficialHours = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.OfficialHours')))
					,YearsOfService = (SELECT JSON_EXTRACT(JSONData, '$.Requestor.Job.YearsOfService'))
					,Salary = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.Salary')))
					,ManagerName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerName')))
					,ManagerPosition = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Job.ManagerPosition')))
				WHERE Id = JobId;
			END IF;
						
			
			UPDATE Address
			SET
				Street = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.Address.Street')))
				,HouseNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.HouseNumber')))
				,PoBox =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.PoBox')))
				,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.PhoneNumber')))
				,City = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.City')))
				,ZIP = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Zip')))
				,State = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.State')))
				,Neighborhood = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Neighborhood')))
				,Reference = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Address.Reference')))
			WHERE Id = AddressId;
			
			
			UPDATE Requestor
			SET
				FullName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Requestor.FullName')))
				,Age = (SELECT JSON_EXTRACT(JSONData, '$.Requestor.Age'))
				,DateOfBirth = (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.DateOfBirth')) as datetime))
				,PlaceOfBirth = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.PlaceOfBirth')))
				,MaritalStatusId = (SELECT JSON_EXTRACT(JSONData, '$.Requestor.MaritalStatusId'))
				,Education = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.Education')))
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Requestor.CurrentOccupation')))
				,RelationshipId = (SELECT JSON_EXTRACT(JSONData, '$.Requestor.RelationshipId'))
			WHERE Id = RequestorId;
			
			
			UPDATE RegistrationRequest
			SET
				HowYouHearAboutUs = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.HowYouHearAboutUs')))
				,Reasons = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Reasons')))
				,FamilyComposition = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyComposition')))
				,FamilyInteraction = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyInteraction')))
				,EconomicSituation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EconomicSituation')))
				,SituationsOfDomesticViolence = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SituationsOfDomesticViolence')))
				,FamilyHealthStatus = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealthStatus')))
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))
                ,RegistrationRequestStatusId = (SELECT JSON_EXTRACT(JSONData, '$.RegistrationRequestStatusId'))
				,SocialWorkerName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SocialWorkerName')))
			WHERE Id = RegistrationRequestId;
	
		END IF;
	END IF;
	COMMIT; 
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `GenCSharpModel`;

DELIMITER ;;
CREATE PROCEDURE `GenCSharpModel`(in pTableName VARCHAR(255) )
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
    SELECT ColumnName1 as col1,
		   ColumnName2 as col2
    FROM
    (SELECT SUBSTRING_INDEX(pTableName, '_', -1) as ColumnName2,
        SUBSTRING_INDEX(pTableName, '_', 1) as ColumnName1) A) B;

    
    CREATE TEMPORARY TABLE IF NOT EXISTS  temp1 ENGINE=InnoDB  
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
    select ColumnName1 as col1,
           ColumnName2 as col2, DATA_TYPE
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
CREATE PROCEDURE `test_mysql_while_loop`()
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


DROP PROCEDURE IF EXISTS `AddOrUpdateAddress`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateAddress`(
	IN  JSONData LONGTEXT,
    OUT AddressId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT JSON_EXTRACT(JSONData, '$.Id') INTO AddressId;
 	
	IF AddressId = 0 THEN							
		
		INSERT INTO Address(Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Street'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PoBox'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.City'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Zip'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.State'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Neighborhood'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Reference'));
		SET AddressId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Address WHERE Id = AddressId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Address not found';
		ELSE			
						
			UPDATE Address
			SET
				 `Street` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Street')))
				,`HouseNumber` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseNumber')))
				,`PoBox` =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PoBox')))
				,`PhoneNumber` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PhoneNumber')))
				,`City` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.City')))
				,`ZIP` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Zip')))
				,`State` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.State')))
				,`Neighborhood` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Neighborhood')))
				,`Reference` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Reference')))
			WHERE Id = AddressId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateLegalGuardian`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateLegalGuardian`(
	IN  JSONData LONGTEXT,
	IN  SpouseId INT,
	IN  AddressId INT,
    OUT LegalGuardianId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    		
	SELECT JSON_EXTRACT(JSONData, '$.Id') INTO LegalGuardianId;
 	
	IF LegalGuardianId = 0 THEN							
		
		INSERT INTO LegalGuardian(`FullName` 
			,`Age` 
			,`PlaceOfBirth` 
			,`MaritalStatusId` 
			,`Education` 
			,`CurrentOccupation`  
			,`RelationshipId` 
			,`AddressId` 
			,`CellPhoneNumber`
			,`PhoneNumber`
			,`Errand`
			,`SpouseId`
			,`DateOfBirth`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FullName'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Age'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PlaceOfBirth'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MaritalStatusId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RelationshipId'))
			,AddressId
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CellPhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PhoneNumber'))
      		,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Errand'))
			,SpouseId
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DateOfBirth'));
		SET LegalGuardianId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM LegalGuardian WHERE Id = LegalGuardianId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'LegalGuardian not found';
		ELSE			
						
			UPDATE LegalGuardian
			SET
				 `FullName` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.FullName')))
				,`Age` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Age')))
				,`PlaceOfBirth` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PlaceOfBirth')))
				,`MaritalStatusId` =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MaritalStatusId')))
				,`Education` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Education')))
				,`CurrentOccupation` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation')))
				,`RelationshipId` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RelationshipId')))
				,`AddressId` = AddressId
				,`CellPhoneNumber` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CellPhoneNumber')))
				,`PhoneNumber`= (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PhoneNumber')))
                ,`Errand` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Errand')))
				,`SpouseId` = SpouseId
				,`DateOfBirth` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DateOfBirth')))
			WHERE Id = LegalGuardianId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateSpouse`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateSpouse`(
	IN  JSONData LONGTEXT,
    OUT SpouseId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;    
	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO SpouseId;
 
	
	IF SpouseId = 0 THEN							
		
		INSERT INTO Spouse(`FullName` ,`Age` ,`CurrentOccupation` ,`Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FullName'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Age'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'));
		SET SpouseId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Spouse WHERE Id = SpouseId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Spouse not found';
		ELSE			
						
			UPDATE Spouse
			SET
				 `FullName` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.FullName')))
				,`Age` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Age')))
				,`CurrentOccupation` =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation')))
				,`Comments` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))
			WHERE Id = SpouseId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateMinor`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateMinor`(
	IN  JSONData LONGTEXT,
	IN  FormalEducationId INT,
    OUT MinorId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO MinorId;
 
	
	IF MinorId = 0 THEN							
		
		INSERT INTO Minor (`FullName`, `DateOfBirth`, `PlaceOfBirth`, `Age`, `Education`, `CurrentOccupation` ,`FormalEducationId`)
		SELECT
			JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FullName'))
			,CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DateOfBirth')) AS datetime)
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PlaceOfBirth'))
			,JSON_EXTRACT(JSONData, '$.Age')
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation'))
			,FormalEducationId;
		SET MinorId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Minor WHERE Id = MinorId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Minor not found';
		ELSE			
						
			UPDATE Minor
			SET
				 `FullName` =           (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FullName')))
				,`DateOfBirth` =       (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DateOfBirth')) AS datetime)) 
				,`PlaceOfBirth` =      (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PlaceOfBirth')))
				,`Age` =               (SELECT JSON_EXTRACT(JSONData, '$.Age'))
				,`Education` =         (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Education')))
				,`CurrentOccupation` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentOccupation')))
				,`FormalEducationId` =  FormalEducationId
			WHERE Id = MinorId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFormalEducation`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFormalEducation`(
	IN  JSONData LONGTEXT,
    OUT FormalEducationId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO FormalEducationId;
 
	
	IF FormalEducationId = 0 THEN							
		
		INSERT INTO FormalEducation (CanItRead, CanItWrite, IsItStudyingNow, CurrentGrade, ReasonsToStopStudying)
		SELECT
			 CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CanItRead'))) = 'TRUE' THEN 1 ELSE 0 END
			,CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CanItWrite'))) = 'TRUE' THEN 1 ELSE 0 END
			,CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.IsItStudyingNow'))) = 'TRUE' THEN 1 ELSE 0 END
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentGrade'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ReasonsToStopStudying'));
		SET FormalEducationId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FormalEducation WHERE Id = FormalEducationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FormalEducation not found';
		ELSE			
						
			UPDATE FormalEducation
			SET
				 CanItRead = 		(SELECT CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CanItRead'))) = 'TRUE' THEN 1 ELSE 0 END)
				,CanItWrite = 		(SELECT CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CanItWrite'))) = 'TRUE' THEN 1 ELSE 0 END)
				,IsItStudyingNow = 	(SELECT CASE WHEN UPPER(JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.IsItStudyingNow'))) = 'TRUE' THEN 1 ELSE 0 END)
				,CurrentGrade =   	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CurrentGrade')))
				,ReasonsToStopStudying = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ReasonsToStopStudying')))
			WHERE Id = FormalEducationId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdatePreviousFoundation`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdatePreviousFoundation`(
	IN  JSONData LONGTEXT,
    OUT PreviousFoundationId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO PreviousFoundationId;
	
	IF PreviousFoundationId = 0 THEN							
		
		INSERT INTO PreviousFoundation(`Familiar` ,`Procuraduria` ,`Dif` ,`Otro`,`InstitucionAnterior`,`TiempoDeEstadia`,`MotivoDeEgreso`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Familiar'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Procuraduria'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Dif'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Otro'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.InstitucionAnterior'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TiempoDeEstadia'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MotivoDeEgreso'));
		SET PreviousFoundationId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM PreviousFoundation WHERE Id = PreviousFoundationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'PreviousFoundation not found';
		ELSE			
						
			UPDATE PreviousFoundation
			SET
				Familiar = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.Familiar')))
				,Procuraduria = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Procuraduria')))
				,Dif =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Dif')))
				,Otro = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Otro')))
				,InstitucionAnterior = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.InstitucionAnterior')))
				,TiempoDeEstadia = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TiempoDeEstadia')))
				,MotivoDeEgreso = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MotivoDeEgreso')))
			WHERE Id = PreviousFoundationId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyHealth`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFamilyHealth`(
	IN  JSONData LONGTEXT,
    OUT FamilyHealthId INT,
	OUT ErrorMessage VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO FamilyHealthId;
 
	
	IF FamilyHealthId = 0 THEN							
		
		INSERT INTO FamilyHealth
		(`FamilyHealthStatus` ,`DerechoHambienteAServiciosDeSalud` ,`Tipo` ,`EnfermedadesCronicasDegenerativas`,`ConsumoDeTabaco`,`ConsumoDeAlcohol`,`ConsumoDeDrogas`, `Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealthStatus'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DerechoHambienteAServiciosDeSalud'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Tipo'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EnfermedadesCronicasDegenerativas'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeTabaco'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeAlcohol'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeDrogas'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'));
		SET FamilyHealthId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FamilyHealth WHERE Id = FamilyHealthId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyHealth not found';
		ELSE			
						
			UPDATE FamilyHealth
			SET
				FamilyHealthStatus = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.FamilyHealthStatus')))
				,DerechoHambienteAServiciosDeSalud = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealth.DerechoHambienteAServiciosDeSalud')))
				,Tipo =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Tipo')))
				,EnfermedadesCronicasDegenerativas = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EnfermedadesCronicasDegenerativas')))
				,ConsumoDeTabaco = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeTabaco')))
				,ConsumoDeAlcohol = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeAlcohol')))
				,ConsumoDeDrogas = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ConsumoDeDrogas')))
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))
			WHERE Id = FamilyHealthId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyMembersDetails`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFamilyMembersDetails`(
	IN  FamilyMembersId INT,
    IN  ArrayItem BLOB
)
BEGIN

	DECLARE Data LONGTEXT DEFAULT CAST(ArrayItem as CHAR CHARACTER SET utf8mb4);
		
	INSERT INTO FamilyMembersDetails (`FullName` ,`Age`,`MaritalStatusId`,`RelationshipId`,`Education`,`CurrentOccupation`,`FamilyMembersId`)
	SELECT
		 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FullName'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Age'))			
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.MaritalStatusId'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.RelationshipId'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Education'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.CurrentOccupation'))
		,FamilyMembersId;	

END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyMembers`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFamilyMembers`(
	IN  `JSONData` LONGTEXT,
    OUT `FamilyMembersId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO FamilyMembersId;
 
	IF FamilyMembersId = 0 THEN							
		
		INSERT INTO FamilyMembers (`FamilyInteraction` ,`Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyInteraction'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'));
		SET FamilyMembersId = LAST_INSERT_ID();

		CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.FamilyMembersDetails')), FamilyMembersId, 'AddOrUpdateFamilyMembersDetails');
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FamilyMembers WHERE Id = FamilyMembersId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyMembers not found';
		ELSE			
						
			UPDATE FamilyMembers
			SET
				FamilyInteraction = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, ' $.FamilyInteraction')))
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))				
			WHERE Id = FamilyMembersId;

			DELETE FROM FamilyMembersDetails  WHERE `FamilyMembersId` = FamilyMembersId;

			CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.FamilyMembersDetails')), FamilyMembersId, 'AddOrUpdateFamilyMembersDetails');
			
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateSocioEconomicStudy`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateSocioEconomicStudy`(
	  IN  `JSONData` LONGTEXT,
      OUT `SocioEconomicStudyId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;
	DEClARE HouseLayoutId INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO SocioEconomicStudyId;
 
	
	IF SocioEconomicStudyId = 0 THEN						

		INSERT INTO HouseLayout (`Bedroom` ,`Dinningroom`,`Kitchen`,`Livingroom`,`Bathroom`,`Patio`,`Garage`,`Backyard`,`Other`,`Ground`,`Walls`,`Roof`,`Description`,`TipoDeMobiliarioId`,`CharacteristicsOfFurniture`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Bedroom'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Dinningroom'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Kitchen'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Livingroom'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Bathroom'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Patio'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Garage'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Backyard'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Other'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Ground'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Walls'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Roof'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Description'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.TipoDeMobiliarioId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.CharacteristicsOfFurniture'));
		SET HouseLayoutId = LAST_INSERT_ID();
		
		
		INSERT INTO SocioEconomicStudy (`HomeAcquisitionId` ,`NombrePropietario`, `MedioAdquisicion`, `TypesOfHousesId`, `HouseLayoutId`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HomeAcquisitionId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.NombrePropietario'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MedioAdquisicion'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TypesOfHousesId'))
			,HouseLayoutId;
		SET SocioEconomicStudyId = LAST_INSERT_ID();

	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM SocioEconomicStudy WHERE Id = SocioEconomicStudyId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'SocioEconomicStudy not found';
		ELSE

			SELECT hl.Id INTO HouseLayoutId
			FROM SocioEconomicStudy ss
			JOIN HouseLayout hl on ss.HouseLayoutId = hl.Id
			WHERE ss.Id = SocioEconomicStudyId;			
						
			UPDATE SocioEconomicStudy
				SET
				 HomeAcquisitionId =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HomeAcquisitionId')))
				,NombrePropietario =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.NombrePropietario')))
				,MedioAdquisicion =   (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MedioAdquisicion')))
				,TypesOfHousesId =    (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TypesOfHousesId')))
			WHERE Id = SocioEconomicStudyId;

			UPDATE HouseLayout
			SET
				Bedroom =          				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Bedroom')))
				,Dinningroom = 					(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Dinningroom')))
				,Kitchen =  					(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Kitchen')))
				,Livingroom =   				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Livingroom')))
				,Bathroom =   				    (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Bathroom')))
				,Patio = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Patio')))
				,Garage =  						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Garage')))
				,Backyard =   					(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Backyard')))
				,Other = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Other')))
				,Ground =  						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Ground')))
				,Walls =   						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Walls')))
				,Roof = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Roof')))
				,Description =  				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.Description')))
				,TipoDeMobiliarioId =          	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.TipoDeMobiliarioId')))
				,CharacteristicsOfFurniture =  	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.HouseLayout.CharacteristicsOfFurniture')))
			WHERE Id = HouseLayoutId;
		
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateDistrict`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateDistrict`(
	  IN  `JSONData` LONGTEXT,
      OUT `DistrictId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO DistrictId;
 
	
	IF DistrictId = 0 THEN						
		
		INSERT INTO District (`TypeOfDistrictId` ,`AguaPotable`, `Telefono`, `Electricidad`, `Drenaje`,`Hospital`,`Correo`,`Escuela`,`Policia`,`AlumbradoPublico`,`ViasDeAcceso`,`TransportePublico`,`AseoPublico`,`Iglesia`,`Mercado`,`Otros`,`Description`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TypeOfDistrictId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AguaPotable'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Telefono'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Electricidad'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Drenaje'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Hospital'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Correo'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Escuela'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Policia'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AlumbradoPublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ViasDeAcceso'))			
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TransportePublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AseoPublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Iglesia'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Mercado'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Otros'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Description'));
		SET DistrictId = LAST_INSERT_ID();

	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM District WHERE Id = DistrictId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'District not found';
		ELSE		
						
			UPDATE District
				SET
				 TypeOfDistrictId = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TypeOfDistrictId')))
				,AguaPotable =  		(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AguaPotable')))
				,Telefono = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Telefono')))
				,Electricidad = 		(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Electricidad')))
				,Drenaje =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Drenaje')))
				,Hospital = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Hospital')))
				,Correo = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Correo')))
				,Escuela =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Escuela')))
				,Policia = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Policia')))
				,AlumbradoPublico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AlumbradoPublico')))
				,ViasDeAcceso =  		(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ViasDeAcceso')))
				,TransportePublico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.TransportePublico')))
				,AseoPublico = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.AseoPublico')))
				,Iglesia =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Iglesia')))
				,Mercado =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Mercado')))
				,Otros = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Otros')))
				,Description = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Description')))				
			WHERE Id = DistrictId;
			
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddEconomicSituationPatrimonyRelation`;

DELIMITER ;;
CREATE PROCEDURE `AddEconomicSituationPatrimonyRelation`(
	In  EconomicSituationId INT,
    IN  ArrayItem BLOB
)
BEGIN

	DECLARE Data LONGTEXT DEFAULT CAST(ArrayItem as CHAR CHARACTER SET utf8mb4);
		
	INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
	SELECT
		 EconomicSituationId
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PatrimonyId'))			
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Value'));
		
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateEconomicSituation`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateEconomicSituation`(
	  IN  `JSONData` LONGTEXT,
      OUT `EconomicSituationId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO EconomicSituationId;
 
	
	IF EconomicSituationId = 0 THEN						
		
		INSERT INTO EconomicSituation (`NivelSocioEconomico`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.NivelSocioEconomico'));
		SET EconomicSituationId = LAST_INSERT_ID();

		CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.EconomicSituationPatrimonyRelation')), EconomicSituationId, 'AddEconomicSituationPatrimonyRelation');
		
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM EconomicSituation WHERE Id = EconomicSituationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'EconomicSituation not found';
		ELSE		
						
			UPDATE EconomicSituation
				SET
				NivelSocioEconomico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.NivelSocioEconomico')))							
			WHERE Id = EconomicSituationId;
			
			DELETE FROM EconomicSituationPatrimonyRelation  WHERE `EconomicSituationId` = EconomicSituationId;

			CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.EconomicSituationPatrimonyRelation')), EconomicSituationId, 'AddEconomicSituationPatrimonyRelation');
				
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `foreach_array_item`;

DELIMITER ;;
CREATE PROCEDURE `foreach_array_item`(
        in_array LONGTEXT,
        in_id INT,
        in_callback VARCHAR(100)
    )
    MODIFIES SQL DATA
    COMMENT 'Iterate an array and for each item invoke a callback procedure'
BEGIN
    DECLARE i INT UNSIGNED
        DEFAULT 0;
    DECLARE v_count INT UNSIGNED
        DEFAULT JSON_LENGTH(in_array);
    DECLARE v_current_item BLOB
        DEFAULT NULL;

    -- loop from 0 to the last item
    WHILE i < v_count DO
        -- get the current item and build an SQL statement
        -- to pass it to a callback procedure
        SET v_current_item :=
            JSON_QUOTE(JSON_EXTRACT(in_array, CONCAT('$[', i, ']')));
        SET @sql_array_callback :=
            CONCAT('CALL ', in_callback, '(', in_id, ', ', v_current_item, ');');
        
        PREPARE stmt_array_callback FROM @sql_array_callback;
        EXECUTE stmt_array_callback;
        DEALLOCATE PREPARE stmt_array_callback;

        SET i := i + 1;
    END WHILE;
END;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddFamilyNutritionFoodRelation`;

DELIMITER ;;
CREATE PROCEDURE `AddFamilyNutritionFoodRelation`(
	IN  FamilyNutritionId INT,
    IN  ArrayItem BLOB
)
BEGIN

  	DECLARE Data LONGTEXT DEFAULT CAST(ArrayItem as CHAR CHARACTER SET utf8mb4);
		
	INSERT INTO FamilyNutritionFoodRelation (`FamilyNutritionId`, `FoodId`, `FrequencyId`)
	SELECT FamilyNutritionId
	,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FoodId')))
	,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FrequencyId')));		

END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyNutrition`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFamilyNutrition`(
	  IN   JSONData LONGTEXT,
      OUT  FamilyNutritionId INT,
	  OUT  ErrorMessage VARCHAR(2000)
)
BEGIN

	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE,
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;

	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO FamilyNutritionId;

	IF FamilyNutritionId = 0 THEN

		INSERT INTO FamilyNutrition (`Comments`, `FoodAllergy`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'))
       		,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FoodAllergy'));
		SET FamilyNutritionId = LAST_INSERT_ID();

		CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.FamilyNutritionFoodRelation')), FamilyNutritionId, 'AddFamilyNutritionFoodRelation');

	ELSE

		SELECT  EXISTS(SELECT 1 FROM FamilyNutrition WHERE Id = FamilyNutritionId) INTO rowExists;

		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyNutrition not found';
		ELSE

			UPDATE FamilyNutrition
				SET
				`Comments` =		(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))
				,`FoodAllergy` =	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FoodAllergy')))
			WHERE Id = FamilyNutritionId;

			DELETE FROM FamilyNutritionFoodRelation WHERE `FamilyNutritionId` = FamilyNutritionId;

			CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.FamilyNutritionFoodRelation')), FamilyNutritionId, 'AddFamilyNutritionFoodRelation');

		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateBenefitsProvidedDetails`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateBenefitsProvidedDetails`(
	IN  BenefitsProvidedId INT,
    IN  ArrayItem BLOB
)
BEGIN

	DECLARE Data LONGTEXT DEFAULT CAST(ArrayItem as CHAR CHARACTER SET utf8mb4);
		
	INSERT INTO BenefitsProvidedDetails (`Institucion` ,`ApoyoRecibido`,`Monto`,`Periodo`,`BenefitsProvidedId`)
	SELECT
		 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Institucion'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.ApoyoRecibido'))			
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Monto'))
		,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Periodo'))
		,BenefitsProvidedId;	

END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateBenefitsProvided`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateBenefitsProvided`(
	IN  `JSONData` LONGTEXT,
    OUT `BenefitsProvidedId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO BenefitsProvidedId;
 
	IF BenefitsProvidedId = 0 THEN							
		
		INSERT INTO BenefitsProvided (`CreationDate`)
		SELECT UTC_TIMESTAMP();
		SET BenefitsProvidedId = LAST_INSERT_ID();

		CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.BenefitsProvidedDetails')), BenefitsProvidedId, 'AddOrUpdateBenefitsProvidedDetails');
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM BenefitsProvided WHERE Id = BenefitsProvidedId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'BenefitsProvided not found';
		ELSE			
						
			DELETE FROM BenefitsProvidedDetails  WHERE `BenefitsProvidedId` = BenefitsProvidedId;

			CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.BenefitsProvidedDetails')), BenefitsProvidedId, 'AddOrUpdateBenefitsProvidedDetails');
			
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddIngresosEgresosMensualesMovimientoRelation`;

DELIMITER ;;
CREATE PROCEDURE `AddIngresosEgresosMensualesMovimientoRelation`(
	In  IngresosEgresosMensualesId INT,
    IN  ArrayItem BLOB
)
BEGIN

    DECLARE Data LONGTEXT DEFAULT CAST(ArrayItem as CHAR CHARACTER SET utf8mb4);
		
	INSERT INTO IngresosEgresosMensualesMovimientoRelation (`IngresosEgresosMensualesId`, `MovimientoId`, `Monto`)
	SELECT IngresosEgresosMensualesId
	,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.MovimientoId')))
	,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Monto')));		

END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateIngresosEgresosMensuales`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateIngresosEgresosMensuales`(
	IN  `JSONData` LONGTEXT,
    OUT `IngresosEgresosMensualesId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN

	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE,
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;

	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO IngresosEgresosMensualesId;

	IF IngresosEgresosMensualesId = 0 THEN

		INSERT INTO IngresosEgresosMensuales (`Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments'));
		SET IngresosEgresosMensualesId = LAST_INSERT_ID();

		CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.IngresosEgresosMensualesMovimientoRelation')), IngresosEgresosMensualesId, 'AddIngresosEgresosMensualesMovimientoRelation');

	ELSE

		SELECT  EXISTS(SELECT 1 FROM IngresosEgresosMensuales WHERE Id = IngresosEgresosMensualesId) INTO rowExists;

		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'IngresosEgresosMensuales not found';
		ELSE

			UPDATE IngresosEgresosMensuales
				SET
				`Comments` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Comments')))
			WHERE Id = IngresosEgresosMensualesId;

			DELETE FROM IngresosEgresosMensualesMovimientoRelation WHERE `IngresosEgresosMensualesId` = IngresosEgresosMensualesId;

			CALL `foreach_array_item`((SELECT JSON_EXTRACT(JSONData, '$.IngresosEgresosMensualesMovimientoRelation')), IngresosEgresosMensualesId, 'AddIngresosEgresosMensualesMovimientoRelation');

		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyResearch`;

DELIMITER ;;
CREATE PROCEDURE `AddOrUpdateFamilyResearch`(
	IN `JSONData` LONGTEXT,
	OUT `FamilyResearchId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    	
	SELECT
	JSON_EXTRACT(JSONData, '$.Id') INTO FamilyResearchId;
 
	
	IF FamilyResearchId = 0 THEN							
		
		INSERT INTO FamilyResearch
		(
			VisitDate 
			,VisitTime 
			,Family 
			,RequestReasons
			,RedesDeApoyoFamiliares 
			,SituationsOfDomesticViolence 
			,FamilyExpectations 
			,FamilyDiagnostic
			,ProblemsIdentified
			,CaseStudyConclusion 
			,Recommendations
			,VisualSupports
			,Sketch
			,LegalGuardianId
			,MinorId
			,PreviousFoundationId
			,FamilyHealthId
			,FamilyMembersId
			,SocioeconomicStudyId
			,DistrictId
			,EconomicSituationId
			,FamilyNutritionId
			,BenefitsProvidedId
			,IngresosEgresosMensualesId
		)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisitDate'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisitTime'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Family'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RequestReasons'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RedesDeApoyoFamiliares'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SituationsOfDomesticViolence'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyExpectations'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyDiagnostic'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ProblemsIdentified'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CaseStudyConclusion'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Recommendations'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisualSupports'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Sketch'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.LegalGuardianId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MinorId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PreviousFoundationId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealthId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyMembersId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SocioEconomicStudyId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DistrictId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EconomicSituationId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyNutritionId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.BenefitsProvidedId'))
			,JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.IngresosEgresosMensualesId'));
		SET FamilyResearchId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FamilyResearch WHERE Id = FamilyResearchId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyResearch not found';
		ELSE			
						
			UPDATE FamilyResearch
			SET
				 `VisitDate` =                     	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisitDate')))
				,`VisitTime` =                     	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisitTime')))
				,`Family` =                        	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Family')))
				,`RequestReasons` =                	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RequestReasons')))
				,`RedesDeApoyoFamiliares` =        	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.RedesDeApoyoFamiliares')))
				,`SituationsOfDomesticViolence` =  	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SituationsOfDomesticViolence')))
				,`FamilyExpectations` =            	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyExpectations')))
				,`FamilyDiagnostic` =              	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyDiagnostic')))
				,`ProblemsIdentified` =            	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.ProblemsIdentified')))
				,`CaseStudyConclusion` =           	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.CaseStudyConclusion')))
				,`Recommendations` =               	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Recommendations')))
				,`VisualSupports` =                	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.VisualSupports')))
				,`Sketch` =                        	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.Sketch')))
				,`LegalGuardianId` = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.LegalGuardianId')))
				,`MinorId` = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.MinorId')))
				,`PreviousFoundationId` = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.PreviousFoundationId')))
				,`FamilyHealthId` = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyHealthId')))
				,`FamilyMembersId` = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyMembersId')))
				,`SocioeconomicStudyId` = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.SocioEconomicStudyId')))
				,`DistrictId` = 					(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.DistrictId')))
				,`EconomicSituationId` = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.EconomicSituationId')))
				,`FamilyNutritionId` = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.FamilyNutritionId')))
				,`BenefitsProvidedId` = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.BenefitsProvidedId')))
				,`IngresosEgresosMensualesId` = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(JSONData, '$.IngresosEgresosMensualesId')))
				
			WHERE Id = FamilyResearchId;
		END IF;
	END IF;
END ;;
DELIMITER ;


--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;

CREATE TABLE `User` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(20) NOT NULL,
  `Password` varchar(50) NOT NULL,
  `Email`    varchar(50) NOT NULL,
  `Active`   tinyint(1)  NOT NULL,
  `CreationDate` datetime,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;

CREATE TABLE `Roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;


--
-- Table structure for table `UserRolesRelation`
--

DROP TABLE IF EXISTS `UserRolesRelation`;

CREATE TABLE `UserRolesRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `RolesId` int(11) NOT NULL,
  `Active`  tinyint(1)  NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserId_RolesId` (`UserId`,`RolesId`),
  KEY `FK_UserRolesRelation_User` (`UserId`),
  CONSTRAINT `FK_UserRolesRelation_User` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`),
  KEY `FK_UserRolesRelation_Roles` (`RolesId`),
  CONSTRAINT `FK_UserRolesRelation_Roles` FOREIGN KEY (`RolesId`) REFERENCES `Roles` (`Id`)
) ENGINE=InnoDB;

--
-- Table structure for table `log`
--

DROP TABLE IF EXISTS `log`;

CREATE TABLE `log` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `Application` varchar(50) DEFAULT NULL,
  `Logged` datetime DEFAULT NULL,
  `Level` varchar(50) DEFAULT NULL,
  `Message` text DEFAULT NULL,
  `Logger` varchar(250) DEFAULT NULL,
  `Callsite` text DEFAULT NULL,
  `Exception` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;



DROP PROCEDURE IF EXISTS `SaveLog`;

DELIMITER ;;
CREATE PROCEDURE `SaveLog`(
    IN Application varchar(50),
    IN Logged datetime,
    IN Level varchar(50),
    IN Message text ,
    IN Logger varchar(250),
    IN Callsite text,
    IN Exception longtext
)
BEGIN

	INSERT INTO log (
			`Application`,
			`Logged`,
			`Level`,
			`Message`,
			`Logger`,
			`Callsite`,
			`Exception`
		)
	VALUES (
		Application,
		Logged,
		Level,
		Message,
		Logger,
		Callsite,
		Exception
	);

END ;;
DELIMITER ;




SET FOREIGN_KEY_CHECKS = 1;