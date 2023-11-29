DROP SCHEMA IF EXISTS IMS;

CREATE DATABASE IMS;

USE IMS;

-- CreateTable
CREATE TABLE `Setting` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Type` VARCHAR(20) NOT NULL,
    `Value` VARCHAR(50) NOT NULL,

    INDEX `Setting_Type_Value_idx`(`Type`, `Value`),
    INDEX `Setting_Id_idx`(`Id`),
    UNIQUE INDEX `Setting_Type_Value_key`(`Type`, `Value`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `User` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Email` VARCHAR(100) NOT NULL,
    `Password` VARCHAR(64) NOT NULL,
    `RoleId` INTEGER NOT NULL,
    `Name` VARCHAR(50) NOT NULL,
    `Avatar` VARCHAR(100) NULL,
    `Gender` BOOLEAN NULL,
    `Phone` VARCHAR(15) NULL,
    `Address` VARCHAR(191) NULL,
    `Status` BOOLEAN NULL,
    `ConfirmToken` VARCHAR(64) NULL,
    `ResetToken` VARCHAR(64) NULL,

    UNIQUE INDEX `User_Email_key`(`Email`),
    INDEX `User_Id_idx`(`Id`),
    INDEX `User_Email_idx`(`Email`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Contact` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Email` VARCHAR(100) NOT NULL,
    `Phone` VARCHAR(15) NULL,
    `Name` VARCHAR(50) NOT NULL,
    `Message` TEXT NOT NULL,
    `IsValid` BOOLEAN NOT NULL DEFAULT true,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `UpdatedAt` DATETIME(3) NOT NULL,

    UNIQUE INDEX `Contact_Email_key`(`Email`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Post` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Title` TEXT NOT NULL,
    `Description` TEXT NOT NULL,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `UpdatedAt` DATETIME(3) NOT NULL,
    `UserId` INTEGER NOT NULL,
    `ImageUrl` VARCHAR(100) NULL,

    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `User` ADD CONSTRAINT `User_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Post` ADD CONSTRAINT `Post_UserId_fkey` FOREIGN KEY (`UserId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

INSERT INTO `IMS`.`Setting` (`Type`, `Value`)
VALUES ('ROLE', 'Admin'),
       ('ROLE', 'Marketer'),
       ('ROLE', 'Teacher'),
       ('ROLE', 'Student');


INSERT INTO `IMS`.`User` (`Email`, `Password`, `RoleId`, `Name`, `Status`)
VALUES ('admin@gmail.com', '$2a$11$C0XheCBSOFNJThlyh.1YuOfznCCkteW8NJsXFbTSawA9DhcE2UwlK',4,'Administrator', 1);


INSERT INTO `IMS`.`User` (`Email`, `Password`, `RoleId`, `Name`, `Status`)
VALUES ('marketer@gmail.com', '$2a$11$C0XheCBSOFNJThlyh.1YuOfznCCkteW8NJsXFbTSawA9DhcE2UwlK',4,'Marketer', 1);


INSERT INTO `IMS`.`User` (`Email`, `Password`, `RoleId`, `Name`, `Status`)
VALUES ('teacher@gmail.com', '$2a$11$C0XheCBSOFNJThlyh.1YuOfznCCkteW8NJsXFbTSawA9DhcE2UwlK',4,'Teacher', 1);


INSERT INTO `IMS`.`User` (`Email`, `Password`, `RoleId`, `Name`, `Status`)
VALUES ('student@gmail.com', '$2a$11$C0XheCBSOFNJThlyh.1YuOfznCCkteW8NJsXFbTSawA9DhcE2UwlK',4,'Student', 1);