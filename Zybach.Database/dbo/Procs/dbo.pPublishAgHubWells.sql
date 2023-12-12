create procedure dbo.pPublishAgHubWells
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	-- we are always getting all the yearly irrigated acres for a well so we can just truncate and rebuild
	truncate table dbo.AgHubWellIrrigatedAcre

	delete aw 
	from dbo.AgHubWell aw
	join dbo.Well w on aw.WellID = w.WellID
	left join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID
	where aws.AgHubWellStagingID is null

	insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate, RequiresChemigation, RequiresWaterLevelInspection, IsReplacement)
	select	upper(aws.WellRegistrationID) as WellRegistrationID, 
			aws.WellGeometry,
			@fetchDate as CreateDate,
			@fetchDate as LastUpdateDate,
			0 as RequiresChemigation,
			0 as RequiresWaterLevelInspection,
			0 as IsReplacement
	from dbo.AgHubWellStaging aws
	left join dbo.Well aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.WellID is null


	insert into dbo.AgHubWell(WellID, AgHubWellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, AgHubRegisteredUser, FieldName)
	select	w.WellID,
			aws.WellGeometry as AgHubWellGeometry,
			aws.WellTPNRDPumpRate,
			aws.TPNRDPumpRateUpdated,
			aws.WellConnectedMeter,
			aws.WellAuditPumpRate,
			aws.AuditPumpRateUpdated,
			aws.HasElectricalData,
			aws.RegisteredPumpRate,
			aws.RegisteredUpdated,
			aws.AgHubRegisteredUser,
			aws.FieldName
	from dbo.AgHubWellStaging aws
	join dbo.Well w on aws.WellRegistrationID = w.WellRegistrationID
	left join dbo.AgHubWell aw on w.WellID = aw.WellID
	where aw.AgHubWellID is null

	update aw
	set aw.AgHubWellGeometry = aws.WellGeometry,
		aw.WellTPNRDPumpRate = aws.WellTPNRDPumpRate,
		aw.TPNRDPumpRateUpdated = aws.TPNRDPumpRateUpdated,
		aw.WellConnectedMeter = aws.WellConnectedMeter,
		aw.WellAuditPumpRate = aws.WellAuditPumpRate,
		aw.AuditPumpRateUpdated = aws.AuditPumpRateUpdated,
		aw.HasElectricalData = aws.HasElectricalData,
		aw.RegisteredPumpRate = aws.RegisteredPumpRate,
		aw.RegisteredUpdated =aws.RegisteredUpdated,
		aw.AgHubRegisteredUser = aws.AgHubRegisteredUser,
		aw.FieldName = aws.FieldName
	from dbo.AgHubWell aw
	join dbo.Well w on aw.WellID = w.WellID
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID

	update w
	set LastUpdateDate = @fetchDate, WellGeometry.STSrid = 4326
	from dbo.Well w
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID

	update dbo.AgHubWell
	Set AgHubWellGeometry.STSrid = 4326, AgHubIrrigationUnitID = null

	-- add irrigation units
	-- first get the new list of possible AgHubWell Irrigation Units
	-- then do a merge (delete, insert, update)
	select distinct aws.IrrigationUnitGeometry.STAsText() as IrrigationUnitGeometry,
			aws.WellTPID
	into #agIrrigationUnits
	from dbo.AgHubWellStaging aws
	where aws.WellTPID is not null and aws.IrrigationUnitGeometry is not null

	delete ahiuoet
	from dbo.AgHubIrrigationUnitOpenETDatum ahiuoet
	join AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiuoet.AgHubIrrigationUnitID
	left join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID
	where ahiuNew.IrrigationUnitGeometry is null

	delete ahiug
	from dbo.AgHubIrrigationUnitGeometry ahiug
	join AgHubIrrigationUnit ahiu on ahiug.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
	left join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID
	where ahiuNew.IrrigationUnitGeometry is null

	delete ahiu
	from dbo.AgHubIrrigationUnit ahiu
	left join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID
	where ahiuNew.WellTPID is null

	insert into dbo.AgHubIrrigationUnit (WellTPID)
	select ahiuNew.WellTPID from #agIrrigationUnits ahiuNew
	left join dbo.AgHubIrrigationUnit ahiu on ahiuNew.WellTPID = ahiu.WellTPID
	where ahiu.AgHubIrrigationUnitID is null

	insert into dbo.AgHubIrrigationUnitGeometry (AgHubIrrigationUnitID, IrrigationUnitGeometry)
	select ahiu.AgHubIrrigationUnitID, ahiuNew.IrrigationUnitGeometry from #agIrrigationUnits ahiuNew
	join dbo.AgHubIrrigationUnit ahiu on ahiuNew.WellTPID = ahiu.WellTPID
	left join dbo.AgHubIrrigationUnitGeometry ahiug on ahiu.AgHubIrrigationUnitID = ahiug.AgHubIrrigationUnitID
	where ahiug.AgHubIrrigationUnitGeometryID is null 

	update ahiug
	set ahiug.IrrigationUnitGeometry = ahiuNew.IrrigationUnitGeometry
	from dbo.AgHubIrrigationUnitGeometry ahiug
	join AgHubIrrigationUnit ahiu on ahiug.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
	join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID

	update dbo.AgHubIrrigationUnitGeometry
	Set IrrigationUnitGeometry.STSrid = 4326
	WHERE IrrigationUnitGeometry is not null
	
	update dbo.AgHubWell
	set AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
	from dbo.AgHubWell ahw
	join dbo.Well w on ahw.WellID = w.WellID
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID
	left join dbo.AgHubIrrigationUnit ahiu on aws.WellTPID = ahiu.WellTPID
	
	insert into dbo.AgHubWellIrrigatedAcre(AgHubWellID, IrrigationYear, Acres, CropType, Tillage)
	select	aw.AgHubWellID, 
			awias.IrrigationYear,
			avg(awias.Acres) as Acres,
            awias.CropType,
            awias.Tillage
	from dbo.AgHubWellIrrigatedAcreStaging awias
	join dbo.Well w on awias.WellRegistrationID = w.WellRegistrationID
	join dbo.AgHubWell aw on w.WellID = aw.WellID
	group by aw.AgHubWellID, awias.IrrigationYear, awias.CropType, awias.Tillage

	-- pulling latest acres straight from AgHubWellIrrigatedAcre to ahiu
	update dbo.AgHubIrrigationUnit
	set IrrigationUnitAreaInAcres = 
	(
		select TOP 1 awia.Acres
		FROM dbo.AgHubWellIrrigatedAcre awia 
		join dbo.AgHubWell ahw on awia.AgHubWellID = ahw.AgHubWellID and dbo.AgHubIrrigationUnit.AgHubIrrigationUnitID = ahw.AgHubIrrigationUnitID
		ORDER BY IrrigationYear DESC
	)

	-- Set StreamflowZoneID; first "reset" it to null; then actually calculate matching ones
	update dbo.Well
	Set StreamflowZoneID = null

	update aw
	set StreamflowZoneID = sfz.StreamFlowZoneID
	from dbo.StreamFlowZone sfz
	join dbo.Well aw on aw.WellGeometry.STWithin(sfz.StreamFlowZoneGeometry) = 1


    insert into dbo.Sensor(SensorName, SensorTypeID, CreateDate, LastUpdateDate, InGeoOptix, IsActive, WellID)
    select	concat('E-', upper(w.WellRegistrationID)) as SensorName, 
		    4 as SensorTypeID,
		    getutcdate() as CreateDate,
		    getutcdate() as LastUpdateDate,
		    0 as InGeoOptix,
		    1 as IsActive,
            w.WellID
    from dbo.AgHubWell aw
    join dbo.Well w on aw.WellID = w.WellID
    left join dbo.Sensor s on concat('E-', upper(w.WellRegistrationID)) = s.SensorName
    where aw.HasElectricalData = 1 and s.SensorID is null

    -- we need to mark electrical usage sensors as inactive if no longer a wellconnectedmeter
    update s
    set s.IsActive = 0, s.LastUpdateDate = getutcdate()
    from dbo.Sensor s 
    left join 
    (
        select concat('E-', upper(w.WellRegistrationID)) as SensorName
        from dbo.AgHubWell aw
        join dbo.Well w on aw.WellID = w.WellID
        where aw.HasElectricalData = 1
    ) a on s.SensorName = a.SensorName
    where s.SensorTypeID = 4 and a.SensorName is null

    -- we need to mark electrical usage sensors as active if they are a wellconnectedmeter
    update s
    set s.IsActive = 1, s.LastUpdateDate = getutcdate()
    from dbo.Sensor s 
    join 
    (
        select concat('E-', upper(w.WellRegistrationID)) as SensorName
        from dbo.AgHubWell aw
        join dbo.Well w on aw.WellID = w.WellID
        where aw.HasElectricalData = 1
    ) a on s.SensorName = a.SensorName
    where s.SensorTypeID = 4 and s.IsActive = 0

    --set the well id again just in case it got unhooked
    update s
    set s.WellID = w.WellID
    from dbo.Sensor s 
    join dbo.Well w on s.SensorName = concat('E-', upper(w.WellRegistrationID))
    where s.SensorTypeID = 4


    -- finally create sensors with SensorY
    exec dbo.pPublishWellSensorMeasurementStaging

end

GO