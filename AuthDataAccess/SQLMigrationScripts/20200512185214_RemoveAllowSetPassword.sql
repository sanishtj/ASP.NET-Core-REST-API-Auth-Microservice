DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tenants]') AND [c].[name] = N'AllowSetPassword');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Tenants] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Tenants] DROP COLUMN [AllowSetPassword];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200512185214_RemoveAllowSetPassword', N'3.1.3');

GO
