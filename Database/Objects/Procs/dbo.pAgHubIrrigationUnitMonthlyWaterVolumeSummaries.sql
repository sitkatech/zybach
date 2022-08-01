IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries'))
    drop procedure dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries
go

create procedure dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries
as
begin

	declare @gallonsToAcreFeetConversion decimal = 325851.0

	select cx.AgHubIrrigationUnitID, cx.[Year], cx.[Month],
		   ahiuwymed.EvapotranspirationAcreInches / 12 as EvapotranspirationAcreFeet, 
		   ahiuwympd.PrecipitationAcreInches / 12 as PrecipitationAcreFeet,
		   wps.PumpedVolumeAcreFeet
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
			   PumpedVolumeAcreFeet = 
				case ahw.WellConnectedMeter
					when 1 then sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end) / @gallonsToAcreFeetConversion
					when 0 then sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end) / @gallonsToAcreFeetConversion 
					else null
				end
		from
		(
			select wsm.WellRegistrationID, wsm.MeasurementTypeID, wsm.MeasurementValue, wsm.IsAnomalous, wsm.ReadingYear, wsm.ReadingMonth
			from dbo.WellSensorMeasurement wsm
		) wsm 
		left join dbo.Well w on wsm.WellRegistrationID = w.WellRegistrationID
		left join dbo.AgHubWell ahw on w.WellID = ahw.WellID
		left join dbo.AgHubIrrigationUnit ahiu on ahw.AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
		where (IsAnomalous = 0 or IsAnomalous is null)		
		group by wsm.WellRegistrationID, wsm.ReadingYear, wsm.ReadingMonth, ahiu.AgHubIrrigationUnitID, ahw.WellConnectedMeter
	) wps on cx.[Year] = wps.ReadingYear and 
			 cx.[Month] = wps.ReadingMonth and
			 cx.AgHubIrrigationUnitID = wps.AgHubIrrigationUnitID

end

GO
