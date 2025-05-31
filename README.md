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




# Run this Query for create table 



``USE [AccountDB]
GO
/****** Object:  Table [dbo].[ChartOfAccounts]    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartOfAccounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](100) NOT NULL,
	[ParentId] [int] NULL,
	[AccountType] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO``
CREATE TABLE [dbo].[Modules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO``
/****** Object:  Table [dbo].[RoleModules]  
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO``
CREATE TABLE [dbo].[RoleModules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[ModuleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VoucherHeaders]    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VoucherHeaders](
	[VoucherID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherDate] [date] NOT NULL,
	[ReferenceNo] [nvarchar](50) NULL,
	[VoucherType] [nvarchar](20) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VoucherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VoucherLines]    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VoucherLines](
	[LineID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherID] [int] NULL,
	[AccountID] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
 
	[Narration] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[LineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vouchers]    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vouchers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VoucherDate] [date] NULL,
	[ReferenceNo] [nvarchar](50) NULL,
	[VoucherType] [nvarchar](20) NULL,
	[CreatedBy] [int] NULL,
	[AccountID] [int] NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Narration] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChartOfAccounts] ON 
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (1, N'Bank', NULL, N'Savings', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (3, N'Cash', NULL, N'Hand Cash', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (4, N' Petty Cash', 3, N'Hand Cash', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (6, N'Accounts Receivable - Customers', 5, N'Normal', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (7, N'DBBL', 1, N'Salary', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (8, N'FSIB', 1, N'Deposit', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (9, N'Receivables', NULL, N'Normal', 1)
GO
INSERT [dbo].[ChartOfAccounts] ([Id], [AccountName], [ParentId], [AccountType], [IsActive]) VALUES (10, N'Employee Advances', 9, N'Normal', 1)
GO
SET IDENTITY_INSERT [dbo].[ChartOfAccounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Modules] ON 
GO
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES (1, N'Report', N'Report/Report')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES (2, N'Dashboard', N'Index')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES (3, N'Add Module', N'Module/AddModule')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES (4, N'Add Account', N'Account/ChartOfAccounts')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Url]) VALUES (5, N'Add Voucher', N'Account/Voucher')
GO
SET IDENTITY_INSERT [dbo].[Modules] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleModules] ON 
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (1, N'Viewer', 1)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (2, N'Admin', 1)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (3, N'Accountant', 1)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (4, N'Admin', 2)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (5, N'Admin', 3)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (6, N'Admin', 4)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (7, N'Admin', 5)
GO
INSERT [dbo].[RoleModules] ([Id], [RoleName], [ModuleId]) VALUES (8, N'Accountant', 5)
GO
SET IDENTITY_INSERT [dbo].[RoleModules] OFF
GO
SET IDENTITY_INSERT [dbo].[VoucherHeaders] ON 
GO
INSERT [dbo].[VoucherHeaders] ([VoucherID], [VoucherDate], [ReferenceNo], [VoucherType], [CreatedBy], [CreatedDate]) VALUES (1, CAST(N'2025-05-30' AS Date), N'45367829', N'Journal', 0, CAST(N'2025-05-31T00:54:59.243' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[VoucherHeaders] OFF
GO
SET IDENTITY_INSERT [dbo].[VoucherLines] ON 
GO
INSERT [dbo].[VoucherLines] ([LineID], [VoucherID], [AccountID], [DebitAmount], [CreditAmount], [Narration]) VALUES (1, 1, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), N'something happen')
GO
SET IDENTITY_INSERT [dbo].[VoucherLines] OFF
GO
ALTER TABLE [dbo].[ChartOfAccounts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[VoucherHeaders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[VoucherLines] ADD  DEFAULT ((0)) FOR [DebitAmount]
GO
ALTER TABLE [dbo].[VoucherLines] ADD  DEFAULT ((0)) FOR [CreditAmount]
GO
ALTER TABLE [dbo].[RoleModules]  WITH CHECK ADD FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Modules] ([Id])
GO
ALTER TABLE [dbo].[VoucherLines]  WITH CHECK ADD FOREIGN KEY([VoucherID])
REFERENCES [dbo].[VoucherHeaders] ([VoucherID])
GO





## Exicute this command for create storeProcidure

store procidure:1
--Add module with role

USE [AccountDB]
GO

/****** Object:  StoredProcedure [dbo].[AddModuleAndAssignToRole]    Script Date: 5/31/2025 2:04:40 AM ******/
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




store procudure 2:
for getModule:



CREATE PROCEDURE GetModulesForUserRoles
    @UserName NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get roles for the user
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





--for account

CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10),         -- 'Insert', 'Update', 'Delete'
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


--For Voucher

CREATE PROCEDURE sp_SaveVoucher
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
END;




//for get voucher


CREATE PROCEDURE sp_GetVoucherList
AS
BEGIN
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






