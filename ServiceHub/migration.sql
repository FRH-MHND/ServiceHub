CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AdminActionLogs` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AdminName` longtext CHARACTER SET utf8mb4 NULL,
    `Action` longtext CHARACTER SET utf8mb4 NULL,
    `Timestamp` datetime(6) NOT NULL,
    CONSTRAINT `PK_AdminActionLogs` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `FAQs` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Question` longtext CHARACTER SET utf8mb4 NULL,
    `Answer` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_FAQs` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Notifications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Message` longtext CHARACTER SET utf8mb4 NULL,
    `IsRead` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_Notifications` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Payments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `ServiceType` longtext CHARACTER SET utf8mb4 NULL,
    `ProviderName` longtext CHARACTER SET utf8mb4 NULL,
    `Date` datetime(6) NOT NULL,
    `AmountPaid` decimal(18,2) NOT NULL,
    `PaymentMethod` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Payments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ServiceProviders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `PersonalInformation` longtext CHARACTER SET utf8mb4 NULL,
    `BusinessDetails` longtext CHARACTER SET utf8mb4 NULL,
    `Licenses` longtext CHARACTER SET utf8mb4 NULL,
    `Status` longtext CHARACTER SET utf8mb4 NULL,
    `AvailabilityStatus` longtext CHARACTER SET utf8mb4 NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `AverageRating` double NOT NULL,
    CONSTRAINT `PK_ServiceProviders` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ServiceRequests` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserLocation` longtext CHARACTER SET utf8mb4 NULL,
    `RequestedService` longtext CHARACTER SET utf8mb4 NULL,
    `Date` datetime(6) NOT NULL,
    `Time` datetime(6) NOT NULL,
    `IsAccepted` tinyint(1) NOT NULL,
    `IsDeclined` tinyint(1) NOT NULL,
    `ProviderId` int NOT NULL,
    `UserId` int NOT NULL,
    `IsExpress` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_ServiceRequests` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `SupportContacts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_SupportContacts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `UserProfiles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `AccountSettings` longtext CHARACTER SET utf8mb4 NULL,
    `IsActive` tinyint(1) NOT NULL,
    `ReceiveBookingConfirmations` tinyint(1) NOT NULL,
    `ReceivePromotions` tinyint(1) NOT NULL,
    `ReceiveReminders` tinyint(1) NOT NULL,
    `UserId` int NOT NULL,
    CONSTRAINT `PK_UserProfiles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `VerificationCode` longtext CHARACTER SET utf8mb4 NULL,
    `Status` longtext CHARACTER SET utf8mb4 NULL,
    `IsAdmin` tinyint(1) NOT NULL,
    `IsProvider` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Services` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Category` longtext CHARACTER SET utf8mb4 NULL,
    `Price` decimal(18,2) NOT NULL,
    `AverageRating` double NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ServiceProviderId` int NOT NULL,
    CONSTRAINT `PK_Services` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Services_ServiceProviders_ServiceProviderId` FOREIGN KEY (`ServiceProviderId`) REFERENCES `ServiceProviders` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Bookings` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ServiceId` int NOT NULL,
    `ServiceProviderId` int NOT NULL,
    `UserId` int NOT NULL,
    `AppointmentDate` datetime(6) NOT NULL,
    `IssueDescription` longtext CHARACTER SET utf8mb4 NULL,
    `UrgencyLevel` longtext CHARACTER SET utf8mb4 NULL,
    `Status` longtext CHARACTER SET utf8mb4 NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Rating` int NULL,
    `Feedback` longtext CHARACTER SET utf8mb4 NULL,
    `RescheduledDate` datetime(6) NULL,
    CONSTRAINT `PK_Bookings` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Bookings_ServiceProviders_ServiceProviderId` FOREIGN KEY (`ServiceProviderId`) REFERENCES `ServiceProviders` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Bookings_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Bookings_ServiceId` ON `Bookings` (`ServiceId`);

CREATE INDEX `IX_Bookings_ServiceProviderId` ON `Bookings` (`ServiceProviderId`);

CREATE INDEX `IX_Services_ServiceProviderId` ON `Services` (`ServiceProviderId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250105084805_InitialCreate', '8.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE `ServiceProviders` ADD `PasswordHash` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `ServiceProviders` ADD `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `ServiceProviders` ADD `ServiceCategory` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250107091140_AddPasswordHashToUsers', '8.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE `ServiceProviders` ADD `AboutMe` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250111205637_AddAboutMeToServiceProvider', '8.0.2');

COMMIT;

START TRANSACTION;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250111225109_AddServiceCategoryTable', '8.0.2');

COMMIT;

