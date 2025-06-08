IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Authors] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Phone] varchar(13) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
);

CREATE TABLE [Genres] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(255) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
);

CREATE TABLE [Books] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [GenreId] uniqueidentifier NOT NULL,
    [AuthorId] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Books_Genres_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);

CREATE INDEX [IX_Books_GenreId] ON [Books] ([GenreId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250606220456_v1', N'9.0.5');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Genres]') AND [c].[name] = N'Description');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Genres] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Genres] ALTER COLUMN [Description] nvarchar(255) NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Authors]') AND [c].[name] = N'Phone');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Authors] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Authors] ALTER COLUMN [Phone] varchar(13) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Authors]') AND [c].[name] = N'Email');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Authors] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Authors] ALTER COLUMN [Email] varchar(100) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250607015142_v2', N'9.0.5');

COMMIT;
GO