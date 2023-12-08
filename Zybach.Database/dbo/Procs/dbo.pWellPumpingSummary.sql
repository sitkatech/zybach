create procedure dbo.pWellPumpingSummary
(
	@startDate datetime,
	@endDate dateTime
)
as
begin

	select w.WellID, w.WellRegistrationID, w.OwnerName, agw.Acres,
		agw.WellAuditPumpRate, agw.RegisteredPumpRate, agw.WellTPNRDPumpRate,
		mrst.SupportTicketID as MostRecentSupportTicketID, mrst.SupportTicketTitle as MostRecentSupportTicketTitle,
		wst.FlowMeters, wst.ContinuityMeters, wst.ElectricalUsage,
		wps.FlowMeterPumpedVolumeGallons, wps.ContinuityMeterPumpedVolumeGallons, wps.ElectricalUsagePumpedVolumeGallons
	from dbo.Well w
	left join 
	(
		select WellID, SupportTicketID, SupportTicketTitle, DateUpdated, rank() over(partition by WellID order by DateUpdated desc) as Ranking 
		from dbo.SupportTicket
		where SupportTicketStatusID != 3
	) mrst on w.WellID = mrst.WellID and mrst.Ranking = 1
	left join (
		select aw.WellID, aw.WellAuditPumpRate, aw.RegisteredPumpRate, aw.WellTPNRDPumpRate, awia.Acres 
		from dbo.AgHubWell aw
		left join dbo.AgHubWellIrrigatedAcre awia on aw.AgHubWellID = awia.AgHubWellID
		where awia.IrrigationYear = year(getdate())
	) agw on w.WellID = agw.WellID
	left join 
	(
		select WellID,
			string_agg(case when SensorTypeID = 1 then SensorName else null end, ',') as FlowMeters,
			string_agg(case when SensorTypeID = 2 then SensorName else null end, ',') as ContinuityMeters,
			string_agg(case when SensorTypeID = 4 then SensorName else null end, ',') as ElectricalUsage
		from dbo.Sensor 
		where WellID is not null
		group by WellID
	) wst on w.WellID = wst.WellID
	left join 
	(
		select wsm.WellRegistrationID,
			sum(case when wsm.MeasurementTypeID = 1 then wsm.MeasurementValue else null end) as FlowMeterPumpedVolumeGallons,
			sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end) as ContinuityMeterPumpedVolumeGallons,
			sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end) as ElectricalUsagePumpedVolumeGallons
		from
		(
			select wsm.WellRegistrationID, wsm.MeasurementTypeID, wsm.MeasurementValue, wsm.IsAnomalous, 
				datefromparts(wsm.ReadingYear, wsm.ReadingMonth, wsm.ReadingDay) as ReadingDate,
				(case when s.RetirementDate is not null and s.RetirementDate < @endDate then s.RetirementDate else @endDate end) as EndDate
			from dbo.WellSensorMeasurement wsm
			join dbo.Sensor s on wsm.SensorName = s.SensorName
		) wsm 
		where (IsAnomalous = 0 or IsAnomalous is null) and ReadingDate >= @startDate and ReadingDate <= EndDate
		group by wsm.WellRegistrationID
	) wps on w.WellRegistrationID = wps.WellRegistrationID
end

GO
