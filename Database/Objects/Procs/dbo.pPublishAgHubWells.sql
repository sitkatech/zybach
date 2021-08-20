IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishAghubWells'))
    drop procedure dbo.pPublishAghubWells
go

create procedure dbo.pPublishAghubWells
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	truncate table dbo.AgHubWellIrrigatedAcre

	delete aw 
	from dbo.AgHubWell aw
	left join dbo.AgHubWellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID
	where aws.AgHubWellStagingID is null

	delete aw 
	from dbo.AgHubWell aw
	left join dbo.AgHubWellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID
	where aws.AgHubWellStagingID is null

	insert into dbo.AgHubWell(WellRegistrationID, WellTPID, WellGeometry, TPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, FetchDate)
	select	upper(aws.WellRegistrationID) as WellRegistrationID, 
			aws.WellTPID,
			aws.WellGeometry,
			aws.TPNRDPumpRate,
			aws.TPNRDPumpRateUpdated,
			aws.WellConnectedMeter,
			aws.WellAuditPumpRate,
			aws.AuditPumpRateUpdated,
			aws.HasElectricalData,
			@fetchDate as FetchDate
	from dbo.AgHubWellStaging aws
	left join dbo.AgHubWell aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.AgHubWellID is null

	update aw
	set aw.WellRegistrationID = upper(aws.WellRegistrationID),
		aw.WellTPID = aws.WellTPID,
		aw.WellGeometry = aws.WellGeometry,
		aw.TPNRDPumpRate = aws.TPNRDPumpRate,
		aw.TPNRDPumpRateUpdated = aws.TPNRDPumpRateUpdated,
		aw.WellConnectedMeter = aws.WellConnectedMeter,
		aw.WellAuditPumpRate = aws.WellAuditPumpRate,
		aw.AuditPumpRateUpdated = aws.AuditPumpRateUpdated,
		aw.HasElectricalData = aws.HasElectricalData,
		aw.FetchDate = @fetchDate
	from dbo.AgHubWell aw
	join dbo.AgHubWellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID

	update dbo.AgHubWell
	Set WellGeometry.STSrid = 4326

	insert into dbo.AgHubWellIrrigatedAcre(AgHubWellID, IrrigationYear, Acres)
	select	aw.AgHubWellID, 
			awias.IrrigationYear,
			avg(awias.Acres) as Acres
	from dbo.AgHubWellIrrigatedAcreStaging awias
	join dbo.AgHubWell aw on awias.WellRegistrationID = aw.WellRegistrationID
	group by aw.AgHubWellID, awias.IrrigationYear


	--select	aw.AgHubWellID, 
	--		awias.IrrigationYear,
	--		awias.Acres
	--from dbo.AgHubWellIrrigatedAcreStaging awias
	--join dbo.AgHubWell aw on awias.WellRegistrationID = awias.WellRegistrationID

	exec dbo.pPublishWellSensorMeasurementStaging 3 -- Electrical Usage

end

GO