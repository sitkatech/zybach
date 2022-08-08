IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pAgHubIrrigationUnitSummariesByDateRange'))
    drop procedure dbo.pAgHubIrrigationUnitSummariesByDateRange
go

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
		   ahiuwymed.TotalEvapotranspirationInches,
		   ahiuwympd.TotalPrecipitationInches,
		   ahiur.FlowMeterPumpedVolumeGallonsTotal, ahiur.ContinuityMeterPumpedVolumeGallonsTotal, ahiur.ElectricalUsagePumpedVolumeGallonsTotal
	from dbo.AgHubIrrigationUnit ahiu

	left join 
	(
		select wymed.AgHubIrrigationUnitID,
			sum(wymed.EvapotranspirationInches) as TotalEvapotranspirationInches
		from
		(
			select AgHubIrrigationUnitID, EvapotranspirationInches 
			from dbo.AgHubIrrigationUnitWaterYearMonthETDatum
			where WaterYearMonthID in 
			(
				select WaterYearMonthID 
				from dbo.WaterYearMonth wym
				where (wym.[Year] = @startDateYear and wym.[Month] >= @startDateMonth) or
					  (wym.[Year] = @endDateYear and wym.[Month] <= @endDateMonth) or
				      (wym.[Year] > @startDateYear and wym.[Year] < @endDateYear)
			)
		) wymed 
		group by wymed.AgHubIrrigationUnitID
	) ahiuwymed on ahiuwymed.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID

	left join 
	(
		select wympd.AgHubIrrigationUnitID,
			sum(wympd.PrecipitationInches) as TotalPrecipitationInches
		from
		(
			select AgHubIrrigationUnitID, PrecipitationInches 
			from dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum
			where WaterYearMonthID in 
			(
				select WaterYearMonthID 
				from dbo.WaterYearMonth wym
				where (wym.[Year] = @startDateYear and wym.[Month] >= @startDateMonth) or
						(wym.[Year] = @endDateYear and wym.[Month] <= @endDateMonth) or
						(wym.[Year] > @startDateYear and wym.[Year] < @endDateYear)
			)
		) wympd 
		group by wympd.AgHubIrrigationUnitID
	) ahiuwympd on ahiuwympd.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID

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
