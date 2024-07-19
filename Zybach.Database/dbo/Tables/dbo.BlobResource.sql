CREATE TABLE [dbo].[BlobResource](
    [BlobResourceID]            INT                 NOT NULL IDENTITY (1, 1),
    [BlobResourceGUID]          UNIQUEIDENTIFIER    NOT NULL, 
    [BlobResourceCanonicalName] VARCHAR(100)        NOT NULL,

    [OriginalBaseFilename]      VARCHAR (255)       NOT NULL,
    [OriginalFileExtension]     VARCHAR (255)       NOT NULL,

    [CreateUserID]              INT                 NOT NULL,
    [CreateDate]                DATETIME            NOT NULL,

    CONSTRAINT [PK_BlobResource_FileResourceID]             PRIMARY KEY CLUSTERED ([BlobResourceID] ASC),

    CONSTRAINT [FK_BlobResource_User_CreateUserID_UserID]   FOREIGN KEY ([CreateUserID]) REFERENCES [dbo].[User] ([UserID]),

    CONSTRAINT [AK_BlobResource_FileResourceGUID]           UNIQUE NONCLUSTERED ([BlobResourceGUID] ASC),
    CONSTRAINT [AK_BlobResource_CanonicalName]              UNIQUE NONCLUSTERED ([BlobResourceCanonicalName] ASC),
);

