
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/13/2018 20:43:43
-- Generated from EDMX file: C:\Users\Hüseyin\Desktop\OnGoingProjects\GraduateModule\GraduateSoftware\GraduateSoftware\Models\GraduateDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GraduateModule];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Graduates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Graduates];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Graduates'
CREATE TABLE [dbo].[Graduates] (
    [StudentID] nvarchar(50)  NOT NULL,
    [StudentPassword] nvarchar(100)  NULL,
    [GraduateName] nvarchar(50)  NULL,
    [GraduateLastName] nvarchar(50)  NULL,
    [GraduateYear] int  NULL,
    [WorkAreaID] int  NULL,
    [WorkAreaDetailID] int  NULL,
    [GraduateCompany] nvarchar(100)  NULL,
    [GraduateTitle] nvarchar(50)  NULL,
    [GraduateMail] nvarchar(50)  NULL,
    [GraduatePhone] nvarchar(50)  NULL
);
GO

-- Creating table 'WorkAreaDetails'
CREATE TABLE [dbo].[WorkAreaDetails] (
    [WADID] int IDENTITY(1,1) NOT NULL,
    [WorkAreaDetailName] nvarchar(50)  NULL,
    [WorkAreaID] int  NULL
);
GO

-- Creating table 'WorkAreas'
CREATE TABLE [dbo].[WorkAreas] (
    [WAID] int IDENTITY(1,1) NOT NULL,
    [WorkAreaName] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [StudentID] in table 'Graduates'
ALTER TABLE [dbo].[Graduates]
ADD CONSTRAINT [PK_Graduates]
    PRIMARY KEY CLUSTERED ([StudentID] ASC);
GO

-- Creating primary key on [WADID] in table 'WorkAreaDetails'
ALTER TABLE [dbo].[WorkAreaDetails]
ADD CONSTRAINT [PK_WorkAreaDetails]
    PRIMARY KEY CLUSTERED ([WADID] ASC);
GO

-- Creating primary key on [WAID] in table 'WorkAreas'
ALTER TABLE [dbo].[WorkAreas]
ADD CONSTRAINT [PK_WorkAreas]
    PRIMARY KEY CLUSTERED ([WAID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [WorkAreaID] in table 'Graduates'
ALTER TABLE [dbo].[Graduates]
ADD CONSTRAINT [FK__Graduates__WorkA__286302EC]
    FOREIGN KEY ([WorkAreaID])
    REFERENCES [dbo].[WorkAreas]
        ([WAID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Graduates__WorkA__286302EC'
CREATE INDEX [IX_FK__Graduates__WorkA__286302EC]
ON [dbo].[Graduates]
    ([WorkAreaID]);
GO

-- Creating foreign key on [WorkAreaDetailID] in table 'Graduates'
ALTER TABLE [dbo].[Graduates]
ADD CONSTRAINT [FK__Graduates__WorkA__29572725]
    FOREIGN KEY ([WorkAreaDetailID])
    REFERENCES [dbo].[WorkAreaDetails]
        ([WADID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Graduates__WorkA__29572725'
CREATE INDEX [IX_FK__Graduates__WorkA__29572725]
ON [dbo].[Graduates]
    ([WorkAreaDetailID]);
GO

-- Creating foreign key on [WorkAreaID] in table 'WorkAreaDetails'
ALTER TABLE [dbo].[WorkAreaDetails]
ADD CONSTRAINT [FK__WorkAreaD__WorkA__398D8EEE]
    FOREIGN KEY ([WorkAreaID])
    REFERENCES [dbo].[WorkAreas]
        ([WAID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__WorkAreaD__WorkA__398D8EEE'
CREATE INDEX [IX_FK__WorkAreaD__WorkA__398D8EEE]
ON [dbo].[WorkAreaDetails]
    ([WorkAreaID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------