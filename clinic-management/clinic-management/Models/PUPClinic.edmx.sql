
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/26/2018 11:05:19
-- Generated from EDMX file: C:\Users\Jonina Fontanilla\Desktop\PM Project\clinic-management\clinic-management\clinic-management\Models\PUPClinic.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbClinicManagement];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MedCheckHeader_Patient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MedChecks] DROP CONSTRAINT [FK_MedCheckHeader_Patient];
GO
IF OBJECT_ID(N'[dbo].[FK_MedCheckHeader_Staff]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MedChecks] DROP CONSTRAINT [FK_MedCheckHeader_Staff];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_PatientType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Patients] DROP CONSTRAINT [FK_Patient_PatientType];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_PCollege]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Patients] DROP CONSTRAINT [FK_Patient_PCollege];
GO
IF OBJECT_ID(N'[dbo].[FK_Staff_UserType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Staffs] DROP CONSTRAINT [FK_Staff_UserType];
GO
IF OBJECT_ID(N'[dbo].[FK_MedCheckID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MedCheckItems] DROP CONSTRAINT [FK_MedCheckID];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemMedCheckItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MedCheckItems] DROP CONSTRAINT [FK_ItemMedCheckItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemSupply]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Supplies] DROP CONSTRAINT [FK_ItemSupply];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplySupplyChanges]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplyChanges] DROP CONSTRAINT [FK_SupplySupplyChanges];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[MedChecks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedChecks];
GO
IF OBJECT_ID(N'[dbo].[Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patients];
GO
IF OBJECT_ID(N'[dbo].[PatientTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientTypes];
GO
IF OBJECT_ID(N'[dbo].[PColleges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PColleges];
GO
IF OBJECT_ID(N'[dbo].[Staffs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Staffs];
GO
IF OBJECT_ID(N'[dbo].[Supplies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supplies];
GO
IF OBJECT_ID(N'[dbo].[UserTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTypes];
GO
IF OBJECT_ID(N'[dbo].[MedCheckItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedCheckItems];
GO
IF OBJECT_ID(N'[dbo].[SupplyChanges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplyChanges];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [ItemID] varchar(20)  NOT NULL,
    [ItemName] nvarchar(max)  NOT NULL,
    [ItemQuantity] smallint  NOT NULL,
    [ItemType] nvarchar(max)  NOT NULL,
    [ItemPurpose] nvarchar(max)  NOT NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MedChecks'
CREATE TABLE [dbo].[MedChecks] (
    [MedCheckID] int IDENTITY(1,1) NOT NULL,
    [StaffID] varchar(20)  NULL,
    [PatientID] int  NULL,
    [DateTimeOfVisit] datetime  NOT NULL,
    [Complaint] nvarchar(max)  NOT NULL,
    [Diagnosis] nvarchar(max)  NOT NULL,
    [Treatment] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [MedCheckType] int  NULL,
    [MedCheckStatus] int  NULL,
    [deleted] nvarchar(max)  NOT NULL,
    [Time_in] datetime  NULL,
    [Time_out] datetime  NULL
);
GO

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [PatientID] int IDENTITY(1,1) NOT NULL,
    [PatientLast] varchar(50)  NULL,
    [PatientFirst] varchar(max)  NULL,
    [PatientMid] varchar(50)  NULL,
    [PatientGender] varchar(20)  NULL,
    [PatientBDate] datetime  NULL,
    [PatientAddrss] varchar(max)  NULL,
    [TypeID] int  NULL,
    [PatientClass] nvarchar(max)  NULL,
    [CollegeID] int  NOT NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PatientTypes'
CREATE TABLE [dbo].[PatientTypes] (
    [TypeID] int IDENTITY(1,1) NOT NULL,
    [TypeName] varchar(20)  NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PColleges'
CREATE TABLE [dbo].[PColleges] (
    [CollegeID] int IDENTITY(1,1) NOT NULL,
    [CollegeCode] varchar(10)  NULL,
    [CollegeName] varchar(max)  NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Staffs'
CREATE TABLE [dbo].[Staffs] (
    [StaffID] varchar(20)  NOT NULL,
    [StaffLast] varchar(50)  NULL,
    [StaffFirst] varchar(max)  NULL,
    [StaffMid] varchar(50)  NULL,
    [StaffGender] varchar(20)  NULL,
    [StaffPassword] varchar(20)  NOT NULL,
    [StaffJoinedDate] datetime  NULL,
    [UserTypeID] int  NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Supplies'
CREATE TABLE [dbo].[Supplies] (
    [SupplyID] int IDENTITY(1,1) NOT NULL,
    [ItemID] varchar(20)  NOT NULL,
    [SupplyQuantity] smallint  NOT NULL,
    [ReceivedDate] datetime  NULL,
    [ExpirationDate] datetime  NULL,
    [removed] int  NOT NULL
);
GO

-- Creating table 'UserTypes'
CREATE TABLE [dbo].[UserTypes] (
    [UserTypeID] int IDENTITY(1,1) NOT NULL,
    [TypeDesc] varchar(50)  NULL,
    [deleted] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MedCheckItems'
CREATE TABLE [dbo].[MedCheckItems] (
    [MedCheckID] int  NOT NULL,
    [ItemID] varchar(20)  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'SupplyChanges'
CREATE TABLE [dbo].[SupplyChanges] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplyID] int  NOT NULL,
    [DateChange] datetime  NOT NULL,
    [ChangeReason] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ItemID] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([ItemID] ASC);
GO

-- Creating primary key on [MedCheckID] in table 'MedChecks'
ALTER TABLE [dbo].[MedChecks]
ADD CONSTRAINT [PK_MedChecks]
    PRIMARY KEY CLUSTERED ([MedCheckID] ASC);
GO

-- Creating primary key on [PatientID] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([PatientID] ASC);
GO

-- Creating primary key on [TypeID] in table 'PatientTypes'
ALTER TABLE [dbo].[PatientTypes]
ADD CONSTRAINT [PK_PatientTypes]
    PRIMARY KEY CLUSTERED ([TypeID] ASC);
GO

-- Creating primary key on [CollegeID] in table 'PColleges'
ALTER TABLE [dbo].[PColleges]
ADD CONSTRAINT [PK_PColleges]
    PRIMARY KEY CLUSTERED ([CollegeID] ASC);
GO

-- Creating primary key on [StaffID] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [PK_Staffs]
    PRIMARY KEY CLUSTERED ([StaffID] ASC);
GO

-- Creating primary key on [SupplyID] in table 'Supplies'
ALTER TABLE [dbo].[Supplies]
ADD CONSTRAINT [PK_Supplies]
    PRIMARY KEY CLUSTERED ([SupplyID] ASC);
GO

-- Creating primary key on [UserTypeID] in table 'UserTypes'
ALTER TABLE [dbo].[UserTypes]
ADD CONSTRAINT [PK_UserTypes]
    PRIMARY KEY CLUSTERED ([UserTypeID] ASC);
GO

-- Creating primary key on [MedCheckID], [ItemID] in table 'MedCheckItems'
ALTER TABLE [dbo].[MedCheckItems]
ADD CONSTRAINT [PK_MedCheckItems]
    PRIMARY KEY CLUSTERED ([MedCheckID], [ItemID] ASC);
GO

-- Creating primary key on [Id] in table 'SupplyChanges'
ALTER TABLE [dbo].[SupplyChanges]
ADD CONSTRAINT [PK_SupplyChanges]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PatientID] in table 'MedChecks'
ALTER TABLE [dbo].[MedChecks]
ADD CONSTRAINT [FK_MedCheckHeader_Patient]
    FOREIGN KEY ([PatientID])
    REFERENCES [dbo].[Patients]
        ([PatientID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedCheckHeader_Patient'
CREATE INDEX [IX_FK_MedCheckHeader_Patient]
ON [dbo].[MedChecks]
    ([PatientID]);
GO

-- Creating foreign key on [StaffID] in table 'MedChecks'
ALTER TABLE [dbo].[MedChecks]
ADD CONSTRAINT [FK_MedCheckHeader_Staff]
    FOREIGN KEY ([StaffID])
    REFERENCES [dbo].[Staffs]
        ([StaffID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedCheckHeader_Staff'
CREATE INDEX [IX_FK_MedCheckHeader_Staff]
ON [dbo].[MedChecks]
    ([StaffID]);
GO

-- Creating foreign key on [TypeID] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_Patient_PatientType]
    FOREIGN KEY ([TypeID])
    REFERENCES [dbo].[PatientTypes]
        ([TypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Patient_PatientType'
CREATE INDEX [IX_FK_Patient_PatientType]
ON [dbo].[Patients]
    ([TypeID]);
GO

-- Creating foreign key on [CollegeID] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_Patient_PCollege]
    FOREIGN KEY ([CollegeID])
    REFERENCES [dbo].[PColleges]
        ([CollegeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Patient_PCollege'
CREATE INDEX [IX_FK_Patient_PCollege]
ON [dbo].[Patients]
    ([CollegeID]);
GO

-- Creating foreign key on [UserTypeID] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [FK_Staff_UserType]
    FOREIGN KEY ([UserTypeID])
    REFERENCES [dbo].[UserTypes]
        ([UserTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Staff_UserType'
CREATE INDEX [IX_FK_Staff_UserType]
ON [dbo].[Staffs]
    ([UserTypeID]);
GO

-- Creating foreign key on [MedCheckID] in table 'MedCheckItems'
ALTER TABLE [dbo].[MedCheckItems]
ADD CONSTRAINT [FK_MedCheckID]
    FOREIGN KEY ([MedCheckID])
    REFERENCES [dbo].[MedChecks]
        ([MedCheckID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ItemID] in table 'MedCheckItems'
ALTER TABLE [dbo].[MedCheckItems]
ADD CONSTRAINT [FK_ItemMedCheckItem]
    FOREIGN KEY ([ItemID])
    REFERENCES [dbo].[Items]
        ([ItemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemMedCheckItem'
CREATE INDEX [IX_FK_ItemMedCheckItem]
ON [dbo].[MedCheckItems]
    ([ItemID]);
GO

-- Creating foreign key on [ItemID] in table 'Supplies'
ALTER TABLE [dbo].[Supplies]
ADD CONSTRAINT [FK_ItemSupply]
    FOREIGN KEY ([ItemID])
    REFERENCES [dbo].[Items]
        ([ItemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemSupply'
CREATE INDEX [IX_FK_ItemSupply]
ON [dbo].[Supplies]
    ([ItemID]);
GO

-- Creating foreign key on [SupplyID] in table 'SupplyChanges'
ALTER TABLE [dbo].[SupplyChanges]
ADD CONSTRAINT [FK_SupplySupplyChanges]
    FOREIGN KEY ([SupplyID])
    REFERENCES [dbo].[Supplies]
        ([SupplyID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplySupplyChanges'
CREATE INDEX [IX_FK_SupplySupplyChanges]
ON [dbo].[SupplyChanges]
    ([SupplyID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------