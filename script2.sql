DROP SCHEMA IF EXISTS IMS;

CREATE DATABASE IMS;

USE IMS;

-- CreateTable
CREATE TABLE `Setting` (
    `Id` INTEGER NOT NULL AUTO_INCREMENT,
    `Type` VARCHAR(20) NOT NULL,
    `Value` VARCHAR(50) NOT NULL,
    `Description` VARCHAR(400) NULL,

    INDEX `Setting_Type_Value_idx`(`Type`, `Value`),
    INDEX `Setting_Id_idx`(`Id`),
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
    `CreatedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `UpdatedAt` DATETIME(3) NULL,
    `IsPublic` BOOLEAN NOT NULL DEFAULT false,
    `ImageUrl` VARCHAR(100) NULL,
    `AuthorId` INTEGER NOT NULL,
    `CategoryId` INTEGER NULL,

    INDEX `Post_Id_idx`(`Id`),
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
    `Code` VARCHAR(50) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    `Description` TEXT NULL,
    `SubjectId` INTEGER NULL,

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
    `Page` VARCHAR(191) NOT NULL,
    `CanCreate` BOOLEAN NOT NULL DEFAULT false,
    `CanRead` BOOLEAN NOT NULL DEFAULT false,
    `CanUpdate` BOOLEAN NOT NULL DEFAULT false,
    `CanDelete` BOOLEAN NOT NULL DEFAULT false,

    UNIQUE INDEX `Permission_RoleId_key`(`RoleId`),
    INDEX `Permission_RoleId_Page_idx`(`RoleId`, `Page`),
    UNIQUE INDEX `Permission_RoleId_Page_key`(`RoleId`, `Page`),
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
ALTER TABLE `Permission` ADD CONSTRAINT `Permission_RoleId_fkey` FOREIGN KEY (`RoleId`) REFERENCES `Setting`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Project` ADD CONSTRAINT `Project_ClassId_fkey` FOREIGN KEY (`ClassId`) REFERENCES `Class`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Project` ADD CONSTRAINT `Project_LeaderId_fkey` FOREIGN KEY (`LeaderId`) REFERENCES `User`(`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ProjectStudent` ADD CONSTRAINT `ProjectStudent_ProjectId_fkey` FOREIGN KEY (`ProjectId`) REFERENCES `Project`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ProjectStudent` ADD CONSTRAINT `ProjectStudent_StudentId_fkey` FOREIGN KEY (`StudentId`) REFERENCES `User`(`Id`) ON DELETE RESTRICT ON UPDATE CASCADE;

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
       ('CONTACT_TYPE', 'International Students');

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