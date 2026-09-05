SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AspNetRoles')
BEGIN
    RAISERROR('ABORTED: AspNetRoles does not exist, so this database is not an existing deployment. Run the migration normally instead of baselining it.', 16, 1);
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId]    nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT 'Created __EFMigrationsHistory (it was missing).';
END

IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260728074202_inital')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260728074202_inital', N'10.0.10');
    PRINT 'Baseline 20260728074202_inital recorded as applied.';
END
ELSE
BEGIN
    PRINT 'Baseline 20260728074202_inital was already recorded. Nothing to do.';
END

SELECT [MigrationId], [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId] DESC;
