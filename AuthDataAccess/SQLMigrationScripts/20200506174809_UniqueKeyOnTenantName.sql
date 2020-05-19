CREATE UNIQUE INDEX [IX_Tenants_TenantName] ON [Tenants] ([TenantName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200506174809_UniqueKeyOnTenantName', N'3.1.3');

GO
