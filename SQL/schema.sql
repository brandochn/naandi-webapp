CREATE DATABASE IF NOT EXISTS `naandi` CHARACTER SET = 'utf8mb4' COLLATE = 'utf8mb4_unicode_ci';
USE `naandi`;


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
  PRIMARY KEY (`Id`),
  KEY `FK_LegalGuardian_MaritalStatus` (`MaritalStatusId`),
  KEY `FK_LegalGuardian_Relationship` (`RelationshipId`),
  KEY `FK_LegalGuardian_Address` (`AddressId`),
  CONSTRAINT `FK_LegalGuardian_Address` FOREIGN KEY (`AddressId`) REFERENCES `Address` (`Id`),
  CONSTRAINT `FK_LegalGuardian_MaritalStatus` FOREIGN KEY (`MaritalStatusId`) REFERENCES `MaritalStatus` (`Id`),
  CONSTRAINT `FK_LegalGuardian_Relationship` FOREIGN KEY (`RelationshipId`) REFERENCES `Relationship` (`Id`)
) ENGINE=InnoDB COMMENT='Tutor legal info';

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
-- Table structure for table `DerivadaPor`
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
  `AguaPotable` varchar(100) NOT NULL,
  `Telefono` varchar(10) NOT NULL,
  `Electricidad` varchar(100) NOT NULL,
  `Drenaje` varchar(100) NOT NULL,
  `Hospital` varchar(100) NOT NULL,
  `Correo` varchar(100) NOT NULL,
  `Escuela` varchar(100) NOT NULL,
  `Policia` varchar(100) NOT NULL,
  `AlumbradoPublico` varchar(100) NOT NULL,
  `ViasDeAcceso` varchar(100) NOT NULL,
  `TransportePublico` varchar(100) NOT NULL,
  `AseoPublico` varchar(100) NOT NULL,
  `Iglesia` varchar(100) NOT NULL,
  `Otros` varchar(100) NOT NULL,
  `Description` varchar(100) NOT NULL,
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
  `Bedroom` varchar(100) NOT NULL,
  `Dinningroom` varchar(100) NOT NULL,
  `Kitchen` varchar(100) NOT NULL,
  `Livingroom` varchar(100) NOT NULL,
  `Patio` varchar(100) NOT NULL,
  `Garage` varchar(100) NOT NULL,
  `Backyard` varchar(100) NOT NULL,
  `Other` varchar(100) NOT NULL,
  `Ground` varchar(100) NOT NULL,
  `Walls` varchar(100) NOT NULL,
  `Roof` varchar(100) NOT NULL,
  `Description` varchar(400) NOT NULL,
  `TipoDeMobiliarioId` INT NULL,
  `CharacteristicsOfFurniture` varchar(400) NOT NULL,
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
  `Value` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `EconomicSituationId_PatrimonyId` (`EconomicSituationId`,`PatrimonyId`),
  KEY `FK_EconomicSituationPatrimonyRelation_Patrimony` (`PatrimonyId`),
  CONSTRAINT `FK_EconomicSituationPatrimonyRelation_Patrimony` FOREIGN KEY (`PatrimonyId`) REFERENCES `Patrimony` (`Id`),
  KEY `FK_EconomicSituationPatrimonyRelation_EconomicSituation` (`EconomicSituationId`),
  CONSTRAINT `FK_EconomicSituationPatrimonyRelation_EconomicSituation` FOREIGN KEY (`EconomicSituationId`) REFERENCES `EconomicSituation` (`Id`)
) ENGINE=InnoDB;


--
-- Table structure for table `SocioeconomicStudy`
--

DROP TABLE IF EXISTS `SocioeconomicStudy`;

CREATE TABLE `SocioeconomicStudy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Vivienda` varchar(100) DEFAULT NULL,
  `NombrePropietario` varchar(100) DEFAULT NULL,
  `MedioAdquisicion` varchar(100) DEFAULT NULL,
  `TypesOfHousesId` int(11) DEFAULT NULL,
  `HouseLayoutId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SocioeconomicStudy_TypesOfHouses` (`TypesOfHousesId`),
  CONSTRAINT `FK_SocioeconomicStudy_TypesOfHouses` FOREIGN KEY (`TypesOfHousesId`) REFERENCES `TypesOfHouses` (`Id`),
  KEY `FK_SocioeconomicStudy_HouseLayout` (`HouseLayoutId`),
  CONSTRAINT `FK_SocioeconomicStudy_HouseLayout` FOREIGN KEY (`HouseLayoutId`) REFERENCES `HouseLayout` (`Id`)
) ENGINE=InnoDB COMMENT='SOCIOECONOMICO no tengo la traducción correcta para algunas columnas';

