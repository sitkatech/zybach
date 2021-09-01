drop table if exists #tempUsers
go

Create table #tempUsers ([UserGuid] uniqueidentifier not null,
    [FirstName] varchar(100) not null
    ,[LastName] varchar(100) not null
    ,[Email] varchar(255) not null
    ,[RoleID] int not null
    ,[CreateDate] datetime not null
    ,[IsActive] bit not null
    ,[ReceiveSupportEmails] bit not null
    ,[LoginName] varchar(128) not null
    ,[DisclaimerAcknowledgedDate] datetime null
)
INSERT INTO #tempUsers
    ([UserGuid]
    ,[FirstName]
    ,[LastName]
    ,[Email]
    ,[RoleID]
    ,[CreateDate]
    ,[IsActive]
    ,[ReceiveSupportEmails]
    ,[LoginName]
    ,[DisclaimerAcknowledgedDate])
VALUES 
('8a9c82c3-9900-4877-a83c-73986dd63a18', 'Nick', 'Padinha', 'nick.padinha@sitkatech.com', 1, '2021-02-25T03:20:20.758Z', 1, 0, 'n', '2021-02-25T21:00:28.631Z' ),
('bacfe929-7bb5-4ad4-b93a-8bc56aacc49b', 'Kathleen', 'Elmquist', 'kathleen.elmquist@sitkatech.com', 1, '2021-03-01T21:40:02.272Z', 1, 1, 'kathleen.elmquist', '2021-03-01T21:56:12.922Z' ),
('a116bb78-5ca5-44ba-a8c4-3e7885f35208', 'Mallory', 'Morton', 'mmorton@olsson.com', 1, '2021-03-19T14:36:48.933Z', 1, 1, 'mmorton', '2021-05-25T20:16:09.370Z' ),
('2f783a30-36e1-4b0c-a1b6-aa4afe68ddb3', 'John', 'Burns', 'john.burns@sitkatech.com', 1, '2021-03-24T17:00:05.601Z', 1, 1, 'john.burns@sitkatech.com', '2021-03-24T17:01:30.296Z' ),
('6c42b796-747f-4418-932e-7c622b66e2ae', 'Mack', 'Peters', 'mack.peters@sitkatech.com', 1, '2021-03-24T17:02:01.152Z', 1, 0, 'mack.peters@sitkatech.com', '2021-05-31T20:25:37.557Z' ),
('cd3dab18-4242-4fe9-ab10-874ca43aaee2', 'Ray', 'Lee', 'ray@sitkatech.com', 1, '2021-03-24T17:02:15.854Z', 1, 0, 'ray@sitkatech.com', '2021-08-18T16:11:55.040Z' ),
('c83eff58-d73f-49f6-94c8-2728f6f510db', 'Jim', 'Schneider', 'jschneider@olsson.com', 1, '2021-03-24T17:03:40.121Z', 1, 0, 'jschneider@olsson.com', '2021-05-25T19:41:39.728Z' ),
('37a4031c-f7c8-4afa-ae38-169f0a21e2b6', 'Keith', 'Steele', 'keith@sitkatech.com', 1, '2021-04-07T19:12:07.666Z', 1, 0, 'keith@sitkatech.com', '2021-04-09T04:50:16.553Z' ),
('003a23e8-b05a-449f-a99d-921414bdd203', 'Colby', 'Osborn', 'cosborn@olsson.com', 1, '2021-05-25T19:42:28.683Z', 1, 0, 'cosborn@olsson.com', '2021-07-16T13:22:16.960Z' ),
('5715d7b5-401a-4b8e-bc18-15d996c32ea7', 'Phil', 'Heimann', 'pheimann@tpnrd.org', 1, '2021-05-25T21:00:01.277Z', 1, 0, 'pheimann@tpnrd.org', '2021-06-08T18:19:36.666Z' ),
('013f64b8-24c2-40b8-9be6-1e50b1778af6', 'Glen', 'Bowers', 'gbowers@tpnrd.org', 1, '2021-05-25T21:00:35.677Z', 1, 0, 'gbowers@tpnrd.org', '2021-05-26T12:30:06.421Z' ),
('badeafbb-b075-41ce-add5-a33bbf3be9b3', 'Ann', 'Dimmitt', 'adimmitt@tpnrd.org', 1, '2021-05-25T21:00:57.793Z', 1, 0, 'adimmitt@tpnrd.org', '2021-06-17T14:52:24.382Z' ),
('81838e1d-bb46-40e3-ba43-4d325370a1bd', 'Amy', 'Oberst', 'aoberst@tpnrd.org', 1, '2021-05-25T21:01:16.930Z', 1, 0, 'aoberst@tpnrd.org', null ),
('40be5224-74bf-4801-821f-8223e967a5de', 'Bill', 'Carhart', 'bcarhart@tpnrd.org', 1, '2021-05-25T21:01:36.110Z', 1, 0, 'bcarhart@tpnrd.org', null ),
('34883308-0d86-4630-a94d-18b55ffb9608', 'Kent', 'Miller', 'komiller@tpnrd.org', 1, '2021-05-25T21:01:50.940Z', 1, 0, 'komiller@tpnrd.org', null ),
('ada9a505-8dd4-4621-a5b0-44d57fec1fd9', 'Billy', 'Tiller', 'billy@gisc.coop', 1, '2021-05-28T15:00:23.566Z', 1, 0, 'btiller', '2021-05-28T15:03:32.390Z' ),
('44bc6816-6003-4c85-828d-000ff7001d5f', 'Dave', 'Slattery', 'dlslattery1@tpnrd.org', 1, '2021-06-02T16:09:39.245Z', 1, 0, 'dlslattery1@tpnrd.org', null ),
('f1a562b8-68aa-4b3a-b84a-ba7ddca87aa8', 'Julie', 'Bushell', 'jbushell@paigewireless.com', 1, '2021-06-24T21:44:28.598Z', 1, 0, 'jbushell@paigewireless.com', null ),
('fe21238d-716e-414a-8db1-530a52f2feb2', 'Luke', 'Tiller', 'luke.tiller@gisc.coop', 1, '2021-06-25T14:06:54.893Z', 1, 1, 'luke.tiller', '2021-07-28T16:58:09.853Z' ),
('46a39c65-28de-4435-8439-7071f4e5313a', 'Jenny', 'Turner', 'jturner@tpnrd.org', 1, '2021-08-11T19:32:46.449Z', 1, 0, 'jturner@tpnrd.org', '2021-08-11T20:40:03.251Z' )

insert into dbo.[User] 
    ([UserGuid]
    ,[FirstName]
    ,[LastName]
    ,[Email]
    ,[RoleID]
    ,[CreateDate]
    ,[IsActive]
    ,[ReceiveSupportEmails]
    ,[LoginName]
    ,[DisclaimerAcknowledgedDate])

select 
    [UserGuid]
    ,[FirstName]
    ,[LastName]
    ,[Email]
    ,[RoleID]
    ,[CreateDate]
    ,[IsActive]
    ,[ReceiveSupportEmails]
    ,[LoginName]
    ,[DisclaimerAcknowledgedDate]
from #tempUsers tu where tu.UserGuid not in (select UserGuid from dbo.[User])

drop table #tempUSers