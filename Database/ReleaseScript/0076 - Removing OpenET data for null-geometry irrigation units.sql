	--- clear out ET and precip data for IUs which will be deleted in updated PublishAgHubWells sproc
	delete ahiuwymed
	from dbo.AgHubIrrigationUnitWaterYearMonthETDatum ahiuwymed
	join AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiuwymed.AgHubIrrigationUnitID
	where ahiu.IrrigationUnitGeometry is null

	delete ahiuwympd
	from dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum ahiuwympd
	join AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiuwympd.AgHubIrrigationUnitID
	where ahiu.IrrigationUnitGeometry is null