--
-- Table structure for table `FamilyNutrition`
--

DROP TABLE IF EXISTS `FamilyNutrition`;

CREATE TABLE `FamilyNutrition` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Comments` varchar(400),
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
  `Institucion` varchar(100),
  `ApoyoRecibido` varchar(200),
  `Monto`   decimal(13, 2),
  `Periodo` datetime,
  `RedesDeApoyoFamiliares` varchar(400),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT='Apoyos y Servicios Otorgados no tengo la traducción correcta para algunas columnas';

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
  `Comments` varchar(400) NOT NULL,
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
-- Table structure for table `InvestigacionFamiliar`
--

DROP TABLE IF EXISTS `InvestigacionFamiliar`;

CREATE TABLE `InvestigacionFamiliar` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationDate` datetime NOT NULL,
  `CreationTime` time NOT NULL,
  `Family` varchar(100) NOT NULL,
  `RequestReasons` varchar(300) NOT NULL,
  `SituationsOfDomesticViolence` varchar(200) DEFAULT NULL,
  `FamilyExpectations` varchar(300) DEFAULT NULL,
  `FamilyDiagnostic` varchar(300) DEFAULT NULL,
  `CaseStudyConclusion` varchar(300) DEFAULT NULL,
  `Recommendations` varchar(300) DEFAULT NULL,
  `VisualSupports` varchar(300) DEFAULT NULL,
  `Sketch` varchar(300) DEFAULT NULL,
  `MinorId` int(11) DEFAULT NULL,
  `LegalGuardianId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_InvestigacionFamiliar_Minor` (`MinorId`),
  CONSTRAINT `FK_InvestigacionFamiliar_Minor` FOREIGN KEY (`MinorId`) REFERENCES `Minor` (`Id`),
  KEY `FK_InvestigacionFamiliar_LegalGuardian` (`LegalGuardianId`),
  CONSTRAINT `FK_InvestigacionFamiliar_LegalGuardian` FOREIGN KEY (`LegalGuardianId`) REFERENCES `LegalGuardian` (`Id`)
) ENGINE=InnoDB COMMENT='Investigacion Familiar no tengo la traducción correcta al ingles para esta tabla';

--
-- Table structure for table `FamilyMembersInvestigacionFamiliarRelation`
--

DROP TABLE IF EXISTS `FamilyMembersInvestigacionFamiliarRelation`;

