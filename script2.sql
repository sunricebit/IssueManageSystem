DROP SCHEMA IF EXISTS IMS;

CREATE DATABASE IMS;

USE IMS;

-- CreateTable
CREATE TABLE `Setting` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Type` VARCHAR(20) NOT NULL,
    `Value` VARCHAR(50) NOT NULL,
    `Description` VARCHAR(400) NULL,

    INDEX `Setting_Id_idx`(`Id`),
    INDEX `Setting_Type_Value_idx`(`Type`, `Value`),
    UNIQUE INDEX `Setting_Type_Value_key`(`Type`, `Value`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Contact` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Email` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    `Phone` VARCHAR(15) NULL,
    `IsValid` BOOLEAN NOT NULL DEFAULT true,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `CarerId` INTEGER NULL,
    `ContactTypeId` INTEGER NOT NULL,

    INDEX `Contact_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Post` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Title` TEXT NOT NULL,
    `Description` TEXT NULL,
    `Excerpt` TEXT NULL,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `UpdatedAt` DATETIME(3) NULL,
    `IsPublic` BOOLEAN NOT NULL DEFAULT false,
    `ImageUrl` VARCHAR(300) NULL,
    `AuthorId` INTEGER NOT NULL,
    `CategoryId` INTEGER NULL,

    INDEX `Post_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Report` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Content` VARCHAR(300) NOT NULL,
    `ReporterId` INTEGER NOT NULL,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `PostId` INTEGER NOT NULL,

    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `User` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Email` VARCHAR(100) NOT NULL,
    `Password` VARCHAR(64) NOT NULL,
    `RoleId` INTEGER NOT NULL,
    `Name` VARCHAR(50) NOT NULL,
    `Avatar` VARCHAR(300) NULL,
    `Gender` BOOLEAN NULL,
    `Phone` VARCHAR(15) NULL,
    `Address` VARCHAR(191) NULL,
    `Status` BOOLEAN NULL,
    `LstAccessTime` DATETIME(3) NULL,
    `ConfirmToken` VARCHAR(64) NULL,
    `ResetToken` VARCHAR(64) NULL,

    UNIQUE INDEX `User_Email_key`(`Email`),
    INDEX `User_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Assignment` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    `Description` TEXT NULL,
    `SubjectId` INTEGER NULL,
    `IsActive` BOOLEAN NOT NULL DEFAULT true,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),

    INDEX `Assignment_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Class` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(15) NOT NULL,
    `Description` VARCHAR(300) NULL,
    `TeacherId` INTEGER NULL,
    `SubjectId` INTEGER NULL,
    `IsActive` BOOLEAN NOT NULL DEFAULT false,

    INDEX `Class_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `ClassStudent` (
    `StudentId` INTEGER NOT NULL,
    `ClassId` INTEGER NOT NULL,

    INDEX `ClassStudent_ClassId_StudentId_idx`(`ClassId`, `StudentId`),
    PRIMARY KEY (`ClassId`, `StudentId`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Issue` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Title` TEXT NOT NULL,
    `Description` TEXT NOT NULL,
    `MilestoneId` INTEGER NULL,
    `ProjectId` INTEGER NULL,
    `AuthorId` INTEGER NOT NULL,
    `AssigneeId` INTEGER NULL,

    INDEX `Issue_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `IssueSetting` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(50) NOT NULL,
    `Status` BOOLEAN NOT NULL DEFAULT true,
    `ClassId` INTEGER NULL,
    `IssueId` INTEGER NULL,
    `ProjectId` INTEGER NULL,

    INDEX `IssueSetting_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Message` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Content` VARCHAR(100) NOT NULL,
    `ContactId` INTEGER NULL,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),

    INDEX `Message_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Milestone` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Title` TEXT NOT NULL,
    `Description` TEXT NULL,
    `StartDate` DATETIME(3) NULL,
    `EndDate` DATETIME(3) NULL,
    `ClassId` INTEGER NULL,
    `AssignmentId` INTEGER NULL,

    INDEX `Milestone_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Permission` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `RoleId` INTEGER NOT NULL,
    `PageId` INTEGER NOT NULL,
    `CanCreate` BOOLEAN NOT NULL DEFAULT false,
    `CanRead` BOOLEAN NOT NULL DEFAULT false,
    `CanUpdate` BOOLEAN NOT NULL DEFAULT false,
    `CanDelete` BOOLEAN NOT NULL DEFAULT false,

    INDEX `Permission_RoleId_PageId_idx`(`RoleId`, `PageId`),
    UNIQUE INDEX `Permission_RoleId_PageId_key`(`RoleId`, `PageId`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Project` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `EnglishName` VARCHAR(100) NOT NULL,
    `VietnameseName` VARCHAR(100) NOT NULL,
    `Status` BOOLEAN NULL DEFAULT true,
    `Description` TEXT NOT NULL,
    `ClassId` INTEGER NULL,
    `LeaderId` INTEGER NULL,

    INDEX `Project_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `ProjectStudent` (
    `StudentId` INTEGER NOT NULL,
    `ProjectId` INTEGER NOT NULL,

    INDEX `ProjectStudent_ProjectId_StudentId_idx`(`ProjectId`, `StudentId`),
    PRIMARY KEY (`ProjectId`, `StudentId`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Subject` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Code` VARCHAR(10) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    `Description` TEXT NULL,
    `IsActive` BOOLEAN NOT NULL DEFAULT true,
    `SubjectManagerId` INTEGER NOT NULL,
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),

    INDEX `Subject_Id_idx`(`Id`),
    PRIMARY KEY (`Id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `Contact` ADD CONSTRAINT `Contact_ContactTypeId_fkey` FOREIGN KEY (`ContactTypeId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Contact` ADD CONSTRAINT `Contact_CarerId_fkey` FOREIGN KEY (`CarerId`) REFERENCES `User`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Post` ADD CONSTRAINT `Post_CategoryId_fkey` FOREIGN KEY (`CategoryId`) REFERENCES `Setting`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Post` ADD CONSTRAINT `Post_AuthorId_fkey` FOREIGN KEY (`AuthorId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Report` ADD CONSTRAINT `Report_ReporterId_fkey` FOREIGN KEY (`ReporterId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Report` ADD CONSTRAINT `Report_PostId_fkey` FOREIGN KEY (`PostId`) REFERENCES `Post`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `User` ADD CONSTRAINT `User_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Assignment` ADD CONSTRAINT `Assignment_SubjectId_fkey` FOREIGN KEY (`SubjectId`) REFERENCES `Subject`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Class` ADD CONSTRAINT `Class_SubjectId_fkey` FOREIGN KEY (`SubjectId`) REFERENCES `Subject`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Class` ADD CONSTRAINT `Class_TeacherId_fkey` FOREIGN KEY (`TeacherId`) REFERENCES `User`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ClassStudent` ADD CONSTRAINT `ClassStudent_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `Class`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ClassStudent` ADD CONSTRAINT `ClassStudent_StudentId_fkey` FOREIGN KEY (`StudentId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Issue` ADD CONSTRAINT `Issue_AssigneeId_fkey` FOREIGN KEY (`AssigneeId`) REFERENCES `User`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Issue` ADD CONSTRAINT `Issue_AuthorId_fkey` FOREIGN KEY (`AuthorId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Issue` ADD CONSTRAINT `Issue_MilestoneId_fkey` FOREIGN KEY (`MilestoneId`) REFERENCES `Milestone`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Issue` ADD CONSTRAINT `Issue_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `Project`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `IssueSetting` ADD CONSTRAINT `IssueSetting_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `Class`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `IssueSetting` ADD CONSTRAINT `IssueSetting_IssueId_fkey` FOREIGN KEY (`IssueId`) REFERENCES `Issue`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `IssueSetting` ADD CONSTRAINT `IssueSetting_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `Project`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Message` ADD CONSTRAINT `Message_ContactId_fkey` FOREIGN KEY (`ContactId`) REFERENCES `Contact`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Milestone` ADD CONSTRAINT `Milestone_AssignmentId_fkey` FOREIGN KEY (`AssignmentId`) REFERENCES `Assignment`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Milestone` ADD CONSTRAINT `Milestone_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `Class`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Permission` ADD CONSTRAINT `Permission_PageId_fkey` FOREIGN KEY (`PageId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Permission` ADD CONSTRAINT `Permission_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Project` ADD CONSTRAINT `Project_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `Class`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Project` ADD CONSTRAINT `Project_LeaderId_fkey` FOREIGN KEY (`LeaderId`) REFERENCES `User`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ProjectStudent` ADD CONSTRAINT `ProjectStudent_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `Project`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ProjectStudent` ADD CONSTRAINT `ProjectStudent_StudentId_fkey` FOREIGN KEY (`StudentId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Subject` ADD CONSTRAINT `Subject_SubjectManagerId_fkey` FOREIGN KEY (`SubjectManagerId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

INSERT INTO `IMS`.`Setting` (`Type`, `Value`)
VALUES ('ROLE', 'Admin'),
       ('ROLE', 'Subject Manager'),
       ('ROLE', 'Class Manager'),
       ('ROLE', 'Marketer Manager'),
       ('ROLE', 'Teacher'),
       ('ROLE', 'Student'),
       ('POST_CATEGORY', 'Art and Culture'),
       ('POST_CATEGORY', 'Travel and Adventure'),
       ('POST_CATEGORY', 'Science and Technology'),
       ('POST_CATEGORY', 'Health and Lifestyle'),
       ('POST_CATEGORY', 'Learning and Personal Development'),
       ('POST_CATEGORY', 'Family Life'),
       ('CONTACT_TYPE', 'Networking'),
       ('CONTACT_TYPE', 'IT Career Development'),
       ('CONTACT_TYPE', 'Financial Aid and Scholarships'),
       ('CONTACT_TYPE', 'Faculty and Research'),
       ('CONTACT_TYPE', 'International Students'),
       ('PAGE_LINK', '/Setting/SettingList');
       
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (1, 'admin@gmail.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 1, 'Kincaid Itzakson', 'https://robohash.org/etasperioresexcepturi.png?size=100x100&set=set1', 0, '0950564222', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (2, 'subject@gmail.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 2, 'Queenie Suff', 'https://robohash.org/consectetursuntet.png?size=100x100&set=set1', null, '0654960037', '36459 Nevada Trail', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (3, 'class@gmail.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 3, 'Jeremie Bethel', 'https://robohash.org/voluptatibusconsequaturin.png?size=100x100&set=set1', 1, '0720320267', '09 Hudson Terrace', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (4, 'marketer@gmail.', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 4, 'Noella Quaintance', 'https://robohash.org/sedquodquis.png?size=100x100&set=set1', 0, '0176636839', '4522 Sycamore Avenue', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (5, 'teacher@gmail.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 5, 'Bucky Ferrettini', 'https://robohash.org/liberoreiciendisvelit.png?size=100x100&set=set1', null, '0542942149', '16 Warbler Parkway', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (6, 'student@facebook.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Iolanthe Akker', 'https://robohash.org/eosnostrumquo.png?size=100x100&set=set1', 0, '0537712440', '33107 Barby Alley', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (7, 'imatschuk6@hibu.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Ingra Matschuk', 'https://robohash.org/maioresquinulla.png?size=100x100&set=set1', 1, '0241136823', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (8, 'dorridge7@homestead.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Daron Orridge', null, null, '0378177525', '62366 Northport Crossing', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (9, 'mgegay8@bing.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Maggi Gegay', 'https://robohash.org/esseofficiasint.png?size=100x100&set=set1', 1, '0181626572', '22277 Granby Way', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (10, 'khanratty9@eventbrite.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Kahlil Hanratty', 'https://robohash.org/impeditsequiporro.png?size=100x100&set=set1', 0, '0415660856', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (11, 'jkerswella@msn.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Jacklin Kerswell', 'https://robohash.org/ininullam.png?size=100x100&set=set1', 1, '0045353089', '16 Dayton Trail', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (12, 'dmccurdyb@opera.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Darcie McCurdy', 'https://robohash.org/utetest.png?size=100x100&set=set1', 0, '0114430360', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (13, 'atolumelloc@yale.edu', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Adelice Tolumello', 'https://robohash.org/ipsamnequelaborum.png?size=100x100&set=set1', 0, '0155192798', '9651 Nova Drive', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (14, 'mdubbled@sogou.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Marcelo Dubble', 'https://robohash.org/iustonullaqui.png?size=100x100&set=set1', 0, '0714674650', '021 Aberg Lane', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (15, 'rschulze@state.tx.us', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Ruthy Schulz', null, 0, '0346589718', '9 Marcy Hill', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (16, 'mannottf@trellian.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Marten Annott', 'https://robohash.org/necessitatibusquiculpa.png?size=100x100&set=set1', 1, '0948897513', '52802 School Parkway', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (17, 'mbridgewaterg@webs.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Marcella Bridgewater', 'https://robohash.org/nesciuntofficiisconsequuntur.png?size=100x100&set=set1', 1, '0741841275', '06067 Fallview Park', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (18, 'wmandevilleh@latimes.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Westley Mandeville', 'https://robohash.org/nisivelitperspiciatis.png?size=100x100&set=set1', 1, '0145591733', '05 Northview Junction', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (19, 'awinningi@google.com.au', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Alena Winning', null, 0, '0298745665', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (20, 'ntinghillj@marriott.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Natalya Tinghill', null, 0, '0529296287', '0379 Scott Lane', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (21, 'dmilingtonk@guardian.co.uk', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Druci Milington', 'https://robohash.org/etquiomnis.png?size=100x100&set=set1', 0, '0563458391', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (22, 'dmacguinessl@geocities.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Debora MacGuiness', null, 1, '0158369774', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (23, 'efleethamm@wsj.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Ethelred Fleetham', null, null, '0625333168', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (24, 'rseabornen@dyndns.org', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Ring Seaborne', null, 0, '0264063417', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (25, 'narthyo@squarespace.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Nicole Arthy', 'https://robohash.org/expeditaperferendisodit.png?size=100x100&set=set1', null, '0868364777', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (26, 'aarnoldip@cisco.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Almeta Arnoldi', 'https://robohash.org/quisquamaliquidet.png?size=100x100&set=set1', 0, '0256102363', '784 Pond Circle', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (27, 'drydingsq@wikia.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Duncan Rydings', 'https://robohash.org/etuttempora.png?size=100x100&set=set1', 1, '0150387157', '5 Holmberg Alley', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (28, 'rder@g.co', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Rex De Meyer', null, 0, '0478593443', null, 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (29, 'esimcoxs@opensource.org', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Ellynn Simcox', 'https://robohash.org/consequaturetadipisci.png?size=100x100&set=set1', 1, '0266914275', '1282 Red Cloud Circle', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (30, 'fmeiningert@irs.gov', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Forester Meininger', 'https://robohash.org/laboreexercitationemratione.png?size=100x100&set=set1', 1, '0617649138', '62 Lakewood Gardens Junction', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (31, 'dscawnu@economist.com', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Daisy Scawn', null, 1, '0972010267', '55565 Pine View Court', 1);
INSERT INTO `IMS`.`User` (`Id`, `Email`, `Password`, `RoleId`, `Name`, `Avatar`, `Gender`, `Phone`, `Address`, `Status`) VALUES (32, 'mharmev@shop-pro.jp', '$2a$11$w9pEIVd27QqscyODByaqh./dZlCob8WHntaoI/VzF/07MY45cokVG', 6, 'Mireille Harme', null, 0, '0568924091', '127 Northfield Plaza', 1);

insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (1, 'myellowlee0@rakuten.co.jp', 'Morley Yellowlee', '0579421047', 4, 14);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (2, 'ldrewery1@cpanel.net', 'Lonnie Drewery', '0312487974', 4, 13);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (3, 'rcaitlin2@springer.com', 'Rob Caitlin', '0411455192', 1, 16);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (4, 'gchaff3@dmoz.org', 'Gilli Chaff', '0816481931', 1, 16);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (5, 'abailles4@fema.gov', 'Aime Bailles', '0504574491', 1, 15);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (6, 'kheakey5@bloglines.com', 'Katerina Heakey', '0395053048', 1, 15);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (7, 'lsyder6@usgs.gov', 'Lyndsay Syder', '0661478939', 1, 13);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (8, 'lhamsley7@google.fr', 'Lewes Hamsley', '0393419687', 1, 15);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (9, 'fweaving8@canalblog.com', 'Farlee Weaving', '0239507840', 1, 16);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (10, 'bgores9@apache.org', 'Brandtr Gores', '0749441649', 1, 15);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (11, 'cmcginleya@de.vu', 'Cindra McGinley', '0125382173', 4, 13);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (12, 'hboylesb@timesonline.co.uk', 'Hazel Boyles', '0070353353', 4, 14);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (13, 'mdavenhallc@narod.ru', 'Marla Davenhall', '0745924359', 4, 15);
insert into `IMS`.`Contact` (Id, Email, Name, Phone, CarerId, ContactTypeId) values (14, 'epealingd@vistaprint.com', 'Elie Pealing', '0298591661', 1, 15);

insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (1, 'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.

Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.

Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.', 'Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.

Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus.

Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.', 1, null, 4, 7);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (2, 'Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.

Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus.', 'In quis justo. Maecenas rhoncus aliquam lacus. Morbi quis tortor id nulla ultrices aliquet.

Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.', 0, 'http://dummyimage.com/588x643.png/ff4444/ffffff', 4, 10);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (3, 'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.

Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.', 'Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.

Sed ante. Vivamus tortor. Duis mattis egestas metus.', 1, 'http://dummyimage.com/533x699.png/dddddd/000000', 4, 10);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (4, 'Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus.

Phasellus in felis. Donec semper sapien a libero. Nam dui.', 'Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.

Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.

Morbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.', 1, 'http://dummyimage.com/448x560.png/5fa2dd/ffffff', 4, 9);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (5, 'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec pharetra, magna vestibulum aliquet ultrices, erat tortor sollicitudin mi, sit amet lobortis sapien sapien non mi. Integer ac neque.

Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.

In sagittis dui vel nisl. Duis ac nibh. Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.', 'Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.', 1, null, 4, 9);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (6, 'Duis consequat dui nec nisi volutpat eleifend. Donec ut dolor. Morbi vel lectus in quam fringilla rhoncus.', 'Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.

Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec pharetra, magna vestibulum aliquet ultrices, erat tortor sollicitudin mi, sit amet lobortis sapien sapien non mi. Integer ac neque.', 1, 'http://dummyimage.com/550x664.png/cc0000/ffffff', 4, 9);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (7, 'Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi.', 'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.

Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.', 0, 'http://dummyimage.com/423x596.png/ff4444/ffffff', 4, 7);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (8, 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.

Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.', 'Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.

Curabitur in libero ut massa volutpat convallis. Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo. Maecenas pulvinar lobortis est.

Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.', 1, null, 4, 10);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (9, 'Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.

In quis justo. Maecenas rhoncus aliquam lacus. Morbi quis tortor id nulla ultrices aliquet.

Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.', 'Morbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.

Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.', 1, 'http://dummyimage.com/443x660.png/cc0000/ffffff', 4, 11);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (10, 'Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.

In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.', 'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.

Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.', 0, 'http://dummyimage.com/492x512.png/ff4444/ffffff', 4, 9);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (11, 'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.

Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.

Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.', 'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.

Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam. Suspendisse potenti.

Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.', 0, null, 4, 12);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (12, 'Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo. In blandit ultrices enim. Lorem ipsum dolor sit amet, consectetuer adipiscing elit.

Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.

Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.

Nullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.', 0, 'http://dummyimage.com/467x648.png/cc0000/ffffff', 4, 12);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (13, 'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.

Aenean lectus. Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum.

Curabitur in libero ut massa volutpat convallis. Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo. Maecenas pulvinar lobortis est.', 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Proin risus. Praesent lectus.

Vestibulum quam sapien, varius ut, blandit non, interdum in, ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio. Curabitur convallis.

Duis consequat dui nec nisi volutpat eleifend. Donec ut dolor. Morbi vel lectus in quam fringilla rhoncus.', 0, null, 4, 10);
insert into `IMS`.`Post` (id, Title, Description, IsPublic, ImageUrl, AuthorId, CategoryId) values (14, 'Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus.

Phasellus in felis. Donec semper sapien a libero. Nam dui.', 'Sed ante. Vivamus tortor. Duis mattis egestas metus.

Aenean fermentum. Donec ut mauris eget massa tempor convallis. Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.

Quisque id justo sit amet sapien dignissim vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est. Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.', 1, null, 4, 12);


insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (1, 'MAE101', 'Mathematics for Engineering', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (2, 'CEA201', 'Computer Organization and Architecture', 1, 'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.

Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.

Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.', 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (3, 'CSI101', 'Connecting to Computer Science', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (4, 'PRF192', 'Programming Fundamentals', 1, 'Fusce consequat. Nulla nisl. Nunc nisl.

Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.', 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (5, 'SSG101', 'Working in Group Skills', 0, 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.

Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.', 2);

insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (6, 'PRJ321', 'Web-Based Java Applications', 0, 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.

Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.', 2);

insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (7, 'NWC202', 'Computer Networking', 0, 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.

Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.', 2);

insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (8, 'SWE102', 'Introduction to Software Engineering', 0, 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.

Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.', 2);

insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (9, 'JPD121', 'Elementary Japanese 1.2', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (10, 'LAB221', 'Desktop Java Lab', 0, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (11, 'JPD131', 'Elementary Japanese 2.1', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (12, 'LAB231', 'Web Java Lab', 0, null, 2);


insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (13, 'PRN292', '.NET and C#', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (14, 'SWR301', 'Software Requirements', 0, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (15, 'SWQ391', 'Software Quality Assurance and Testing', 1, null, 2);
insert into `IMS`.`Subject` (Id, Code, Name, IsActive, Description, SubjectManagerId) values (16, 'OJS201', 'On the job training	', 1, null, 2);


insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (1, 'Assignment 1', null, 1);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (2, 'Assignment 2', null, 2);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (3, 'Assignment 3', 'Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.', 3);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (4, 'Assignment 1', null, 4);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (5, 'Assignment 2', null, 5);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (6, 'Assignment 3', null, 1);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (7, 'Assignment 1', null, 2);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (8, 'Assignment 2', null, 3);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (9, 'Assignment 3', null, 4);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (10, 'Assignment 1', null, 5);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (11, 'Assignment 2', null, 1);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (12, 'Assignment 3', null, 2);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (13, 'Assignment 1', 'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.

Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.', 3);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (14, 'Assignment 2', null, 4);
insert into `IMS`.`Assignment` (Id, Name, Description, SubjectId) values (15, 'Assignment 3', null, 5);

insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (1, 'BSU397', 'Fusce consequat. Nulla nisl. Nunc nisl.', 1, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (2, 'LDT721', 'Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo. Morbi ut odio.

Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo. In blandit ultrices enim. Lorem ipsum dolor sit amet, consectetuer adipiscing elit.', 2, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (3, 'YPK507', null, 3, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (4, 'BLD988', null, 4, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (5, 'MVI063', null, 5, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (6, 'EPL988', null, 1, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (7, 'FNM375', null, 2, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (8, 'DWA190', 'Phasellus in felis. Donec semper sapien a libero. Nam dui.

Proin leo odio, porttitor id, consequat in, consequat ut, nulla. Sed accumsan felis. Ut at dolor quis odio consequat varius.

Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi.', 3, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (9, 'ZOT661', null, 4, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (10, 'YIR967', null, 5, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (11, 'YKN655', null, 1, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (12, 'OQO359', 'Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.

Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.', 2, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (13, 'DSB920', 'Morbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.', 3, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (14, 'UIH198', null, 4, 5);
insert into `IMS`.`Class` (Id, Name, Description, SubjectId, TeacherId) values (15, 'ZCH285', 'Fusce consequat. Nulla nisl. Nunc nisl.

Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa. Donec dapibus. Duis at velit eu est congue elementum.', 5, 5);

insert into `IMS`.`Permission` (`RoleId`, `PageId` ,`CanRead`)
value ('1','18',1),
('2','18',1),
('3','18',1),
('4','18',1),
('5','18',1),
('6','18',1);