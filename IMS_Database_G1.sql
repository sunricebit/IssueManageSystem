-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: ims
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP SCHEMA IF EXISTS IMS;

CREATE DATABASE IMS;

USE IMS;

--
-- Table structure for table `assignment`
--

DROP TABLE IF EXISTS `assignment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assignment` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` text COLLATE utf8mb4_unicode_ci,
  `SubjectId` int NOT NULL,
  `Weight` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`),
  KEY `Assignment_Id_idx` (`Id`),
  KEY `Assignment_SubjectId_fkey` (`SubjectId`),
  CONSTRAINT `Assignment_SubjectId_fkey` FOREIGN KEY (`SubjectId`) REFERENCES `subject` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assignment`
--

LOCK TABLES `assignment` WRITE;
/*!40000 ALTER TABLE `assignment` DISABLE KEYS */;
INSERT INTO `assignment` VALUES (1,'Assignment 1',NULL,1,10,1,'2023-12-16 10:01:12.183'),(2,'Assignment 2',NULL,1,10,1,'2023-12-16 10:01:12.191'),(3,'Assignment 3',NULL,1,10,1,'2023-12-16 10:01:12.195'),(4,'Assignment 1',NULL,2,10,1,'2023-12-16 10:01:12.201'),(5,'Assignment 2',NULL,2,10,1,'2023-12-16 10:01:12.207'),(6,'Assignment 3',NULL,2,10,1,'2023-12-16 10:01:12.212'),(7,'Assignment 1',NULL,3,10,1,'2023-12-16 10:01:12.220'),(8,'Assignment 2',NULL,3,10,1,'2023-12-16 10:01:12.225'),(9,'Assignment 3',NULL,3,10,1,'2023-12-16 10:01:12.230'),(10,'Assignment 1',NULL,4,10,1,'2023-12-16 10:01:12.236'),(11,'Assignment 2',NULL,4,10,1,'2023-12-16 10:01:12.242'),(12,'Assignment 3',NULL,4,10,1,'2023-12-16 10:01:12.246'),(13,'Assignment 1',NULL,5,10,1,'2023-12-16 10:01:12.252'),(14,'Assignment 2',NULL,5,10,1,'2023-12-16 10:01:12.256'),(15,'Assignment 3',NULL,5,10,1,'2023-12-16 10:01:12.261');
/*!40000 ALTER TABLE `assignment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class`
--

DROP TABLE IF EXISTS `class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(15) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` varchar(300) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `TeacherId` int DEFAULT NULL,
  `SubjectId` int DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `Class_Id_idx` (`Id`),
  KEY `Class_SubjectId_fkey` (`SubjectId`),
  KEY `Class_TeacherId_fkey` (`TeacherId`),
  CONSTRAINT `Class_SubjectId_fkey` FOREIGN KEY (`SubjectId`) REFERENCES `subject` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Class_TeacherId_fkey` FOREIGN KEY (`TeacherId`) REFERENCES `user` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class`
--

LOCK TABLES `class` WRITE;
/*!40000 ALTER TABLE `class` DISABLE KEYS */;
INSERT INTO `class` VALUES (1,'IS1234',NULL,4,1,1),(2,'IS1235',NULL,4,2,1),(3,'IS1236',NULL,4,3,1),(4,'IS1237',NULL,4,4,1),(5,'IS1238',NULL,4,5,1),(6,'PRN211',NULL,25,13,1);
/*!40000 ALTER TABLE `class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classstudent`
--

DROP TABLE IF EXISTS `classstudent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `classstudent` (
  `StudentId` int NOT NULL,
  `ClassId` int NOT NULL,
  PRIMARY KEY (`ClassId`,`StudentId`),
  KEY `ClassStudent_ClassId_StudentId_idx` (`ClassId`,`StudentId`),
  KEY `ClassStudent_StudentId_fkey` (`StudentId`),
  CONSTRAINT `ClassStudent_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `class` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `ClassStudent_StudentId_fkey` FOREIGN KEY (`StudentId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classstudent`
--

LOCK TABLES `classstudent` WRITE;
/*!40000 ALTER TABLE `classstudent` DISABLE KEYS */;
INSERT INTO `classstudent` VALUES (5,1),(5,2),(6,1),(7,1),(8,1),(9,1),(10,1),(11,1),(12,1),(13,1),(14,1),(15,1),(16,1),(17,1),(18,1),(19,1),(20,1),(21,1),(22,1),(23,1),(24,1),(25,1),(26,1),(27,1),(28,1),(29,1),(30,1),(31,1);
/*!40000 ALTER TABLE `classstudent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contact`
--

DROP TABLE IF EXISTS `contact`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contact` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Phone` varchar(15) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `IsValid` tinyint(1) NOT NULL DEFAULT '1',
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  `CarerId` int DEFAULT NULL,
  `ContactTypeId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Contact_Id_idx` (`Id`),
  KEY `Contact_ContactTypeId_fkey` (`ContactTypeId`),
  KEY `Contact_CarerId_fkey` (`CarerId`),
  CONSTRAINT `Contact_CarerId_fkey` FOREIGN KEY (`CarerId`) REFERENCES `user` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Contact_ContactTypeId_fkey` FOREIGN KEY (`ContactTypeId`) REFERENCES `setting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contact`
--

LOCK TABLES `contact` WRITE;
/*!40000 ALTER TABLE `contact` DISABLE KEYS */;
INSERT INTO `contact` VALUES (1,'myellowlee0@rakuten.co.jp','Morley Yellowlee','0579421047',1,'2023-12-16 10:01:11.876',4,14),(2,'ldrewery1@cpanel.net','Lonnie Drewery','0312487974',1,'2023-12-16 10:01:11.886',4,13),(3,'rcaitlin2@springer.com','Rob Caitlin','0411455192',1,'2023-12-16 10:01:11.891',1,16),(4,'gchaff3@dmoz.org','Gilli Chaff','0816481931',1,'2023-12-16 10:01:11.896',1,16),(5,'abailles4@fema.gov','Aime Bailles','0504574491',1,'2023-12-16 10:01:11.910',1,15),(6,'kheakey5@bloglines.com','Katerina Heakey','0395053048',1,'2023-12-16 10:01:11.920',1,15),(7,'lsyder6@usgs.gov','Lyndsay Syder','0661478939',1,'2023-12-16 10:01:11.925',1,13),(8,'lhamsley7@google.fr','Lewes Hamsley','0393419687',1,'2023-12-16 10:01:11.931',1,15),(9,'fweaving8@canalblog.com','Farlee Weaving','0239507840',1,'2023-12-16 10:01:11.935',1,16),(10,'bgores9@apache.org','Brandtr Gores','0749441649',1,'2023-12-16 10:01:11.941',1,15),(11,'cmcginleya@de.vu','Cindra McGinley','0125382173',1,'2023-12-16 10:01:11.945',4,13),(12,'hboylesb@timesonline.co.uk','Hazel Boyles','0070353353',1,'2023-12-16 10:01:11.951',4,14),(13,'mdavenhallc@narod.ru','Marla Davenhall','0745924359',1,'2023-12-16 10:01:11.991',4,15),(14,'epealingd@vistaprint.com','Elie Pealing','0298591661',1,'2023-12-16 10:01:12.000',1,15);
/*!40000 ALTER TABLE `contact` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `issue`
--

DROP TABLE IF EXISTS `issue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `issue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_unicode_ci,
  `MilestoneId` int DEFAULT NULL,
  `ProjectId` int NOT NULL,
  `DocumentUrl` longtext COLLATE utf8mb4_unicode_ci,
  `FileName` varchar(100) COLLATE utf8mb4_unicode_ci,
  `AuthorId` int NOT NULL,
  `AssigneeId` int DEFAULT NULL,
  `TypeId` int NOT NULL,
  `StatusId` int NOT NULL,
  `ProcessId` int NOT NULL,
  `ParentIssueId` int DEFAULT NULL,
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  `UpdatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`),
  KEY `Issue_Id_idx` (`Id`),
  KEY `Issue_TypeId_fkey` (`TypeId`),
  KEY `Issue_StatusId_fkey` (`StatusId`),
  KEY `Issue_ProcessId_fkey` (`ProcessId`),
  KEY `Issue_AssigneeId_fkey` (`AssigneeId`),
  KEY `Issue_AuthorId_fkey` (`AuthorId`),
  KEY `Issue_MilestoneId_fkey` (`MilestoneId`),
  KEY `Issue_ProjectId_fkey` (`ProjectId`),
  KEY `Issue_ParentIssueId_fkey` (`ParentIssueId`),
  CONSTRAINT `Issue_AssigneeId_fkey` FOREIGN KEY (`AssigneeId`) REFERENCES `user` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Issue_AuthorId_fkey` FOREIGN KEY (`AuthorId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Issue_MilestoneId_fkey` FOREIGN KEY (`MilestoneId`) REFERENCES `milestone` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Issue_ParentIssueId_fkey` FOREIGN KEY (`ParentIssueId`) REFERENCES `issue` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Issue_ProcessId_fkey` FOREIGN KEY (`ProcessId`) REFERENCES `issuesetting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Issue_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Issue_StatusId_fkey` FOREIGN KEY (`StatusId`) REFERENCES `issuesetting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Issue_TypeId_fkey` FOREIGN KEY (`TypeId`) REFERENCES `issuesetting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `issue`
--

LOCK TABLES `issue` WRITE;
/*!40000 ALTER TABLE `issue` DISABLE KEYS */;
/*!40000 ALTER TABLE `issue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `issuesetting`
--

DROP TABLE IF EXISTS `issuesetting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `issuesetting` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Value` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` varchar(200) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Color` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '#009966',
  `Status` tinyint(1) NOT NULL DEFAULT '1',
  `ClassId` int DEFAULT NULL,
  `ProjectId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IssueSetting_Id_idx` (`Id`),
  KEY `IssueSetting_ClassId_fkey` (`ClassId`),
  KEY `IssueSetting_ProjectId_fkey` (`ProjectId`),
  CONSTRAINT `IssueSetting_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `class` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `IssueSetting_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `issuesetting`
--

LOCK TABLES `issuesetting` WRITE;
/*!40000 ALTER TABLE `issuesetting` DISABLE KEYS */;
INSERT INTO `issuesetting` VALUES (1,'TYPE','R','Requirement',NULL,'#009966',1,NULL,NULL),(2,'TYPE','T','Task',NULL,'#009966',1,NULL,NULL),(3,'TYPE','Q','Q&A',NULL,'#009966',1,NULL,NULL),(4,'TYPE','D','Defect',NULL,'#009966',1,NULL,NULL),(5,'STATUS','Todo','To do',NULL,'#009966',1,NULL,NULL),(6,'STATUS','Doing','Doing',NULL,'#009966',1,NULL,NULL),(7,'STATUS','Done','Done',NULL,'#009966',1,NULL,NULL),(8,'PROCESS','Coding','Coding',NULL,'#009966',1,NULL,NULL),(9,'PROCESS','Req','Req',NULL,'#009966',1,NULL,NULL),(10,'PROCESS','Testing','Testing',NULL,'#009966',1,NULL,NULL),(11,'PROCESS','Design','Design',NULL,'#009966',1,NULL,NULL),(12,'Bug','Service','abc',NULL,'#eee600',1,1,8),(13,'Bug','Controller','abc','Bug of Controller','#330066',1,1,8),(14,'Bug','DAO','abc','Bug Of Dao','#6699cc',1,NULL,NULL),(15,'Defect','Req','abc',NULL,'#dc143c',1,NULL,NULL),(16,'Defect','Valid','abc',NULL,'#eee600',1,NULL,NULL),(17,'Defect','Test','abc',NULL,'#808080',1,1,NULL);
/*!40000 ALTER TABLE `issuesetting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `message`
--

DROP TABLE IF EXISTS `message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `message` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Content` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ContactId` int DEFAULT NULL,
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`),
  KEY `Message_Id_idx` (`Id`),
  KEY `Message_ContactId_fkey` (`ContactId`),
  CONSTRAINT `Message_ContactId_fkey` FOREIGN KEY (`ContactId`) REFERENCES `contact` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `message`
--

LOCK TABLES `message` WRITE;
/*!40000 ALTER TABLE `message` DISABLE KEYS */;
/*!40000 ALTER TABLE `message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `milestone`
--

DROP TABLE IF EXISTS `milestone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `milestone` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` text COLLATE utf8mb4_unicode_ci,
  `StartDate` datetime(3) DEFAULT NULL,
  `EndDate` datetime(3) DEFAULT NULL,
  `ProjectId` int DEFAULT NULL,
  `ClassId` int NOT NULL,
  `AssignmentId` int DEFAULT NULL,
  `Status` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Milestone_Id_idx` (`Id`),
  KEY `Milestone_AssignmentId_fkey` (`AssignmentId`),
  KEY `Milestone_ClassId_fkey` (`ClassId`),
  KEY `Milestone_ProjectId_fkey` (`ProjectId`),
  CONSTRAINT `Milestone_AssignmentId_fkey` FOREIGN KEY (`AssignmentId`) REFERENCES `assignment` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `Milestone_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `class` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Milestone_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `milestone`
--

LOCK TABLES `milestone` WRITE;
/*!40000 ALTER TABLE `milestone` DISABLE KEYS */;
INSERT INTO `milestone` VALUES (1,'Milestone 1','Description for Milestone 1','2023-01-01 00:00:00.000','2023-02-01 00:00:00.000',1,1,NULL,NULL),(2,'Milestone 2','Description for Milestone 2','2023-03-01 00:00:00.000','2023-04-01 00:00:00.000',1,1,NULL,NULL),(3,'Milestone 3','Description for Milestone 3','2023-05-01 00:00:00.000','2023-06-01 00:00:00.000',1,1,NULL,NULL),(4,'Iter1','Iter1','2023-12-16 00:00:00.000','2023-12-24 00:00:00.000',NULL,2,NULL,1),(5,'Iter2','Iter2','2023-12-16 00:00:00.000','2023-12-23 00:00:00.000',NULL,2,NULL,1),(6,'Iter1',NULL,'2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(7,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(8,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(9,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(10,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(11,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(12,'Iter3','Iter3','2023-12-16 00:00:00.000','2023-12-17 00:00:00.000',7,2,NULL,0),(13,'Iter1',NULL,'2023-12-17 00:00:00.000','2023-12-18 00:00:00.000',NULL,1,NULL,0);
/*!40000 ALTER TABLE `milestone` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permission`
--

DROP TABLE IF EXISTS `permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permission` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` int NOT NULL,
  `PageId` int NOT NULL,
  `CanCreate` tinyint(1) NOT NULL DEFAULT '0',
  `CanRead` tinyint(1) NOT NULL DEFAULT '0',
  `CanUpdate` tinyint(1) NOT NULL DEFAULT '0',
  `CanExport` tinyint(1) NOT NULL DEFAULT '0',
  `CanDelete` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Permission_RoleId_PageId_key` (`RoleId`,`PageId`),
  KEY `Permission_RoleId_PageId_idx` (`RoleId`,`PageId`),
  KEY `Permission_PageId_fkey` (`PageId`),
  CONSTRAINT `Permission_PageId_fkey` FOREIGN KEY (`PageId`) REFERENCES `setting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Permission_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `setting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=126 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permission`
--

LOCK TABLES `permission` WRITE;
/*!40000 ALTER TABLE `permission` DISABLE KEYS */;
INSERT INTO `permission` VALUES (1,1,17,1,1,1,0,0),(2,2,17,0,0,0,0,0),(3,3,17,0,0,0,0,0),(4,4,17,0,0,0,0,0),(5,5,17,0,0,0,0,0),(6,1,18,1,1,1,1,1),(7,2,18,0,1,0,0,0),(8,3,18,0,1,0,0,0),(9,5,18,0,1,0,0,0),(10,4,18,1,1,1,0,0),(11,1,19,1,1,1,0,0),(12,2,19,0,1,0,0,0),(13,3,19,0,1,0,0,0),(14,5,19,0,1,0,0,0),(15,4,19,1,1,1,0,0),(16,1,20,1,1,1,0,0),(17,2,20,0,1,0,0,0),(18,3,20,0,1,0,0,0),(19,5,20,0,1,0,0,0),(20,4,20,0,1,0,0,0),(21,1,21,1,1,1,0,0),(22,2,21,0,1,0,0,0),(23,3,21,0,1,0,0,0),(24,5,21,0,1,0,0,0),(25,4,21,0,1,0,0,0),(26,1,22,1,1,1,0,0),(27,2,22,0,1,0,0,0),(28,3,22,0,1,0,0,0),(29,5,22,0,1,0,0,0),(30,4,22,0,1,0,0,0),(31,1,23,1,1,1,0,0),(32,2,23,0,1,0,0,0),(33,3,23,0,1,0,0,0),(34,5,23,0,1,0,0,0),(35,4,23,0,1,0,0,0),(36,1,26,1,1,1,1,1),(37,2,26,0,1,0,0,0),(38,3,26,0,0,0,0,0),(39,5,26,0,0,0,0,0),(40,4,26,1,1,1,0,0),(41,1,27,1,1,1,1,1),(42,2,27,0,1,0,0,0),(43,3,27,0,0,0,0,0),(44,5,27,0,0,0,0,0),(45,4,27,0,0,0,0,0),(46,1,28,1,1,1,0,0),(47,2,28,0,1,0,0,0),(48,3,28,0,1,0,0,0),(49,5,28,0,1,0,0,0),(50,4,28,0,1,0,0,0),(51,1,29,1,1,1,1,1),(52,2,29,0,1,0,0,0),(53,3,29,0,0,0,0,0),(54,5,29,0,0,0,0,0),(55,4,29,0,0,0,0,0),(56,1,31,1,1,1,0,0),(57,2,31,0,1,0,0,0),(58,3,31,0,1,0,0,0),(59,5,31,0,1,0,0,0),(60,4,31,0,1,0,0,0),(61,1,32,1,1,1,1,1),(62,2,32,1,1,1,1,1),(63,3,32,1,1,1,1,1),(64,5,32,1,1,1,1,1),(65,4,32,1,1,1,1,1),(66,1,33,1,1,1,1,1),(67,2,33,1,1,1,1,1),(68,3,33,0,1,0,0,0),(69,5,33,0,1,0,0,0),(70,4,33,0,1,1,0,0),(71,1,34,1,1,1,1,1),(72,2,34,0,1,0,0,0),(73,3,34,0,1,0,0,0),(74,5,34,0,1,0,0,0),(75,4,34,1,1,1,0,0),(76,1,35,1,1,1,1,1),(77,2,35,0,1,0,0,0),(78,3,35,0,1,0,0,0),(79,5,35,0,1,0,0,0),(80,4,35,1,1,1,0,0),(81,1,36,1,1,1,1,1),(82,2,36,1,1,1,1,1),(83,3,36,0,1,0,0,0),(84,5,36,0,1,0,0,0),(85,4,36,0,1,1,0,0),(86,1,37,1,1,1,1,1),(87,2,37,0,1,0,0,0),(88,3,37,0,1,0,0,0),(89,5,37,0,1,0,0,0),(90,4,37,0,1,0,0,0),(91,1,38,1,1,1,1,1),(92,2,38,0,1,0,0,0),(93,3,38,0,1,0,0,0),(94,5,38,0,1,0,0,0),(95,4,38,0,1,0,0,0),(96,1,39,1,1,1,1,1),(97,2,39,0,1,0,0,0),(98,3,39,0,1,0,0,0),(99,5,39,0,1,0,0,0),(100,4,39,1,1,1,0,0),(101,1,40,1,1,1,1,1),(102,2,40,0,1,0,0,0),(103,3,40,0,1,0,0,0),(104,5,40,0,1,0,0,0),(105,4,40,1,1,1,0,0),(106,1,41,1,1,1,1,1),(107,2,41,0,1,0,0,0),(108,3,41,0,1,0,0,0),(109,5,41,0,1,0,0,0),(110,4,41,1,1,1,0,0),(111,1,42,1,1,1,1,1),(112,2,42,1,1,1,1,1),(113,3,42,1,1,1,1,1),(114,5,42,1,1,1,1,1),(115,4,42,1,1,1,1,1),(116,1,43,1,1,1,1,1),(117,2,43,0,1,0,0,0),(118,3,43,0,1,0,0,0),(119,5,43,1,1,1,0,0),(120,4,43,1,1,1,0,0),(121,1,44,1,1,1,1,1),(122,2,44,0,1,0,0,0),(123,3,44,0,1,0,0,0),(124,5,44,0,1,0,0,0),(125,4,44,0,1,0,0,0);
/*!40000 ALTER TABLE `permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `post`
--

DROP TABLE IF EXISTS `post`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `post` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_unicode_ci,
  `Excerpt` text COLLATE utf8mb4_unicode_ci,
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  `UpdatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  `IsPublic` tinyint(1) NOT NULL DEFAULT '0',
  `ImageUrl` varchar(300) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `AuthorId` int NOT NULL,
  `CategoryId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Post_Id_idx` (`Id`),
  KEY `Post_CategoryId_fkey` (`CategoryId`),
  KEY `Post_AuthorId_fkey` (`AuthorId`),
  CONSTRAINT `Post_AuthorId_fkey` FOREIGN KEY (`AuthorId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Post_CategoryId_fkey` FOREIGN KEY (`CategoryId`) REFERENCES `setting` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `post`
--

LOCK TABLES `post` WRITE;
/*!40000 ALTER TABLE `post` DISABLE KEYS */;
INSERT INTO `post` VALUES (1,'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.','Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus.Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.','Phasellus sit amet erat. Nulla tempus','2023-12-16 10:01:12.010','2023-12-16 10:01:12.010',1,NULL,4,7),(2,'Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus.','In quis justo. Maecenas rhoncus aliquam lacus. Morbi quis tortor id nulla ultrices aliquet.Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.','Sed sagittis. Nam congue, risus semper porta volutpa','2023-12-16 10:01:12.021','2023-12-16 10:01:12.021',0,'http://dummyimage.com/588x643.png/ff4444/ffffff',4,10),(3,'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.','Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.Sed ante. Vivamus tortor. Duis mattis egestas metus.','Phasellus sit amet erat. Nulla tempus','2023-12-16 10:01:12.026','2023-12-16 10:01:12.026',1,'http://dummyimage.com/533x699.png/dddddd/000000',4,10),(4,'Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus.Phasellus in felis. Donec semper sapien a libero. Nam dui.','Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.Morbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.','Quisque porta volutpat erat.','2023-12-16 10:01:12.031','2023-12-16 10:01:12.031',1,'http://dummyimage.com/448x560.png/5fa2dd/ffffff',4,9),(5,'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec pharetra, magna vestibulum aliquet ultrices, erat tortor sollicitudin mi, sit amet lobortis sapien sapien non mi. Integer ac neque.Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.In sagittis dui vel nisl. Duis ac nibh. Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.','Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.','Donec diam neque, vestibulum eget','2023-12-16 10:01:12.039','2023-12-16 10:01:12.039',1,NULL,4,9),(6,'Duis consequat dui nec nisi volutpat eleifend. Donec ut dolor. Morbi vel lectus in quam fringilla rhoncus.','Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec pharetra, magna vestibulum aliquet ultrices, erat tortor sollicitudin mi, sit amet lobortis sapien sapien non mi. Integer ac neque.','Duis consequat dui nec nisi volutpat eleifend','2023-12-16 10:01:12.045','2023-12-16 10:01:12.045',1,'http://dummyimage.com/550x664.png/cc0000/ffffff',4,9),(7,'Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi.','Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.','Nulla ut erat id mauris vulputate elementum','2023-12-16 10:01:12.052','2023-12-16 10:01:12.052',0,'http://dummyimage.com/423x596.png/ff4444/ffffff',4,7),(8,'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.','Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.Curabitur in libero ut massa volutpat convallis. Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo. Maecenas pulvinar lobortis est.Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.','Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id','2023-12-16 10:01:12.058','2023-12-16 10:01:12.058',1,NULL,4,10),(9,'Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.In quis justo. Maecenas rhoncus aliquam lacus. Morbi quis tortor id nulla ultrices aliquet.Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.','Morbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.','Nullam sit amet turpis elementum ligula vehicula consequat','2023-12-16 10:01:12.063','2023-12-16 10:01:12.063',1,'http://dummyimage.com/443x660.png/cc0000/ffffff',4,11),(10,'Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.','Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.','Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi','2023-12-16 10:01:12.069','2023-12-16 10:01:12.069',0,'http://dummyimage.com/492x512.png/ff4444/ffffff',4,9),(11,'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.','Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.','Maecenas leo odio, condimentum id, luctus nec','2023-12-16 10:01:12.075','2023-12-16 10:01:12.075',0,NULL,4,12),(12,'Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo. In blandit ultrices enim. Lorem ipsum dolor sit amet, consectetuer adipiscing elit.Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.','Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.','Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo','2023-12-16 10:01:12.081','2023-12-16 10:01:12.081',0,'http://dummyimage.com/467x648.png/cc0000/ffffff',4,12),(13,'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.Curabitur in libero ut massa volutpat convallis. Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo. Maecenas pulvinar lobortis est.','Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Proin risus. Praesent lectus.Vestibulum quam sapien, varius ut, blandit non, interdum in, ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio. Curabitur convallis.Duis consequat dui nec nisi volutpat eleifend. Donec ut dolor. Morbi vel lectus in quam fringilla rhoncus.','Proin interdum mauris non ligula pellentesque ultrices','2023-12-16 10:01:12.086','2023-12-16 10:01:12.086',0,NULL,4,10),(14,'Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus.Phasellus in felis. Donec semper sapien a libero. Nam dui.','Sed ante. Vivamus tortor. Duis mattis egestas metus.Aenean fermentum. Donec ut mauris eget massa tempor convallis. Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.Quisque id justo sit amet sapien dignissim vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est. Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.','Quisque porta volutpat erat','2023-12-16 10:01:12.093','2023-12-16 10:01:12.093',1,NULL,4,12);
/*!40000 ALTER TABLE `post` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `GroupName` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Status` tinyint(1) DEFAULT '1',
  `Description` text COLLATE utf8mb4_unicode_ci,
  `ClassId` int NOT NULL,
  `LeaderId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Project_Id_idx` (`Id`),
  KEY `Project_ClassId_fkey` (`ClassId`),
  KEY `Project_LeaderId_fkey` (`LeaderId`),
  CONSTRAINT `Project_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `class` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Project_LeaderId_fkey` FOREIGN KEY (`LeaderId`) REFERENCES `user` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project`
--

LOCK TABLES `project` WRITE;
/*!40000 ALTER TABLE `project` DISABLE KEYS */;
INSERT INTO `project` VALUES (1,'HumanResourceManagement','G1',1,NULL,1,5),(2,'HumanResourceManagement111','G2',1,NULL,1,5),(3,'FinancialProject','G2',1,NULL,1,12),(4,'CustomerManagementSystem','G2',1,NULL,1,18),(5,'LogisticsProject','G2',1,NULL,1,24),(6,'ECommerceApp','G2',1,NULL,1,30),(7,'XCB01','G1',1,NULL,2,4),(8,'VCM01','g1',1,NULL,1,4);
/*!40000 ALTER TABLE `project` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projectstudent`
--

DROP TABLE IF EXISTS `projectstudent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projectstudent` (
  `StudentId` int NOT NULL,
  `ProjectId` int NOT NULL,
  PRIMARY KEY (`ProjectId`,`StudentId`),
  KEY `ProjectStudent_ProjectId_StudentId_idx` (`ProjectId`,`StudentId`),
  KEY `ProjectStudent_StudentId_fkey` (`StudentId`),
  CONSTRAINT `ProjectStudent_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `ProjectStudent_StudentId_fkey` FOREIGN KEY (`StudentId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectstudent`
--

LOCK TABLES `projectstudent` WRITE;
/*!40000 ALTER TABLE `projectstudent` DISABLE KEYS */;
INSERT INTO `projectstudent` VALUES (4,1),(4,2),(4,3),(4,4),(4,5),(4,7),(4,8),(5,1),(5,7),(6,1),(6,8),(7,1),(8,1),(8,8),(9,1),(10,1),(11,1),(11,8),(12,2),(13,2),(14,2),(15,2),(16,2),(17,2),(18,3),(19,3),(20,3),(21,3),(22,3),(23,3),(24,4),(25,4),(26,4),(27,4),(28,4),(29,4),(30,5),(31,5);
/*!40000 ALTER TABLE `projectstudent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `report`
--

DROP TABLE IF EXISTS `report`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `report` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Content` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ReporterId` int NOT NULL,
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  `PostId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Report_ReporterId_fkey` (`ReporterId`),
  KEY `Report_PostId_fkey` (`PostId`),
  CONSTRAINT `Report_PostId_fkey` FOREIGN KEY (`PostId`) REFERENCES `post` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Report_ReporterId_fkey` FOREIGN KEY (`ReporterId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `report`
--

LOCK TABLES `report` WRITE;
/*!40000 ALTER TABLE `report` DISABLE KEYS */;
/*!40000 ALTER TABLE `report` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `setting` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Value` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Order` tinyint NOT NULL,
  `Status` tinyint(1) NOT NULL,
  `Description` varchar(400) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Setting_Type_Value_key` (`Type`,`Value`),
  KEY `Setting_Id_idx` (`Id`),
  KEY `Setting_Type_Value_idx` (`Type`,`Value`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `setting`
--

LOCK TABLES `setting` WRITE;
/*!40000 ALTER TABLE `setting` DISABLE KEYS */;
INSERT INTO `setting` VALUES (1,'ROLE','Admin',1,1,NULL),(2,'ROLE','Manager',1,1,NULL),(3,'ROLE','Marketer',1,1,NULL),(4,'ROLE','Teacher',1,1,NULL),(5,'ROLE','Student',1,1,NULL),(6,'POST_CATEGORY','Art and Culture',1,1,NULL),(7,'POST_CATEGORY','Travel and Adventure',1,1,NULL),(8,'POST_CATEGORY','Science and Technology',1,1,NULL),(9,'POST_CATEGORY','Health and Lifestyle',1,1,NULL),(10,'POST_CATEGORY','Learning and Personal Development',1,1,NULL),(11,'POST_CATEGORY','Family Life',1,1,NULL),(12,'CONTACT_TYPE','Networking',1,1,NULL),(13,'CONTACT_TYPE','IT Career Development',1,1,NULL),(14,'CONTACT_TYPE','Financial Aid and Scholarships',1,1,NULL),(15,'CONTACT_TYPE','Faculty and Research',1,1,NULL),(16,'CONTACT_TYPE','International Students',1,1,NULL),(17,'PAGE_LINK','/Setting/SettingList',1,1,NULL),(18,'PAGE_LINK','/Project/Index',0,1,NULL),(19,'PAGE_LINK','/Project/ProjectDetail',0,1,NULL),(20,'PAGE_LINK','/Setting/SettingDetail',0,0,NULL),(21,'PAGE_LINK','/Setting/AddSetting',0,0,NULL),(22,'PAGE_LINK','/Permission/PermissionManage',1,1,''),(23,'PAGE_LINK','/Setting/SettingUpdate',0,0,NULL),(24,'POST_CATEGORY','Family Not Life',2,1,''),(25,'CONTACT_TYPE','Family N Life',7,0,''),(26,'PAGE_LINK','/Project/CreateProject',0,0,NULL),(27,'PAGE_LINK','/User/Index',0,0,NULL),(28,'PAGE_LINK','/Permission/UpdatePermission',0,0,NULL),(29,'PAGE_LINK','/User/Details',0,0,NULL),(30,'PAGE_LINK','abc',1,1,NULL),(31,'PAGE_LINK','/Subject/Index',0,0,NULL),(32,'PAGE_LINK','/UserProfile/User',0,0,NULL),(33,'PAGE_LINK','/Class/Index',1,1,NULL),(34,'PAGE_LINK','/Project/Member',1,1,NULL),(35,'PAGE_LINK','/Project/ProjectMilestone',1,1,NULL),(36,'PAGE_LINK','/Class/Details',1,1,NULL),(37,'PAGE_LINK','/Auth/Logout',1,1,NULL),(38,'PAGE_LINK','/Permission/SearchPage',1,1,NULL),(39,'PAGE_LINK','/Class/People',1,1,NULL),(40,'PAGE_LINK','/Class/Milestones',1,1,NULL),(41,'PAGE_LINK','/Class/IssueSetting',1,1,NULL),(42,'PAGE_LINK','/Auth/ChangePassword',1,1,NULL),(43,'PAGE_LINK','/Project/IssueSetting',1,1,NULL),(44,'PAGE_LINK','/Issue/Index',1,1,NULL);
/*!40000 ALTER TABLE `setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subject`
--

DROP TABLE IF EXISTS `subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subject` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Code` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` text COLLATE utf8mb4_unicode_ci,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `SubjectManagerId` int NOT NULL,
  `CreatedAt` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`),
  KEY `Subject_Id_idx` (`Id`),
  KEY `Subject_SubjectManagerId_fkey` (`SubjectManagerId`),
  CONSTRAINT `Subject_SubjectManagerId_fkey` FOREIGN KEY (`SubjectManagerId`) REFERENCES `user` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subject`
--

LOCK TABLES `subject` WRITE;
/*!40000 ALTER TABLE `subject` DISABLE KEYS */;
INSERT INTO `subject` VALUES (1,'MAE101','Mathematics for Engineering',NULL,1,2,'2023-12-16 10:01:12.097'),(2,'CEA201','Computer Organization and Architecture','Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.',1,2,'2023-12-16 10:01:12.102'),(3,'CSI101','Connecting to Computer Science',NULL,1,2,'2023-12-16 10:01:12.109'),(4,'PRF192','Programming Fundamentals','Fusce consequat. Nulla nisl. Nunc nisl.Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.',1,2,'2023-12-16 10:01:12.114'),(5,'SSG101','Working in Group Skills','In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.',0,2,'2023-12-16 10:01:12.120'),(6,'PRJ321','Web-Based Java Applications','In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.',0,2,'2023-12-16 10:01:12.126'),(7,'NWC202','Computer Networking','In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.',0,2,'2023-12-16 10:01:12.131'),(8,'SWE102','Introduction to Software Engineering','In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.',0,2,'2023-12-16 10:01:12.136'),(9,'JPD121','Elementary Japanese 1.2',NULL,1,2,'2023-12-16 10:01:12.142'),(10,'LAB221','Desktop Java Lab',NULL,0,2,'2023-12-16 10:01:12.147'),(11,'JPD131','Elementary Japanese 2.1',NULL,1,2,'2023-12-16 10:01:12.152'),(12,'LAB231','Web Java Lab',NULL,0,2,'2023-12-16 10:01:12.157'),(13,'PRN292','.NET and C#',NULL,1,2,'2023-12-16 10:01:12.161'),(14,'SWR301','Software Requirements',NULL,0,2,'2023-12-16 10:01:12.168'),(15,'SWQ391','Software Quality Assurance and Testing',NULL,1,2,'2023-12-16 10:01:12.174'),(16,'OJS201','On the job training	',NULL,1,2,'2023-12-16 10:01:12.179');
/*!40000 ALTER TABLE `subject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(64) COLLATE utf8mb4_unicode_ci NOT NULL,
  `RoleId` int NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Avatar` varchar(300) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Gender` tinyint(1) DEFAULT NULL,
  `Phone` varchar(15) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Address` varchar(191) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Status` tinyint(1) DEFAULT NULL,
  `LstAccessTime` datetime(3) DEFAULT NULL,
  `ConfirmToken` varchar(64) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `ResetToken` varchar(64) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `User_Email_key` (`Email`),
  KEY `User_Id_idx` (`Id`),
  KEY `User_RoleId_fkey` (`RoleId`),
  CONSTRAINT `User_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `setting` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',1,'Kincaid Itzakson',NULL,'https://robohash.org/etasperioresexcepturi.png?size=100x100&set=set1',0,'0950564222',NULL,1,'2023-12-16 11:46:26.012',NULL,NULL),(2,'manager@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',2,'Queenie Suff',NULL,'https://robohash.org/consectetursuntet.png?size=100x100&set=set1',NULL,'0654960037','36459 Nevada Trail',1,'2023-12-16 11:46:48.535',NULL,NULL),(3,'marketer@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',3,'Jeremie Bethel',NULL,'https://robohash.org/voluptatibusconsequaturin.png?size=100x100&set=set1',1,'0720320267','09 Hudson Terrace',1,NULL,NULL,NULL),(4,'teacher@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',4,'Noella Quaintance',NULL,'https://robohash.org/sedquodquis.png?size=100x100&set=set1',0,'0176636839','4522 Sycamore Avenue',1,'2023-12-16 11:46:13.575',NULL,NULL),(5,'student@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Bucky Ferrettini',NULL,'https://robohash.org/liberoreiciendisvelit.png?size=100x100&set=set1',NULL,'0542942149','16 Warbler Parkway',1,'2023-12-16 11:45:14.367',NULL,NULL),(6,'student@facebook.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Iolanthe Akker',NULL,'https://robohash.org/eosnostrumquo.png?size=100x100&set=set1',0,'0537712440','33107 Barby Alley',1,NULL,NULL,NULL),(7,'imatschuk6@hibu.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Ingra Matschuk',NULL,'https://robohash.org/maioresquinulla.png?size=100x100&set=set1',1,'0241136823',NULL,1,NULL,NULL,NULL),(8,'dorridge7@homestead.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Daron Orridge',NULL,NULL,NULL,'0378177525','62366 Northport Crossing',1,NULL,NULL,NULL),(9,'test@gmail.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Maggi Gegay',NULL,'https://robohash.org/esseofficiasint.png?size=100x100&set=set1',1,'0181626572','22277 Granby Way',1,NULL,NULL,NULL),(10,'khanratty9@eventbrite.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Kahlil Hanratty',NULL,'https://robohash.org/impeditsequiporro.png?size=100x100&set=set1',0,'0415660856',NULL,1,NULL,NULL,NULL),(11,'jkerswella@msn.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Jacklin Kerswell',NULL,'https://robohash.org/ininullam.png?size=100x100&set=set1',1,'0045353089','16 Dayton Trail',1,NULL,NULL,NULL),(12,'dmccurdyb@opera.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Darcie McCurdy',NULL,'https://robohash.org/utetest.png?size=100x100&set=set1',0,'0114430360',NULL,1,NULL,NULL,NULL),(13,'atolumelloc@yale.edu','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Adelice Tolumello',NULL,'https://robohash.org/ipsamnequelaborum.png?size=100x100&set=set1',0,'0155192798','9651 Nova Drive',1,NULL,NULL,NULL),(14,'mdubbled@sogou.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Marcelo Dubble',NULL,'https://robohash.org/iustonullaqui.png?size=100x100&set=set1',0,'0714674650','021 Aberg Lane',1,NULL,NULL,NULL),(15,'rschulze@state.tx.us','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Ruthy Schulz',NULL,NULL,0,'0346589718','9 Marcy Hill',1,NULL,NULL,NULL),(16,'mannottf@trellian.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Marten Annott',NULL,'https://robohash.org/necessitatibusquiculpa.png?size=100x100&set=set1',1,'0948897513','52802 School Parkway',1,NULL,NULL,NULL),(17,'mbridgewaterg@webs.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Marcella Bridgewater',NULL,'https://robohash.org/nesciuntofficiisconsequuntur.png?size=100x100&set=set1',1,'0741841275','06067 Fallview Park',1,NULL,NULL,NULL),(18,'wmandevilleh@latimes.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Westley Mandeville',NULL,'https://robohash.org/nisivelitperspiciatis.png?size=100x100&set=set1',1,'0145591733','05 Northview Junction',1,NULL,NULL,NULL),(19,'awinningi@google.com.au','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Alena Winning',NULL,NULL,0,'0298745665',NULL,1,NULL,NULL,NULL),(20,'ntinghillj@marriott.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Natalya Tinghill',NULL,NULL,0,'0529296287','0379 Scott Lane',1,NULL,NULL,NULL),(21,'dmilingtonk@guardian.co.uk','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Druci Milington',NULL,'https://robohash.org/etquiomnis.png?size=100x100&set=set1',0,'0563458391',NULL,1,NULL,NULL,NULL),(22,'dmacguinessl@geocities.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Debora MacGuiness',NULL,NULL,1,'0158369774',NULL,1,NULL,NULL,NULL),(23,'efleethamm@wsj.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Ethelred Fleetham',NULL,NULL,NULL,'0625333168',NULL,1,NULL,NULL,NULL),(24,'rseabornen@dyndns.org','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Ring Seaborne',NULL,NULL,0,'0264063417',NULL,1,NULL,NULL,NULL),(25,'narthyo@squarespace.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Nicole Arthy',NULL,'https://robohash.org/expeditaperferendisodit.png?size=100x100&set=set1',NULL,'0868364777',NULL,1,NULL,NULL,NULL),(26,'aarnoldip@cisco.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Almeta Arnoldi',NULL,'https://robohash.org/quisquamaliquidet.png?size=100x100&set=set1',0,'0256102363','784 Pond Circle',1,NULL,NULL,NULL),(27,'drydingsq@wikia.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Duncan Rydings',NULL,'https://robohash.org/etuttempora.png?size=100x100&set=set1',1,'0150387157','5 Holmberg Alley',1,NULL,NULL,NULL),(28,'rder@g.co','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Rex De Meyer',NULL,NULL,0,'0478593443',NULL,1,NULL,NULL,NULL),(29,'esimcoxs@opensource.org','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Ellynn Simcox',NULL,'https://robohash.org/consequaturetadipisci.png?size=100x100&set=set1',1,'0266914275','1282 Red Cloud Circle',1,NULL,NULL,NULL),(30,'fmeiningert@irs.gov','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Forester Meininger',NULL,'https://robohash.org/laboreexercitationemratione.png?size=100x100&set=set1',1,'0617649138','62 Lakewood Gardens Junction',1,NULL,NULL,NULL),(31,'dscawnu@economist.com','$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG',5,'Daisy Scawn',NULL,NULL,1,'0972010267','55565 Pine View Court',1,NULL,NULL,NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-12-16 11:54:44
