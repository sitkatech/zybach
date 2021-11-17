delete wsm
from dbo.WellSensorMeasurement wsm
where wsm.MeasurementTypeID = 3 and wsm.WellRegistrationID in
(
	select WellRegistrationID
	from dbo.AgHubWell aw
	join dbo.Well w on aw.WellID = w.WellID
	where aw.HasElectricalData = 0
)