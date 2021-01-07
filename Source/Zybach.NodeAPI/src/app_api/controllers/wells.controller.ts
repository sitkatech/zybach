import {
    Controller,
    Get,
    Path,
    Query,
    Route,
    Security
} from "tsoa";
import secrets from '../../secrets';
import { ApiError } from '../../errors/apiError'
import { GeoOptixTokenService } from '../services/geo-optix-token-service';
import axios from 'axios';
import moment from 'moment';
import { InfluxDB } from '@influxdata/influxdb-client'
import { SecurityType } from "../security/authentication";

const bucketName = process.env.SOURCE_BUCKET;

@Route("/api/wells")
export class WellController extends Controller {

    @Get("")
    @Security(SecurityType.API_KEY)
    //@Response<ApiError>('500', 'Internal Server Error')
    public async getWells() {
        try {
            // todo: this getting stuff from GeoOptix needs to live in GeoOptixService class
            let geoOptixAccessToken = await new GeoOptixTokenService().getGeoOptixAccessToken();
            const geoOptixRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites`, {
                headers: {
                    "Authorization": `Bearer ${geoOptixAccessToken}`
                }
            });

            return {
                "status": "success",
                "result": geoOptixRequest.data.map((x: { CanonicalName: any; Description: any; Location: any; }) => ({ wellRegistrationID: x.CanonicalName, description: x.Description, location: x.Location }))
            }
        }
        catch (err) {
            console.error(err);
            throw new ApiError("Internal Server Error", 500, err.message);
        }
    }

    @Get("pumpedVolume")
    @Security(SecurityType.API_KEY)
    public async getPumpedVolume(
        @Query("startDate") startDateString: string,
        @Query("filter") wellRegistrationIDs?: string[],
        @Query("endDate") endDateString?: string,
        @Query() interval?: number
    ) {
        return this.getPumpedVolumeImpl(startDateString, wellRegistrationIDs, endDateString, interval)
    }

    @Get("{wellRegistrationID}")
    @Security(SecurityType.API_KEY)
    public async getWell(
        @Path() wellRegistrationID: string
    ) {
        try {
            const geoOptixAccessToken = await new GeoOptixTokenService().getGeoOptixAccessToken();
            const geoOptixWellRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}`, {
                headers: {
                    "Authorization": `Bearer ${geoOptixAccessToken}`
                }
            });
            let resultsObject = abbreviateWellDataResponse(geoOptixWellRequest.data);
            const geoOptixWellSensorsRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}/stations`, {
                headers: {
                    "Authorization": `Bearer ${geoOptixAccessToken}`
                }
            });
            resultsObject.sensors = abbreviateWellSensorsResponse(geoOptixWellSensorsRequest.data);
            return {
                "status": "success",
                "result": resultsObject
            };
        }
        catch (err) {
            console.error(err);
            throw new ApiError("Internal Server Error", 500, err.message);
        }
    }

    @Get("{wellRegistrationID}/pumpedVolume")
    @Security(SecurityType.API_KEY)
    public async getPumpedVolumeByWell(
        @Query("startDate") startDateString: string,
        @Path() wellRegistrationID?: string,
        @Query("endDate") endDateString?: string,
        @Query() interval?: number) {
        return this.getPumpedVolumeImpl(startDateString, wellRegistrationID, endDateString, interval)
    }

    public async getPumpedVolumeImpl(
        startDateString: string,
        wellRegistrationIDs?: string | string[],
        endDateString?: string,
        interval?: number
    ) {
        if (!endDateString) {
            endDateString = moment(new Date()).format();
        }
        if (!interval) {
            interval = 60;
        }
        [{ name: "Start Date", value: startDateString },
        { name: "End Date", value: endDateString }].forEach(x => {
            const value = x.value as string;
            if (x.value === null || x.value === undefined) {
                throw new ApiError("Invalid Request", 400, `${x.name} empty. Please enter a valid ${x.name}.`)
            }

            if (!moment(value, "YYYY-MM-DD", true).isValid() && !moment(value, "YYYYMMDD", true).isValid()
                && !moment(value, "YYYYMMDDTHHmmssZ", true).isValid() && !moment(value, "YYYY-MM-DDTHH:mm:ssZ", true).isValid()
            ) {
                throw new ApiError("Invalid Request", 400, `${x.name} is not a valid Date string in ISO 8601 format. Please enter a valid date string`)
            }
        });

        if (isNaN(interval)) {
            throw new ApiError("Invalid Request", 400, "Interval is invalid. Please enter an integer evenly divisible by 15 to use for interval.");
        }

        if (interval === 0 || interval % 15 != 0) {
            throw new ApiError("Invalid Request", 400, "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval.")
        }

        const startDate = moment(startDateString as string).toDate();
        const endDate = moment(endDateString as string).toDate();

        if (startDate > endDate) {
            throw new ApiError("Invalid Request", 400, "Start date occurs after End date. Please ensure that Start Date occurs before End date");
        }

        try {
            let results = await getFlowMeterSeries(wellRegistrationIDs as string | string[], startDate, endDate);
            return {
                "status": "success",
                "result": results.length > 0 ? structureResults(results, interval) : results
            }
        }
        catch (err) {
            console.error(err);
            throw new ApiError("Internal Server Error", 500, err.message);
        }
    }
}

// TODO: these methods all need better typing.

function abbreviateWellDataResponse(wellData: { CanonicalName: any; Description: any; Tags: any; Location: any; CreateDate: any; UpdateDate: any; }): AbbreviatedWellDataResponse {
    return {
        wellRegistrationID: wellData.CanonicalName,
        description: wellData.Description,
        tags: wellData.Tags,
        location: wellData.Location,
        createDate: wellData.CreateDate,
        updateDate: wellData.UpdateDate,
        sensors: []
    }
}

function abbreviateWellSensorsResponse(wellSensors: { CanonicalName: any, Definition: { sensorType: any } }[]) {
    return wellSensors.map(x => ({
        sensorName: x.CanonicalName,
        sensorType: x.Definition.sensorType
    }));
}


class ResultFromInfluxDB {
    endTime!: Date;
    gallons!: number;
    wellRegistrationID!: string;
}

async function getFlowMeterSeries(wellRegistrationIDs: string | string[], startDate: Date, endDate: Date): Promise<ResultFromInfluxDB[]> {
    const token = secrets.INFLUXDB_TOKEN;
    const org = secrets.INFLUXDB_ORG;
    const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: token });
    const queryApi = client.getQueryApi(org);

    const fifteenMinutesInms = 1000 * 60 * 15;
    const startDateForFlux = new Date((Math.round(startDate.getTime() / fifteenMinutesInms) * fifteenMinutesInms) + 1000).toISOString();
    const endDateForFlux = new Date((Math.round(endDate.getTime() / fifteenMinutesInms) * fifteenMinutesInms) + 1000).toISOString();

    const registrationIDQuery = wellRegistrationIDs !== null && wellRegistrationIDs != undefined ? `and r["registration-id"] == "${Array.isArray(wellRegistrationIDs) ? wellRegistrationIDs.join(`" or r["registration-id"]=="`) : wellRegistrationIDs}"` : "";
    const query = `from(bucket: "${bucketName}") \
        |> range(start: ${startDateForFlux}, stop:${endDateForFlux}) \
        |> filter(fn: (r) => 
            r["_measurement"] == "pumped-volume" and \
            r["_field"] == "gallons" \
            ${registrationIDQuery}) \
        |> sort(columns:["_time"])`;

    let results: ResultFromInfluxDB[] = [];

    return new Promise((resolve, reject) => {
        queryApi.queryRows(query, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                results.push({ endTime: new Date(o._time), gallons: o._value, wellRegistrationID: o['registration-id'] });
            },
            error(error) {
                reject(error);
            },
            complete() {
                resolve(results);
            },
        });
    });
}

function structureResults(results: ResultFromInfluxDB[], interval: number): StructuredResults {
    const distinctWells = [...new Set(results.map(x => x.wellRegistrationID))];
    let startDate = results[0].endTime;
    let endDate = results[results.length - 1].endTime;
    let totalResults = 0;
    let volumesByWell: VolumeByWell[] = [];
    distinctWells.forEach(wellRegistrationID => {
        let currentWellResults = results.filter(x => x.wellRegistrationID === wellRegistrationID).sort((a, b) => a.endTime.getTime() - b.endTime.getTime());

        if (currentWellResults[0].endTime < startDate) {
            startDate = currentWellResults[0].endTime;
        }

        let aggregatedResults = aggregateResults(currentWellResults, interval);

        if (aggregatedResults[aggregatedResults.length - 1].endTime > endDate) {
            endDate = aggregatedResults[aggregatedResults.length - 1].endTime;
        }

        totalResults += aggregatedResults.length;

        let newWellObj = {
            wellRegistrationID: wellRegistrationID,
            intervalCount: aggregatedResults.length,
            intervalVolumes: aggregatedResults
        };
        volumesByWell.push(newWellObj);
    });

    //Because we get the intervals back in 15 minute increments, technically our startDate is 15 minutes BEFORE our actual first time
    //Remove this extra piece if we decide we just want the first interval's end date
    startDate = new Date(startDate.getTime() - (15 * 60000));

    return {
        intervalCountTotal: totalResults,
        intervalWidthInMinutes: interval,
        intervalStart: startDate.toISOString(),
        intervalEnd: endDate.toISOString(),
        durationInMinutes: Math.round((endDate.getTime() - startDate.getTime()) / 60000),
        wellCount: distinctWells.length,
        volumesByWell: volumesByWell
    }
}

class VolumeByWell {
    wellRegistrationID!: string;
    intervalCount!: number;
    intervalVolumes!: AggregatedResult[];
}

class StructuredResults {
    intervalCountTotal!: number;
    intervalWidthInMinutes!: number;
    intervalStart!: string;
    intervalEnd!: string;
    durationInMinutes!: number;
    wellCount!: number;
    volumesByWell!: VolumeByWell[];
}

function aggregateResults(resultsToAggregate: ResultFromInfluxDB[], interval: number): AggregatedResult[] {
    let aggregatedResults = [];
    let sum = 0;
    //Again, because of the 15 minute intervals the first date we get will have been over a previous 15 minute interval
    //So, when we start aggregating, we need to have our first start time 15 minutes BEFORE our first endTime
    let startTime = new Date(resultsToAggregate[0].endTime.getTime() - (15 * 60000));
    let timeThreshold = interval * 60 * 1000;
    resultsToAggregate.forEach(x => {
        sum += Math.round(x.gallons);
        if (x.endTime.getTime() - startTime.getTime() >= timeThreshold) {
            aggregatedResults.push({
                intervalEndTime: x.endTime,
                volumePumpedGallons: sum
            })
            sum = 0;
            startTime = x.endTime;
        }
    })

    //TODO:If we have an incomplete interval, do we want to push it? 
    //Or should we leave incomplete intervals out of the payload? 
    //Should there be a marker stating that it's incomplete?
    if (sum > 0) {
        aggregatedResults.push({
            wellRegistrationID: resultsToAggregate[resultsToAggregate.length - 1].wellRegistrationID,
            endTime: resultsToAggregate[resultsToAggregate.length - 1].endTime,
            gallons: sum
        })
    }

    return aggregatedResults;
}

class AggregatedResult {
    wellRegistrationID!: string;
    endTime!: Date;
    gallons!: number;
}

// todo: these types need a home

class AbbreviatedWellDataResponse {
    wellRegistrationID!: string;
    description!: string;
    tags: any;
    location: any;
    createDate: any;
    updateDate: any;
    sensors!: any[];

}