CREATE TABLE `FamilyMembersInvestigacionFamiliarRelation` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyMembersId` int(11) NOT NULL ,
  `InvestigacionFamiliarId`  int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `FamilyMembersId_InvestigacionFamiliarId` (`FamilyMembersId`,`InvestigacionFamiliarId`),
  KEY `FK_FamilyMembersInvesFamRelation_InvestigacionFam` (`InvestigacionFamiliarId`),
  CONSTRAINT `FK_FamilyMembersInvesFamRelation_InvestigacionFam` FOREIGN KEY (`InvestigacionFamiliarId`) REFERENCES `InvestigacionFamiliar` (`Id`),
  KEY `FK_FamilyMembersInvestigacionFamiliarRelation_FamilyMembers` (`FamilyMembersId`),
  CONSTRAINT `FK_FamilyMembersInvestigacionFamiliarRelation_FamilyMembers` FOREIGN KEY (`FamilyMembersId`) REFERENCES `FamilyMembers` (`Id`)
) ENGINE=InnoDB;

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
	SELECT JSONData AS 'Data';
	
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


DROP PROCEDURE IF EXISTS `AddOrUpdateAddress`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateAddress`(
	IN `JSONData` LONGTEXT,
  OUT `AddressId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.Address.Id') INTO AddressId
	FROM JSON_TABLE;
 
	
	IF AddressId = 0 THEN							
		
		INSERT INTO Address(Street ,HouseNumber ,PoBox ,PhoneNumber ,City ,ZIP ,State ,Neighborhood ,Reference)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Street'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.HouseNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.PoBox'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.PhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.City'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Zip'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.State'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Neighborhood'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Reference'))
		FROM JSON_TABLE;
		SET AddressId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Address WHERE Id = AddressId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Address not found';
		ELSE			
						
			UPDATE Address
			SET
				Street = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Address.Street')) FROM JSON_TABLE)
				,HouseNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.HouseNumber')) FROM JSON_TABLE)
				,PoBox =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.PoBox')) FROM JSON_TABLE)
				,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.PhoneNumber')) FROM JSON_TABLE)
				,City = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.City')) FROM JSON_TABLE)
				,ZIP = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Zip')) FROM JSON_TABLE)
				,State = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.State')) FROM JSON_TABLE)
				,Neighborhood = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Neighborhood')) FROM JSON_TABLE)
				,Reference = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Address.Reference'))  FROM JSON_TABLE)
			WHERE Id = AddressId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateLegalGuardian`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateLegalGuardian`(
	IN `JSONData` LONGTEXT,
  OUT `LegalGuardianId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.LegalGuardian.Id') INTO LegalGuardianId
	FROM JSON_TABLE;
 
	
	IF LegalGuardianId = 0 THEN							
		
		INSERT INTO LegalGuardian(`FullName` ,`Age` ,`PlaceOfBirth` ,`MaritalStatusId` ,`Education` ,`CurrentOccupation`  ,`RelationshipId` ,`AddressId` ,`CellPhoneNumber`, `PhoneNumber`, `Errand`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.FullName'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Age'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.MaritalStatusId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.RelationshipId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.AddressId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.CellPhoneNumber'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.PhoneNumber'))
      ,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Errand'))
		FROM JSON_TABLE;
		SET LegalGuardianId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM LegalGuardian WHERE Id = LegalGuardianId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'LegalGuardian not found';
		ELSE			
						
			UPDATE LegalGuardian
			SET
				FullName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.LegalGuardian.FullName')) FROM JSON_TABLE)
				,Age = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Age')) FROM JSON_TABLE)
				,MaritalStatusId =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.MaritalStatusId')) FROM JSON_TABLE)
				,Education = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Education')) FROM JSON_TABLE)
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.CurrentOccupation')) FROM JSON_TABLE)
				,RelationshipId = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.RelationshipId')) FROM JSON_TABLE)
				,AddressId = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.AddressId')) FROM JSON_TABLE)
				,CellPhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.CellPhoneNumber')) FROM JSON_TABLE)
				,PhoneNumber = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.PhoneNumber'))  FROM JSON_TABLE)
        ,Errand = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.LegalGuardian.Errand'))  FROM JSON_TABLE)
			WHERE Id = LegalGuardianId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateSpouse`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateSpouse`(
	IN  `JSONData` LONGTEXT,
    OUT `SpouseId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.Spouse.Id') INTO SpouseId
	FROM JSON_TABLE;
 
	
	IF SpouseId = 0 THEN							
		
		INSERT INTO Spouse(`FullName` ,`Age` ,`CurrentOccupation` ,`Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.FullName'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.Age'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.CurrentOccupation'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.Comments'))
		FROM JSON_TABLE;
		SET SpouseId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Spouse WHERE Id = SpouseId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Spouse not found';
		ELSE			
						
			UPDATE Spouse
			SET
				FullName = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.Spouse.FullName')) FROM JSON_TABLE)
				,Age = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.Age')) FROM JSON_TABLE)
				,CurrentOccupation =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.CurrentOccupation')) FROM JSON_TABLE)
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Spouse.Comments')) FROM JSON_TABLE)
			WHERE Id = SpouseId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateMinor`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateMinor`(
	IN  `JSONData` LONGTEXT,
    OUT `MinorId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.Minor.Id') INTO MinorId
	FROM JSON_TABLE;
 
	
	IF MinorId = 0 THEN							
		
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
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM Minor WHERE Id = MinorId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Minor not found';
		ELSE			
						
			UPDATE Minor
			SET
				FullName =           (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.FullName')) FROM JSON_TABLE)
				,DateOfBirth =       (SELECT CAST(JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.DateOfBirth')) AS datetime) FROM JSON_TABLE) 
				,PlaceOfBirth =      (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.PlaceOfBirth')) FROM JSON_TABLE)
				,Age =               (SELECT JSON_EXTRACT(Data, '$.Minor.Age') FROM JSON_TABLE)
				,Education =         (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.Education')) FROM JSON_TABLE)
				,CurrentOccupation = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.Minor.CurrentOccupation')) FROM JSON_TABLE)
			WHERE Id = MinorId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFormalEducation`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateFormalEducation`(
	IN  `JSONData` LONGTEXT,
    OUT `FormalEducationId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.FormalEducation.Id') INTO FormalEducationId
	FROM JSON_TABLE;
 
	
	IF FormalEducationId = 0 THEN							
		
		INSERT INTO FormalEducation (CanItRead, CanItWrite, IsItStudyingNow, CurrentGrade, ReasonsToStopStudying)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CanItRead'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CanItWrite'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.IsItStudyingNow'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CurrentGrade'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.ReasonsToStopStudying'))
		FROM JSON_TABLE;
		SET FormalEducationId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FormalEducation WHERE Id = FormalEducationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FormalEducation not found';
		ELSE			
						
			UPDATE FormalEducation
			SET
				 CanItRead = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CanItRead')) FROM JSON_TABLE)
				,CanItWrite = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CanItWrite')) FROM JSON_TABLE)
				,IsItStudyingNow = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.IsItStudyingNow')) FROM JSON_TABLE)
				,CurrentGrade =   (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.CurrentGrade')) FROM JSON_TABLE)
				,ReasonsToStopStudying = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FormalEducation.ReasonsToStopStudying')) FROM JSON_TABLE)
			WHERE Id = FormalEducationId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdatePreviousFoundation`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdatePreviousFoundation`(
	IN  `JSONData` LONGTEXT,
    OUT `PreviousFoundationId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.PreviousFoundation.Id') INTO PreviousFoundationId
	FROM JSON_TABLE;
 
	
	IF PreviousFoundationId = 0 THEN							
		
		INSERT INTO PreviousFoundation(`Familiar` ,`Procuraduria` ,`Dif` ,`Otro`,`InstitucionAnterior`,`TiempoDeEstadia`,`MotivoDeEgreso`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Familiar'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Procuraduria'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Dif'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Otro'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.InstitucionAnterior'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.TiempoDeEstadia'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.MotivoDeEgreso'))
		FROM JSON_TABLE;
		SET PreviousFoundationId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM PreviousFoundation WHERE Id = PreviousFoundationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'PreviousFoundation not found';
		ELSE			
						
			UPDATE PreviousFoundation
			SET
				Familiar = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.PreviousFoundation.Familiar')) FROM JSON_TABLE)
				,Procuraduria = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Procuraduria')) FROM JSON_TABLE)
				,Dif =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Dif')) FROM JSON_TABLE)
				,Otro = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.Otro')) FROM JSON_TABLE)
				,InstitucionAnterior = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.InstitucionAnterior')) FROM JSON_TABLE)
				,TiempoDeEstadia = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.TiempoDeEstadia')) FROM JSON_TABLE)
				,MotivoDeEgreso = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.PreviousFoundation.MotivoDeEgreso')) FROM JSON_TABLE)
			WHERE Id = PreviousFoundationId;
		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyHealth`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateFamilyHealth`(
	IN  `JSONData` LONGTEXT,
    OUT `FamilyHealthId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.FamilyHealth.Id') INTO FamilyHealthId
	FROM JSON_TABLE;
 
	
	IF FamilyHealthId = 0 THEN							
		
		INSERT INTO FamilyHealth
		(`FamilyHealthStatus` ,`DerechoHambienteAServiciosDeSalud` ,`Tipo` ,`EnfermedadesCronicasDegenerativas`,`ConsumoDeTabaco`,`ConsumoDeAlcohol`,`ConsumoDeDrogas`, `Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.FamilyHealthStatus'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.DerechoHambienteAServiciosDeSalud'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.Tipo'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.EnfermedadesCronicasDegenerativas'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeTabaco'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeAlcohol'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeDrogas'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.Comments'))
		FROM JSON_TABLE;
		SET FamilyHealthId = LAST_INSERT_ID();
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FamilyHealth WHERE Id = FamilyHealthId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyHealth not found';
		ELSE			
						
			UPDATE FamilyHealth
			SET
				FamilyHealthStatus = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.FamilyHealth.FamilyHealthStatus')) FROM JSON_TABLE)
				,DerechoHambienteAServiciosDeSalud = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.DerechoHambienteAServiciosDeSalud')) FROM JSON_TABLE)
				,Tipo =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.Tipo')) FROM JSON_TABLE)
				,EnfermedadesCronicasDegenerativas = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.EnfermedadesCronicasDegenerativas')) FROM JSON_TABLE)
				,ConsumoDeTabaco = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeTabaco')) FROM JSON_TABLE)
				,ConsumoDeAlcohol = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeAlcohol')) FROM JSON_TABLE)
				,ConsumoDeDrogas = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.ConsumoDeDrogas')) FROM JSON_TABLE)
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyHealth.Comments')) FROM JSON_TABLE)
			WHERE Id = FamilyHealthId;
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateFamilyMembers`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateFamilyMembers`(
	IN  `JSONData` LONGTEXT,
    OUT `FamilyMembersId` INT,
	OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.FamilyMembers.Id') INTO FamilyMembersId
	FROM JSON_TABLE;
 
	
	IF FamilyMembersId = 0 THEN							
		
		INSERT INTO FamilyMembers (`FamilyInteraction` ,`Comments`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyInteraction'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.Comments'))			
		FROM JSON_TABLE;
		SET FamilyMembersId = LAST_INSERT_ID();

		INSERT INTO FamilyMembersDetails (`FullName` ,`Age`,`MaritalStatusId`,`RelationshipId`,`Education`,`CurrentOccupation`,`FamilyMembersId`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.FullName'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.Age'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.MaritalStatusId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.RelationshipId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.Education'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.CurrentOccupation'))
			,FamilyMembersId
		FROM JSON_TABLE;
	
	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM FamilyMembers WHERE Id = FamilyMembersId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'FamilyMembers not found';
		ELSE			
						
			UPDATE FamilyMembers
			SET
				FamilyInteraction = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.FamilyMembers.FamilyInteraction')) FROM JSON_TABLE)
				,Comments = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.Comments')) FROM JSON_TABLE)				
			WHERE Id = FamilyMembersId;

			DELETE FROM FamilyMembersDetails  WHERE `FamilyMembersId` = FamilyMembersId;

			INSERT INTO FamilyMembersDetails (`FullName` ,`Age`,`MaritalStatusId`,`RelationshipId`,`Education`,`CurrentOccupation`,`FamilyMembersId`)
			SELECT
				JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.FullName'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.Age'))			
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.MaritalStatusId'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.RelationshipId'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.Education'))
				,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.FamilyMembers.FamilyMembersDetails.CurrentOccupation'))
				,FamilyMembersId
			FROM JSON_TABLE;

		END IF;
	END IF;
END ;;
DELIMITER ;

DROP PROCEDURE IF EXISTS `AddOrUpdateSocioeconomicStudy`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateSocioeconomicStudy`(
	  IN  `JSONData` LONGTEXT,
      OUT `SocioeconomicStudyId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;
	DEClARE HouseLayoutId INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.SocioeconomicStudy.Id') INTO SocioeconomicStudyId
	FROM JSON_TABLE;
 
	
	IF SocioeconomicStudyId = 0 THEN						

		INSERT INTO HouseLayout (`Bedroom` ,`Dinningroom`,`Kitchen`,`Livingroom`,`Patio`,`Garage`,`Backyard`,`Other`,`Ground`,`Walls`,`Roof`,`Description`,`TipoDeMobiliarioId`,`CharacteristicsOfFurniture`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Bedroom'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Dinningroom'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Kitchen'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Livingroom'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Patio'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Garage'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Backyard'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Other'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Ground'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Walls'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Roof'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Description'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.TipoDeMobiliarioId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.CharacteristicsOfFurniture'))
		FROM JSON_TABLE;
		SET HouseLayoutId = LAST_INSERT_ID();
		
		
		INSERT INTO SocioeconomicStudy (`Vivienda` ,`NombrePropietario`, `MedioAdquisicion`, `TypesOfHousesId`, `HouseLayoutId`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.Vivienda'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.NombrePropietario'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.MedioAdquisicion'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.TypesOfHousesId'))
			,HouseLayoutId
		FROM JSON_TABLE;
		SET SocioeconomicStudyId = LAST_INSERT_ID();

	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM SocioeconomicStudy WHERE Id = SocioeconomicStudyId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'SocioeconomicStudy not found';
		ELSE

			SELECT hl.Id INTO HouseLayoutId
			FROM SocioeconomicStudy ss
			JOIN HouseLayout hl on ss.HouseLayoutId = hl.Id
			WHERE ss.Id = SocioeconomicStudyId;			
						
			UPDATE SocioeconomicStudy
				SET
				 Vivienda = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, ' $.SocioeconomicStudy.Vivienda')) FROM JSON_TABLE)
				,NombrePropietario = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.NombrePropietario')) FROM JSON_TABLE)
				,MedioAdquisicion =  (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.MedioAdquisicion')) FROM JSON_TABLE)
				,TypesOfHousesId = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.TypesOfHousesId')) FROM JSON_TABLE)
			WHERE Id = SocioeconomicStudyId;

			UPDATE HouseLayout
			SET
				Bedroom =          				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Bedroom')) FROM JSON_TABLE)
				,Dinningroom = 					(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Dinningroom')) FROM JSON_TABLE)
				,Kitchen =  					(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Kitchen')) FROM JSON_TABLE)
				,Livingroom =   				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Livingroom')) FROM JSON_TABLE)
				,Patio = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Patio')) FROM JSON_TABLE)
				,Garage =  						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Garage')) FROM JSON_TABLE)
				,Backyard =   					(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Backyard')) FROM JSON_TABLE)
				,Other = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Other')) FROM JSON_TABLE)
				,Ground =  						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Ground')) FROM JSON_TABLE)
				,Walls =   						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Walls')) FROM JSON_TABLE)
				,Roof = 						(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Roof')) FROM JSON_TABLE)
				,Description =  				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.Description')) FROM JSON_TABLE)
				,TipoDeMobiliarioId =          	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.TipoDeMobiliarioId')) FROM JSON_TABLE)
				,CharacteristicsOfFurniture =  	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.SocioeconomicStudy.HouseLayout.CharacteristicsOfFurniture')) FROM JSON_TABLE)
			WHERE Id = HouseLayoutId;
		
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateDistrict`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateDistrict`(
	  IN  `JSONData` LONGTEXT,
      OUT `DistrictId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.District.Id') INTO DistrictId
	FROM JSON_TABLE;
 
	
	IF DistrictId = 0 THEN						
		
		INSERT INTO District (`TypeOfDistrictId` ,`AguaPotable`, `Telefono`, `Electricidad`, `Drenaje`,`Hospital`,`Correo`,`Escuela`,`Policia`,`AlumbradoPublico`,`ViasDeAcceso`,`TransportePublico`,`AseoPublico`,`Iglesia`,`Otros`,`Description`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.TypeOfDistrictId'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AguaPotable'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Telefono'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Electricidad'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Drenaje'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Hospital'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Correo'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Escuela'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Policia'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AlumbradoPublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.ViasDeAcceso'))			
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.TransportePublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AseoPublico'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Iglesia'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Otros'))
			,JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Description'))
		FROM JSON_TABLE;
		SET DistrictId = LAST_INSERT_ID();

	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM District WHERE Id = DistrictId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'District not found';
		ELSE		
						
			UPDATE District
				SET
				 TypeOfDistrictId = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.TypeOfDistrictId')) FROM JSON_TABLE)
				,AguaPotable =  		(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AguaPotable')) FROM JSON_TABLE)
				,Telefono = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Telefono')) FROM JSON_TABLE)
				,Electricidad = 		(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Electricidad')) FROM JSON_TABLE)
				,Drenaje =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Drenaje')) FROM JSON_TABLE)
				,Hospital = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Hospital')) FROM JSON_TABLE)
				,Correo = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Correo')) FROM JSON_TABLE)
				,Escuela =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Escuela')) FROM JSON_TABLE)
				,Policia = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Policia')) FROM JSON_TABLE)
				,AlumbradoPublico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AlumbradoPublico')) FROM JSON_TABLE)
				,ViasDeAcceso =  		(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.ViasDeAcceso')) FROM JSON_TABLE)
				,TransportePublico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.TransportePublico')) FROM JSON_TABLE)
				,AseoPublico = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.AseoPublico')) FROM JSON_TABLE)
				,Iglesia =  			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Iglesia')) FROM JSON_TABLE)
				,Otros = 				(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Otros')) FROM JSON_TABLE)
				,Description = 			(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.District.Description')) FROM JSON_TABLE)				
			WHERE Id = DistrictId;
			
		END IF;
	END IF;
END ;;
DELIMITER ;


DROP PROCEDURE IF EXISTS `AddOrUpdateEconomicSituation`;

DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrUpdateEconomicSituation`(
	  IN  `JSONData` LONGTEXT,
      OUT `EconomicSituationId` INT,
	  OUT `ErrorMessage` VARCHAR(2000)
)
BEGIN
   
	DECLARE rowExists INT;

	DECLARE exit handler for SQLEXCEPTION
	BEGIN
		 ROLLBACK;
		 GET DIAGNOSTICS CONDITION 1 @sqlstate = RETURNED_SQLSTATE, 
		 @errno = MYSQL_ERRNO, @text = MESSAGE_TEXT;
		 SET ErrorMessage = CONCAT("ERROR ", @errno, " (", @sqlstate, "): ", @text);
	END;
    
	DROP TEMPORARY TABLE IF EXISTS JSON_TABLE;

	CREATE TEMPORARY TABLE JSON_TABLE
	SELECT JSONData AS 'Data';
	
	SELECT
	JSON_EXTRACT(Data, '$.EconomicSituation.Id') INTO EconomicSituationId
	FROM JSON_TABLE;
 
	
	IF EconomicSituationId = 0 THEN						
		
		INSERT INTO EconomicSituation (`NivelSocioEconomico`)
		SELECT
			 JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.NivelSocioEconomico'))
		FROM JSON_TABLE;
		SET EconomicSituationId = LAST_INSERT_ID();

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'Automovil')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Automovil')) FROM JSON_TABLE);
		
		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'Modelo')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Modelo')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'CasaHabitacion')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.CasaHabitacion')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'CasaHabitacionUbicacion')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.CasaHabitacionUbicacion')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'Terreno')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Terreno')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'TerrenoUbicacion')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.TerrenoUbicacion')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'Otros')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Otros')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'Ahorros')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Ahorros')) FROM JSON_TABLE);

		INSERT INTO EconomicSituationPatrimonyRelation (`EconomicSituationId`, `PatrimonyId`, `Value`)
		SELECT EconomicSituationId
		,(SELECT `Id` FROM Patrimony WHERE `Name` = 'FrecuenciaDeAhorro')
		,(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.FrecuenciaDeAhorro')) FROM JSON_TABLE);
		

	ELSE
		
		SELECT  EXISTS(SELECT 1 FROM EconomicSituation WHERE Id = EconomicSituationId) INTO rowExists;
		
		IF rowExists = 0 THEN
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'EconomicSituation not found';
		ELSE		
						
			UPDATE EconomicSituation
				SET
				NivelSocioEconomico = 	(SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.NivelSocioEconomico')) FROM JSON_TABLE)							
			WHERE Id = EconomicSituationId;

			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Automovil')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'Automovil';

			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Modelo')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'Modelo';

			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.CasaHabitacion')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'CasaHabitacion';
			
			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.CasaHabitacionUbicacion')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'CasaHabitacionUbicacion';
			
			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Terreno')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'Terreno';
			
			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.TerrenoUbicacion')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'TerrenoUbicacion';
			
			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Otros')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'Otros';

			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.Ahorros')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'Ahorros';
			
			UPDATE EconomicSituationPatrimonyRelation espr
			JOIN Patrimony p on p.Id = espr.PatrimonyId
			SET `Value` = (SELECT JSON_UNQUOTE(JSON_EXTRACT(Data, '$.EconomicSituation.FrecuenciaDeAhorro')) FROM JSON_TABLE)
			WHERE espr.EconomicSituationId = EconomicSituationId AND p.`Name` = 'FrecuenciaDeAhorro';
			
		END IF;
	END IF;
END ;;
DELIMITER ;
