DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tenants]') AND [c].[name] = N'ModifiedOn');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Tenants] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Tenants] ALTER COLUMN [ModifiedOn] datetime2 NOT NULL;
ALTER TABLE [Tenants] ADD DEFAULT (GETDATE()) FOR [ModifiedOn];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tenants]') AND [c].[name] = N'CreatedOn');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Tenants] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Tenants] ALTER COLUMN [CreatedOn] datetime2 NOT NULL;
ALTER TABLE [Tenants] ADD DEFAULT (GETDATE()) FOR [CreatedOn];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tenants]') AND [c].[name] = N'TenantId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Tenants] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Tenants] ALTER COLUMN [TenantId] uniqueidentifier NOT NULL;
ALTER TABLE [Tenants] ADD DEFAULT (NEWID()) FOR [TenantId];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Id');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [Id] nvarchar(450) NOT NULL;
ALTER TABLE [AspNetUsers] ADD DEFAULT (NEWID()) FOR [Id];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200505174412_Set-DefaultValues', N'3.1.3');

GO
