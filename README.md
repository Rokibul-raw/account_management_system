1.Delete Migration and Add new Migraation and update database(Add-migration init ,Update Database)
2.Comment out this //using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await RoleSeeder.SeedRolesAsync(services);
//    await RoleSeeder.SeedUsersAsync(services);
//}
then run the project again comment it 
3.Run this query for Create this table 

CREATE TABLE ChartOfAccounts (
    Id INT PRIMARY KEY IDENTITY,
    AccountName NVARCHAR(100) NOT NULL,
    ParentId INT NULL, -- Nullable for top-level accounts
    AccountType NVARCHAR(50), -- Optional: Asset, Liability, Income, etc.
    IsActive BIT DEFAULT 1
);


CREATE TABLE VoucherHeaders (
    VoucherID INT IDENTITY(1,1) PRIMARY KEY,
    VoucherDate DATE NOT NULL,
    ReferenceNo NVARCHAR(50),
    VoucherType NVARCHAR(20) NOT NULL,
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE VoucherLines (
    LineID INT IDENTITY(1,1) PRIMARY KEY,
    VoucherID INT FOREIGN KEY REFERENCES VoucherHeaders(VoucherID),
    AccountID INT NOT NULL,
    DebitAmount DECIMAL(18,2) DEFAULT 0,
    CreditAmount DECIMAL(18,2) DEFAULT 0,
    Narration NVARCHAR(200)
);

CREATE TYPE VoucherLineType AS TABLE (
    AccountID INT,
    DebitAmount DECIMAL(18,2),
    CreditAmount DECIMAL(18,2),
    Narration NVARCHAR(200)
);

CREATE TABLE [dbo].[Modules]
(
 [Id] [int] IDENTITY(1,1) NOT NULL,
 [Name] [nvarchar](100) NOT NULL,
 [Url] [nvarchar](300) NULL
)

CREATE TABLE [dbo].[RoleModules]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[ModuleId] [int] NULL
)




4.Exicute this command for create storeProcidure

store procidure:1
--Add module with role

ALTER PROCEDURE AddModuleAndAssignToRole
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
        INSERT INTO		RoleModules (RoleName, ModuleId) VALUES (@RoleName, @ModuleId)
    END
END




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






