create procedure dbo.pAgHubIrrigationUnitSummariesByDateRange
(
	@startDateMonth int,
	@startDateYear int,
	@endDateMonth int,
	@endDateYear int
)
as
begin

	declare @startDate datetime
	declare @endDate datetime
	set @startDate = datefromparts(@startDateYear, @startDateMonth, 1)
	set @endDate = EOMONTH(datefromparts(@endDateYear, @endDateMonth, 1), 0)

	select ahiu.AgHubIrrigationUnitID,
		   ahiuoet.TotalEvapotranspirationInches,
		   ahiuoet.TotalPrecipitationInches,
		   ahiur.FlowMeterPumpedVolumeGallonsTotal, ahiur.ContinuityMeterPumpedVolumeGallonsTotal, ahiur.ElectricalUsagePumpedVolumeGallonsTotal
	from dbo.AgHubIrrigationUnit ahiu

	left join 
	(
		select AgHubIrrigationUnitID,
			sum(case when OpenETDataTypeID = 1 then ReportedValueInches else 0 end) as TotalEvapotranspirationInches,
			sum(case when OpenETDataTypeID = 2 then ReportedValueInches else 0 end) as TotalPrecipitationInches
		from dbo.AgHubIrrigationUnitOpenETDatum 
		where ReportedDate >= @startDate and ReportedDate <= @endDate
		group by AgHubIrrigationUnitID
	) ahiuoet on ahiuoet.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID

	left join 
	(
		select wps.AgHubIrrigationUnitID, 
			   sum(wps.FlowMeterPumpedVolumeGallons) as FlowMeterPumpedVolumeGallonsTotal,
			   sum(wps.ContinuityMeterPumpedVolumeGallons) as ContinuityMeterPumpedVolumeGallonsTotal,
			   sum(wps.ElectricalUsagePumpedVolumeGallons) as ElectricalUsagePumpedVolumeGallonsTotal
		from 
		(
			select ahiu.AgHubIrrigationUnitID,
				sum(case when wsm.MeasurementTypeID = 1 then wsm.MeasurementValue else null end) as FlowMeterPumpedVolumeGallons,
				sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end) as ContinuityMeterPumpedVolumeGallons,
				sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end) as ElectricalUsagePumpedVolumeGallons
			from
			(
				select wsm.WellRegistrationID, wsm.MeasurementTypeID, wsm.MeasurementValue, wsm.IsAnomalous,  wsm.ReadingYear, wsm.ReadingMonth,
					   datefromparts(wsm.ReadingYear, wsm.ReadingMonth, wsm.ReadingDay) as ReadingDate,
					   (case when s.RetirementDate is not null and s.RetirementDate < @endDate then s.RetirementDate else @endDate end) as EndDate
				from dbo.WellSensorMeasurement wsm
				join dbo.Sensor s on wsm.SensorName = s.SensorName
			) wsm 
			left join dbo.Well w on wsm.WellRegistrationID = w.WellRegistrationID
			left join dbo.AgHubWell ahw on w.WellID = ahw.WellID
			left join dbo.AgHubIrrigationUnit ahiu on ahw.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
			where (IsAnomalous = 0 or IsAnomalous is null) and ReadingDate >= @startDate and ReadingDate <= EndDate
			group by wsm.WellRegistrationID, ahiu.AgHubIrrigationUnitID
		) wps
		group by wps.AgHubIrrigationUnitID
	) ahiur on ahiu.AgHubIrrigationUnitID = ahiur.AgHubIrrigationUnitID

end

GO
