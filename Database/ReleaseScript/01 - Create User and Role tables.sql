CREATE TABLE dbo.Role(
	RoleID int NOT NULL CONSTRAINT PK_Role_RoleID PRIMARY KEY,
	RoleName varchar(100) NOT NULL CONSTRAINT AK_Role_RoleName UNIQUE,
	RoleDisplayName varchar(100) NOT NULL CONSTRAINT AK_Role_RoleDisplayName UNIQUE,
	RoleDescription varchar(255) NULL,
	SortOrder int NOT NULL
)


CREATE TABLE dbo.[User](
	UserID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_User_UserID PRIMARY KEY,
	UserGuid uniqueidentifier NULL,
	FirstName varchar(100) NOT NULL,
	LastName varchar(100) NOT NULL,
	Email varchar(255) NOT NULL CONSTRAINT AK_User_Email UNIQUE ,
	Phone varchar(30) NULL,
	RoleID int NOT NULL CONSTRAINT FK_User_Role_RoleID FOREIGN KEY REFERENCES dbo.Role (RoleID),
	CreateDate datetime NOT NULL,
	UpdateDate datetime NULL,
	LastActivityDate datetime NULL,
	IsActive bit NOT NULL,
	ReceiveSupportEmails bit NOT NULL,
	LoginName varchar(128) NULL
)
GO

INSERT INTO dbo.Role(RoleID, RoleName, RoleDisplayName, RoleDescription, SortOrder)
VALUES
(1, 'Admin', 'Administrator', '', 30),
(2, 'Normal', 'Landowner', '', 20),
(3, 'Unassigned', 'Unassigned', '', 10)

insert into dbo.[User](UserGuid, FirstName, LastName, Email, RoleID, CreateDate, ReceiveSupportEmails, LoginName, IsActive)
values
('CD3DAB18-4242-4FE9-AB10-874CA43AAEE2', 'Ray', 'Lee', 'ray@sitkatech.com', 1, '3/13/2020 10:00 AM', 0, 'ray@sitkatech.com', 1),
('2F783A30-36E1-4B0C-A1B6-AA4AFE68DDB3', 'John', 'Burns', 'john.burns@sitkatech.com', 1, '3/13/2020 10:00 AM', 1, 'john.burns@sitkatech.com', 1),
('BACFE929-7BB5-4AD4-B93A-8BC56AACC49B', 'Kathleen', 'Elmquist', 'kathleen.elmquist@sitkatech.com', 1, '3/13/2020 10:00 AM', 1, 'kathleen.elmquist', 1),
('8A9C82C3-9900-4877-A83C-73986DD63A18', 'Nick', 'Padhina', 'nick.padhina@sitkatech.com', 1, '3/13/2020 10:00 AM', 0, 'n', 1),
('6C42B796-747F-4418-932E-7C622B66E2AE', 'Mack', 'Peters', 'mack.peters@sitkatech.com', 1, '3/13/2020 10:00 AM', 0, 'mack.peters', 1),
('B6AB3A16-FFFA-43CE-AFC0-599D89E1A12E', 'Ari', 'Bakoss', 'arielle.bakoss@sitkatech.com', 1, '3/13/2020 10:00 AM', 0, 'abakoss', 1)