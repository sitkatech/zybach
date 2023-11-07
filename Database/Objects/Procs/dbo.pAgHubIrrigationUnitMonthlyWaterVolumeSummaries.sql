IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries'))
    drop procedure dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries
go

create procedure dbo.pAgHubIrrigationUnitMonthlyWaterVolumeSummaries
as
begin

	declare @gallonsToAcreFeetConversion decimal = 325851.0

	select cx.AgHubIrrigationUnitID, cx.[Year], cx.[Month],
		   ahiuoed.EvapotranspirationAcreFeet, ahiuoed.PrecipitationAcreFeet,
		   isnull(wps.PumpedVolumeAcreFeetElectricalUsage, wps.PumpedVolumeAcreFeetContinuity) / @gallonsToAcreFeetConversion as PumpedVolumeAcreFeet
	from 
	(
		select ahiu.AgHubIrrigationUnitID, oes.[Year], oes.[Month], oes.OpenETSyncID
		from dbo.OpenETSync oes
		cross join dbo.AgHubIrrigationUnit ahiu
	) cx

	left join (
		select AgHubIrrigationUnitID, Year(ReportedDate) as [Year], Month(ReportedDate) as [Month],
		sum(case when OpenETDataTypeID = 1 then ReportedValueInches else 0 end) / 12 as EvapotranspirationAcreFeet,
		sum(case when OpenETDataTypeID = 2 then ReportedValueInches else 0 end) / 12 as PrecipitationAcreFeet
		from dbo.AgHubIrrigationUnitOpenETDatum  
		group by AgHubIrrigationUnitID, ReportedDate
	) ahiuoed on cx.AgHubIrrigationUnitID = ahiuoed.AgHubIrrigationUnitID and 
		   cx.[Year] = ahiuoed.[Year] and cx.[Month] = ahiuoed.[Month]

	left join 
	(
		select wsm.WellRegistrationID, wsm.ReadingYear, wsm.ReadingMonth, ahiu.AgHubIrrigationUnitID, 
			   PumpedVolumeAcreFeetElectricalUsage = sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end),
			   PumpedVolumeAcreFeetContinuity = sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end)
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
