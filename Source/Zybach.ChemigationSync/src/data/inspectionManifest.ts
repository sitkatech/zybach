import {InspectionManifest} from "../models"

const inspectionManifestMock: InspectionManifest =
{
	lastChangedDate: new Date("2021-05-20T19:26:29.429Z"),
	deleteOrphanedSamples: false,
	insertMissingWells: true,
	fieldAssignments: [
		{
			cname: "summer-2021-water-levels",
			name: "Summer 2021 Water Level Inspection",
			startDate: new Date("2021-06-01T00:00:00.000Z"),
			endDate: new Date("2021-09-15T00:00:00.000Z"),

			protocol: {
				cname: "water-level-inspection",
				version: 3
			},

			sampleNameTemplate: "WL-{{.Site.Properties.OwnerName}}-{{Year}}{{Month}}",

			sites: [
				{
					cname: "G-0000000",
					tags: ["gwmp-managed"],
					properties: {
						"ownerName": ["John Doe"]
					},
					latitude: -113.7348,
					longitude: 41.2301
				},
				{
					cname: "G-0000001",
					tags: ["gwmp-managed"],
					latitude: -113.7349,
					longitude: 41.2302
				}

			]
		}
	]
}

export default inspectionManifestMock;