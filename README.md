## Delete Migration and Add new Migraation and update database
 1.Add-migration init
 2.Update Database)
## Comment out this //using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await RoleSeeder.SeedRolesAsync(services);
//    await RoleSeeder.SeedUsersAsync(services);
//}
then run the project again comment it 


## Run this query for Create this table 



 ##  ChartOfAccounts Table
sql
Copy
Edit
CREATE TABLE [dbo].[ChartOfAccounts](
    [Id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
    [ParentId] [int] NULL,
      NULL,
    [IsActive] [bit] NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

SET IDENTITY_INSERT [dbo].[ChartOfAccounts] ON;
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES 
(1, N'Bank', NULL, N'Savings', 1),
(3, N'Cash', NULL, N'Hand Cash', 1),
(4, N' Petty Cash', 3, N'Hand Cash', 1),
(6, N'Accounts Receivable - Customers', 5, N'Normal', 1),
(7, N'DBBL', 1, N'Salary', 1),
(8, N'FSIB', 1, N'Deposit', 1),
(9, N'Receivables', NULL, N'Normal', 1),
(10, N'Employee Advances', 9, N'Normal', 1);
SET IDENTITY_INSERT [dbo].[ChartOfAccounts] OFF;
COMMIT;
 ##  Modules Table
sql
Copy
Edit
CREATE TABLE [dbo].[Modules](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] nvarchar NOT NULL,
    [Url] nvarchar NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

SET IDENTITY_INSERT [dbo].[Modules] ON;
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES 
(1, N'Report', N'Report/Report'),
(2, N'Dashboard', N'Index'),
(3, N'Add Module', N'Module/AddModule'),
(4, N'Add Account', N'Account/ChartOfAccounts'),
(5, N'Add Voucher', N'Account/Voucher');
SET IDENTITY_INSERT [dbo].[Modules] OFF;
COMMIT;
 ##  RoleModules Table
sql
Copy
Edit
CREATE TABLE [dbo].[RoleModules](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar NOT NULL,
    [ModuleId] [int] NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

SET IDENTITY_INSERT [dbo].[RoleModules] ON;
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES 
(1, N'Viewer', 1),
(2, N'Admin', 1),
(3, N'Accountant', 1),
(4, N'Admin', 2),
(5, N'Admin', 3),
(6, N'Admin', 4),
(7, N'Admin', 5),
(8, N'Accountant', 5);
SET IDENTITY_INSERT [dbo].[RoleModules] OFF;
COMMIT;
 ##  VoucherHeaders Table
sql
Copy
Edit
CREATE TABLE [dbo].[VoucherHeaders](
    [VoucherID] [int] IDENTITY(1,1) NOT NULL,
    [VoucherDate] [date] NOT NULL,
    [ReferenceNo] nvarchar NULL,
    [VoucherType] nvarchar NOT NULL,
    [CreatedBy] [int] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    PRIMARY KEY CLUSTERED ([VoucherID] ASC)
);
GO

SET IDENTITY_INSERT [dbo].[VoucherHeaders] ON;
INSERT [dbo].[VoucherHeaders] ([VoucherID], [VoucherDate], [ReferenceNo], [VoucherType], [CreatedBy], [CreatedDate]) 
VALUES (1, '2025-05-30', N'45367829', N'Journal', 0, '2025-05-31T00:54:59.243');
SET IDENTITY_INSERT [dbo].[VoucherHeaders] OFF;
COMMIT;
 ##  VoucherLines Table
sql
Copy
Edit
CREATE TABLE [dbo].[VoucherLines](
    [LineID] [int] IDENTITY(1,1) NOT NULL,
    [VoucherID] [int] NULL,
    [AccountID] [int] NOT NULL,
    [DebitAmount] [decimal](18, 2) NULL,
    [CreditAmount] [decimal](18, 2) NULL,
      NULL,
    PRIMARY KEY CLUSTERED ([LineID] ASC)
);
GO

SET IDENTITY_INSERT [dbo].[VoucherLines] ON;
INSERT [dbo].[VoucherLines] ([LineID], [VoucherID], [AccountID], [DebitAmount], [CreditAmount], [Narration])
VALUES (1, 1, 1, 1000.00, 20000.00, N'something happen');
SET IDENTITY_INSERT [dbo].[VoucherLines] OFF;
COMMIT;
 ##  Vouchers Table
sql
Copy
Edit
CREATE TABLE [dbo].[Vouchers](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [VoucherDate] [date] NULL,
    [ReferenceNo] nvarchar NULL,
    [VoucherType] nvarchar NULL,
    [CreatedBy] [int] NULL,
    [AccountID] [int] NULL,
    [DebitAmount] [decimal](18, 2) NULL,
    [CreditAmount] [decimal](18, 2) NULL,
    [Narration] nvarchar NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
COMMIT;
 ## Constraints & Defaults
sql
Copy
Edit
-- Default Constraints
ALTER TABLE [dbo].[ChartOfAccounts] ADD DEFAULT ((1)) FOR [IsActive];
ALTER TABLE [dbo].[VoucherHeaders] ADD DEFAULT (getdate()) FOR [CreatedDate];
ALTER TABLE [dbo].[VoucherLines] ADD DEFAULT ((0)) FOR [DebitAmount];
ALTER TABLE [dbo].[VoucherLines] ADD DEFAULT ((0)) FOR [CreditAmount];

-- Foreign Keys
ALTER TABLE [dbo].[RoleModules] WITH CHECK ADD FOREIGN KEY([ModuleId]) REFERENCES [dbo].[Modules] ([Id]);
ALTER TABLE [dbo].[VoucherLines] WITH CHECK ADD FOREIGN KEY([VoucherID]) REFERENCES [dbo].[VoucherHeaders] ([VoucherID]);
COMMIT;
 ## UDTT

CREATE TYPE [dbo].[VoucherLineType] AS TABLE
(
    AccountID INT,
    DebitAmount DECIMAL(18,2),
    CreditAmount DECIMAL(18,2),
    Narration NVARCHAR(200)
)



## Exicute this command for create storeProcidure

USE [AccountDB]
GO

-- =============================================
-- 1. Add Module and Assign to Role
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddModuleAndAssignToRole]
    @ModuleName NVARCHAR(100),
    @ModuleUrl NVARCHAR(200),
    @RoleName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert Module if not exists
    IF NOT EXISTS (SELECT 1 FROM Modules WHERE Name = @ModuleName)
    BEGIN
        INSERT INTO Modules (Name, Url) VALUES (@ModuleName, @ModuleUrl)
    END
    ELSE
    BEGIN
        -- Optional: update URL if changed
        UPDATE Modules SET Url = @ModuleUrl WHERE Name = @ModuleName
    END

    -- Get ModuleId
    DECLARE @ModuleId INT = (SELECT Id FROM Modules WHERE Name = @ModuleName)

    -- Insert RoleModule if not exists
    IF NOT EXISTS (
        SELECT 1 FROM RoleModules WHERE RoleName = @RoleName AND ModuleId = @ModuleId
    )
    BEGIN
        INSERT INTO RoleModules (RoleName, ModuleId) VALUES (@RoleName, @ModuleId)
    END
END
GO

-- =============================================
-- 2. Get Modules for User Roles
-- =============================================
CREATE PROCEDURE [dbo].[GetModulesForUserRoles]
    @UserName NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT m.Id, m.Name, m.Url
    FROM Modules m
    INNER JOIN RoleModules rm ON m.Id = rm.ModuleId
    INNER JOIN AspNetUsers u ON u.UserName = @UserName
    INNER JOIN AspNetUserRoles ur ON ur.UserId = u.Id
    WHERE rm.RoleName = (
        SELECT r.Name
        FROM AspNetRoles r
        WHERE r.Id = ur.RoleId
    )
END
GO

-- =============================================
-- 3. Manage Chart of Accounts (Insert/Update/Delete)
-- =============================================
CREATE PROCEDURE [dbo].[sp_ManageChartOfAccounts]
    @Action NVARCHAR(10),            -- 'Insert', 'Update', 'Delete'
    @Id INT = NULL,
    @AccountName NVARCHAR(100) = NULL,
    @ParentId INT = NULL,
    @AccountType NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO ChartOfAccounts (AccountName, ParentId, AccountType)
        VALUES (@AccountName, @ParentId, @AccountType)
    END
    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE ChartOfAccounts
        SET AccountName = @AccountName,
            ParentId = @ParentId,
            AccountType = @AccountType
        WHERE Id = @Id
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        DELETE FROM ChartOfAccounts WHERE Id = @Id
    END
END
GO

-- =============================================
-- 4. Save Voucher with Table-Valued Parameter
-- =============================================
-- Required TVP: CREATE TYPE [dbo].[VoucherLineType] AS TABLE
-- (
--   AccountID INT,
--   DebitAmount DECIMAL(18,2),
--   CreditAmount DECIMAL(18,2),
--   Narration NVARCHAR(200)
-- )
-- Ensure to create this first before using the SP below.

CREATE PROCEDURE [dbo].[sp_SaveVoucher]
    @VoucherDate DATE,
    @ReferenceNo NVARCHAR(50),
    @VoucherType NVARCHAR(20),
    @CreatedBy INT,
    @VoucherLines VoucherLineType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO VoucherHeaders (VoucherDate, ReferenceNo, VoucherType, CreatedBy, CreatedDate)
    VALUES (@VoucherDate, @ReferenceNo, @VoucherType, @CreatedBy, GETDATE());

    DECLARE @VoucherID INT = SCOPE_IDENTITY();

    INSERT INTO VoucherLines (VoucherID, AccountID, DebitAmount, CreditAmount, Narration)
    SELECT @VoucherID, AccountID, DebitAmount, CreditAmount, Narration
    FROM @VoucherLines;
END
GO

-- =============================================
-- 5. Get Voucher List
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetVoucherList]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        vh.VoucherID,
        vh.VoucherDate,
        vh.ReferenceNo,
        vh.VoucherType,
        vh.CreatedBy,
        vh.CreatedDate,
        vl.LineID,
        vl.AccountID,
        coa.AccountName,
        coa.AccountType,
        vl.DebitAmount,
        vl.CreditAmount,
        vl.Narration
    FROM VoucherHeaders vh
    INNER JOIN VoucherLines vl ON vh.VoucherID = vl.VoucherID
    INNER JOIN ChartOfAccounts coa ON vl.AccountID = coa.Id
    ORDER BY vh.VoucherDate DESC, vh.VoucherID DESC;
END
GO






