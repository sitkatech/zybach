import {InspectionManifest} from "../models"

const inspectionManifestMock: InspectionManifest =
{
	LastChangedDate: new Date("2021-05-20T19:26:29.429Z"),
	DeleteOrphanedSamples: true,
	InsertMissingWells: true,
	FieldAssignments: [
		{
			CanonicalName: "summer-2021-water-levels",
			Name: "Summer 2021 Water Level Inspection",
			StartDate: new Date("2021-06-01T00:00:00.000Z"),
			FinishDate: new Date("2021-09-15T00:00:00.000Z"),

			Protocol: {
				CanonicalName: "well-sensor-installation",
				Version: 5
			},

			Sites: [
				{
					CanonicalName: "G-0000000",
					Tags: ["gwmp-managed"],
					Properties: {
						"ownerName": ["John Doe"]
					},
					Latitude: -113.7348,
					Longitude: 41.2301
				},
				{
					CanonicalName: "G-0000002",
					Tags: ["gwmp-managed"],
					Latitude: -113.7349,
					Longitude: 41.2302
				}

			]
		}
	]
}

export default inspectionManifestMock;