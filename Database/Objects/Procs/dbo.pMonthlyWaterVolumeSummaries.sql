IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pMonthlyWaterVolumeSummaries'))
    drop procedure dbo.pMonthlyWaterVolumeSummaries
go

create procedure dbo.pMonthlyWaterVolumeSummaries
as
begin

	declare @gallonsToAcreFeetConversion decimal = 325851.0

	select cx.AgHubIrrigationUnitID, cx.[Year], cx.[Month],
		   ahiuwymed.EvapotranspirationAcreInches / 12 as EvapotranspirationAcreFeet, 
		   ahiuwympd.PrecipitationAcreInches / 12 as PrecipitationAcreFeet,
		   wps.ContinuityMeterPumpedVolumeAcreFeet, wps.ElectricalUsagePumpedVolumeAcreFeet, wps.FlowMeterPumpedVolumeAcreFeet
	from 
	(
		select ahiu.AgHubIrrigationUnitID, wym.[Year], wym.[Month], wym.WaterYearMonthID
		from dbo.WaterYearMonth wym
		cross join dbo.AgHubIrrigationUnit ahiu
	) cx

	left join dbo.AgHubIrrigationUnitWaterYearMonthETDatum ahiuwymed 
		on cx.AgHubIrrigationUnitID = ahiuwymed.AgHubIrrigationUnitID and 
		   cx.WaterYearMonthID = ahiuwymed.WaterYearMonthID
	left join dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum ahiuwympd
		on cx.AgHubIrrigationUnitID = ahiuwympd.AgHubIrrigationUnitID and 
		   cx.WaterYearMonthID = ahiuwympd.WaterYearMonthID
	left join 
	(
		select wsm.WellRegistrationID, wsm.ReadingYear, wsm.ReadingMonth, ahiu.AgHubIrrigationUnitID,
			sum(case when wsm.MeasurementTypeID = 1 then wsm.MeasurementValue else null end) / @gallonsToAcreFeetConversion as FlowMeterPumpedVolumeAcreFeet,
			sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end) / @gallonsToAcreFeetConversion as ContinuityMeterPumpedVolumeAcreFeet,
			sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end) / @gallonsToAcreFeetConversion as ElectricalUsagePumpedVolumeAcreFeet
		from
		(
			select wsm.WellRegistrationID, wsm.MeasurementTypeID, wsm.MeasurementValue, wsm.IsAnomalous, wsm.ReadingYear, wsm.ReadingMonth
			from dbo.WellSensorMeasurement wsm
		) wsm 
		left join dbo.Well w on wsm.WellRegistrationID = w.WellRegistrationID
		left join dbo.AgHubWell ahw on w.WellID = ahw.WellID
		left join dbo.AgHubIrrigationUnit ahiu on ahw.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
		where (IsAnomalous = 0 or IsAnomalous is null)		
		group by wsm.WellRegistrationID, wsm.ReadingYear, wsm.ReadingMonth, ahiu.AgHubIrrigationUnitID
	) wps on cx.[Year] = wps.ReadingYear and 
			 cx.[Month] = wps.ReadingMonth and
			 cx.AgHubIrrigationUnitID = wps.AgHubIrrigationUnitID
	
end

GO
