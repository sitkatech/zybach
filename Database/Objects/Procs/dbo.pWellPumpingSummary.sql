IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pWellPumpingSummary'))
    drop procedure dbo.pWellPumpingSummary
go

create procedure dbo.pWellPumpingSummary
(
	@startDate datetime,
	@endDate dateTime
)
as
begin
	
	select w.WellID, w.WellRegistrationID, w.OwnerName, 
		mrst.SupportTicketID as MostRecentSupportTicketID, mrst.SupportTicketTitle as MostRecentSupportTicketTitle,
		wst.HasFlowMeter, wst.HasContinuityMeter, wst.HasElectricalUsage,
		wps.FlowMeterPumpedVolume, wps.ContinuityMeterPumpedVolume, wps.ElectricalUsagePumpedVolume,
		wps.FlowMeterPumpedVolume - wps.ContinuityMeterPumpedVolume as FlowMeterContinuityMeterDifference,
		wps.FlowMeterPumpedVolume - wps.ElectricalUsagePumpedVolume as FlowMeterElectricalUsageDifference
	from dbo.Well w
	left join 
	(
		select WellID, SupportTicketID, SupportTicketTitle, rank() over(partition by WellID order by DateUpdated desc) as Ranking 
		from dbo.SupportTicket
	) mrst on w.WellID = mrst.WellID and mrst.Ranking = 1
	left join 
	(
		select WellID,
			max(case when SensorTypeID = 1 then 1 else 0 end) as HasFlowMeter,
			max(case when SensorTypeID = 2 then 1 else 0 end) as HasContinuityMeter,
			max(case when SensorTypeID = 4 then 1 else 0 end) as HasElectricalUsage
		from Sensor 
		group by WellID
	) wst on w.WellID = wst.WellID
	left join 
	(
		select wsm.WellRegistrationID,
			sum(case when wsm.MeasurementTypeID = 1 then wsm.MeasurementValue else null end) as FlowMeterPumpedVolume,
			sum(case when wsm.MeasurementTypeID = 2 then wsm.MeasurementValue else null end) as ContinuityMeterPumpedVolume,
			sum(case when wsm.MeasurementTypeID = 3 then wsm.MeasurementValue else null end) as ElectricalUsagePumpedVolume
		from
		(
			select WellRegistrationID, MeasurementTypeID, MeasurementValue, IsAnomalous, datefromparts(ReadingYear, ReadingMonth, ReadingDay) as ReadingDate
			from WellSensorMeasurement
		) wsm 
		where (IsAnomalous = 0 or IsAnomalous is null) and ReadingDate >= @startDate and ReadingDate <= @endDate
		group by wsm.WellRegistrationID
	) wps on w.WellRegistrationID = wps.WellRegistrationID
end

GO
