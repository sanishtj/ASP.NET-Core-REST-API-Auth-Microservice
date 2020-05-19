ALTER TABLE [Tenants] ADD [AllowSetPassword] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200510171145_AllowSetPasswordForTenant', N'3.1.3');

GO
