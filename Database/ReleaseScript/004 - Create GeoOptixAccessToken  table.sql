create table dbo.GeoOptixAccessToken (
	GeoOptixAccessTokenID int not null identity(1,1) CONSTRAINT PK_GeoOptixAccessToken_GeoOptixAccessTokenID PRIMARY KEY,
	GeoOptixAccessTokenValue varchar(2048) not null,
	GeoOptixAccessTokenExpiryDate datetime not null
)