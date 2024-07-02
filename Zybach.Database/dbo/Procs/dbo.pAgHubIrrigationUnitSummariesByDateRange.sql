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

	declare @acreInchesToGallonsConversionRate float = 27154.2857

	select ahiu.AgHubIrrigationUnitID,

		sum(ahiuoet.EvapotranspirationInchesForYear) as TotalEvapotranspirationInches, 
		sum(ahiuoet.PrecipitationInchesForYear) as TotalPrecipitationInches,
	
		sum(ahiuoet.EvapotranspirationInchesForYear * ahwiya.Acres * @acreInchesToGallonsConversionRate) as TotalEvapotranspirationGallons,
		sum(ahiuoet.PrecipitationInchesForYear * ahwiya.Acres * @acreInchesToGallonsConversionRate) as TotalPrecipitationGallons, 

		sum(ahiur.FlowMeterPumpedVolumeGallonsForYear) as FlowMeterPumpedVolumeGallonsTotal,
		sum(ahiur.ContinuityMeterPumpedVolumeGallonsForYear) as ContinuityMeterPumpedVolumeGallonsTotal,
		sum(ahiur.ElectricalUsagePumpedVolumeGallonsForYear) as ElectricalUsagePumpedVolumeGallonsTotal,

		sum(ahiur.FlowMeterPumpedVolumeGallonsForYear / ahwiya.Acres / @acreInchesToGallonsConversionRate) as FlowMeterPumpedDepthInchesTotal,
		sum(ahiur.ContinuityMeterPumpedVolumeGallonsForYear / ahwiya.Acres / @acreInchesToGallonsConversionRate) as ContinuityMeterPumpedDepthInchesTotal,
		sum(ahiur.ElectricalUsagePumpedVolumeGallonsForYear / ahwiya.Acres / @acreInchesToGallonsConversionRate) as ElectricalUsagePumpedDepthInchesTotal

	from dbo.AgHubIrrigationUnit ahiu

	left join 
	(
		select AgHubIrrigationUnitID, Year(ReportedDate) as ReportedYear,
			sum(case when OpenETDataTypeID = 1 then ReportedValueInches else 0 end) as EvapotranspirationInchesForYear,
			sum(case when OpenETDataTypeID = 2 then ReportedValueInches else 0 end) as PrecipitationInchesForYear
		from dbo.AgHubIrrigationUnitOpenETDatum 
		where ReportedDate >= @startDate and ReportedDate <= @endDate
		group by AgHubIrrigationUnitID, Year(ReportedDate)
	) ahiuoet on ahiuoet.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID

	left join 
	(
		select wps.AgHubIrrigationUnitID, wps.ReadingYear,
			sum(wps.FlowMeterPumpedVolumeGallons) as FlowMeterPumpedVolumeGallonsForYear,
			sum(wps.ContinuityMeterPumpedVolumeGallons) as ContinuityMeterPumpedVolumeGallonsForYear,
			sum(wps.ElectricalUsagePumpedVolumeGallons) as ElectricalUsagePumpedVolumeGallonsForYear
		from 
		(
			select ahiu.AgHubIrrigationUnitID, wsm.WellRegistrationID, wsm.ReadingYear,
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
			group by wsm.WellRegistrationID, ahiu.AgHubIrrigationUnitID, wsm.ReadingYear
		) wps
		group by wps.AgHubIrrigationUnitID, wps.ReadingYear
	) ahiur on ahiu.AgHubIrrigationUnitID = ahiur.AgHubIrrigationUnitID and ahiuoet.ReportedYear = ahiur.ReadingYear

	left join (
		select ahw.AgHubIrrigationUnitID, ahwia.IrrigationYear, max(ahwia.Acres) as Acres 
		from dbo.AgHubWell ahw
		join dbo.AgHubWellIrrigatedAcre ahwia on ahw.AgHubWellID = ahwia.AgHubWellID
		group by ahw.AgHubIrrigationUnitID, ahwia.IrrigationYear
	) ahwiya on ahiu.AgHubIrrigationUnitID = ahwiya.AgHubIrrigationUnitID and ahiur.ReadingYear = ahwiya.IrrigationYear
	group by ahiu.AgHubIrrigationUnitID

end

GO
