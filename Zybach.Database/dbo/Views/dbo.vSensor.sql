create View dbo.vSensor
as

select  s.SensorID, s.SensorName, s.SensorTypeID, senst.SensorTypeDisplayName as SensorTypeName, s.WellID, s.InGeoOptix, s.CreateDate, s.LastUpdateDate, s.IsActive
        , s.RetirementDate, s.ContinuityMeterStatusID, s.ContinuityMeterStatusLastUpdated, s.SnoozeStartDate
        , w.WellRegistrationID, w.PageNumber, w.OwnerName, w.TownshipRangeSection
        , flwsm.MeasurementTypeID, flwsm.FirstReadingDate, flwsm.LastReadingDate, flwsm.LatestMeasurementValue
        , volt.LastVoltageReadingDate, volt.LastVoltageReading
        , case when s.IsActive = 1 then datediff(hour, isnull(pwp.ReceivedDate, flwsm.LastReadingDate), GETUTCDATE()) else null end as LastMessageAgeInHours
        , st.SupportTicketID as MostRecentSupportTicketID, st.SupportTicketTitle as MostRecentSupportTicketTitle
from dbo.Sensor s
join dbo.SensorType senst on s.SensorTypeID = senst.SensorTypeID
left join dbo.Well w on s.WellID = w.WellID
left join dbo.vWellSensorMeasurementFirstAndLatestForSensor flwsm on s.SensorName = flwsm.SensorName
left join dbo.vSensorLatestBatteryVoltage volt on s.SensorName = volt.SensorName
left join dbo.PaigeWirelessPulse pwp on s.SensorName = pwp.SensorName
left join 
(
    select st.SupportTicketID, st.SensorID, st.SupportTicketTitle, st.SupportTicketStatusID, st.SupportTicketPriorityID, st.DateUpdated, row_number() over(partition by st.SensorID order by st.DateUpdated desc) as Ranking
    from dbo.SupportTicket st
    where st.SupportTicketStatusID !=  3 -- non resolved tickets
) st on s.SensorID = st.SensorID and st.Ranking = 1


go