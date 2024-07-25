CREATE TABLE [dbo].[PrismDataType]
(
	[PrismDataTypeID]				INT NOT NULL,
	[PrismDataTypeName]				VARCHAR(100) NOT NULL,
	[PrismDataTypeDisplayName]		VARCHAR(100) NOT NULL,
	
	CONSTRAINT [PK_PrismDataType_PrismDataTypeID] PRIMARY KEY CLUSTERED ([PrismDataTypeID] ASC)
)
