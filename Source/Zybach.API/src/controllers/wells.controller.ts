import {
    Controller,
    Get,
    Path,
    Query,
    Route,
    Security,
    Response,
    Request,
    SuccessResponse,
    Hidden
} from "tsoa";
import secrets from '../secrets';
import { ApiError } from '../errors/apiError'
import axios from 'axios';
import moment from 'moment';
import { InfluxDB } from '@influxdata/influxdb-client'
import { SecurityType } from "../security/authentication";
import { SensorTypeMap, WellDetailDto, WellSummaryDto, WellWithSensorSummaryDto } from "../dtos/well-summary-dto";
import { ApiResult, ErrorResult } from "../dtos/api-result";
import { GeoOptixService } from "../services/geooptix-service";
import { InternalServerError } from "../errors/internal-server-error";
import { provideSingleton } from "../util/provide-singleton";
import { inject } from "inversify";
import { RequestWithUserContext } from "../request-with-user-context";
import { RoleEnum } from "../models/role";
import { AghubWellService } from "../services/aghub-well-service";
import { InfluxService } from "../services/influx-service";
import { AnnualPumpedVolumeDto } from "../dtos/annual-pumped-volume-dto";
import { RobustReviewDto, MonthlyPumpedVolumeGallonsDto} from "../dtos/robust-review-dto";
import { NotFoundError } from "../errors/not-found-error";

const bucketName = process.env.SOURCE_BUCKET;

@Route("/api/wells")
@provideSingleton(WellController)
export class WellController extends Controller {
    constructor(
        @inject(GeoOptixService) private geooptixService: GeoOptixService,
        @inject(AghubWellService) private aghubWellService: AghubWellService,
        @inject(InfluxService) private influxService: InfluxService,
    ) {
        super();
    }

    /**
     * Returns an array of all Wells in the Water Data Program registered in GeoOptix
     */
    @Get("")
    @Security(SecurityType.API_KEY)
    @SuccessResponse(200, "Returns a list of all wells")
    @Response<ErrorResult>(401, "Unauthorized to perform request")
    @Response<ErrorResult>(403, "Forbidden")
    @Response<ErrorResult>(500, "If something went wrong within the API")
    public async getWells(): Promise<ApiResult<WellSummaryDto[]>> {
        const wellSummaryDtos = await this.geooptixService.getWellSummaries();

        return {
            "status": "success",
            "result": wellSummaryDtos
        };
    }

    /**
     * Returns a time series representing pumped volume at a well or series of wells, summed on a chosen reporting interval,
     * for a given date range. Each point in the output time series represents the total pumped volume over the previous
     * reporting interval.
     * @param wellRegistrationIDs The Well Registration ID(s) for the requested Well(s). If left blank, will bring back data
     * for every Well that has reported data within the time range.
     * @param startDateString The start date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
     * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00)
     * @param endDateString The end date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
     * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00). Default's to today's date.
     * @param interval The reporting interval, in minutes. Defaults to 60.
     */
    @Get("pumpedVolume")
    @Security(SecurityType.API_KEY)
    @SuccessResponse(200, "Returns the requested time series")
    @Response<ErrorResult>(400, "If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)")
    @Response<ErrorResult>(401, "Unauthorized to perform request")
    @Response<ErrorResult>(403, "Forbidden")
    @Response<ErrorResult>(500, "If something went wrong within the API")
    public async getPumpedVolume(
        @Query("startDate") startDateString: string,
        @Query("filter") wellRegistrationIDs?: string[],
        @Query("endDate") endDateString?: string,
        @Query() interval?: number
    ) {
        return this.getPumpedVolumeImpl(startDateString, wellRegistrationIDs, endDateString, interval)
    }

    @Get("/download/robustReviewScenarioJson")
    @Hidden()
    @Security(SecurityType.API_KEY)
    public async getRobustReviewJsonFile(
        @Request() req: RequestWithUserContext
    ) {
        const wells = await this.geooptixService.getWellsWithSensors();
        const aghubWells = await this.aghubWellService.getAghubWells();

        aghubWells.forEach(x=> {
            const geoOptixWell = wells.get(x.wellRegistrationID);
            if (!geoOptixWell){
                wells.set(x.wellRegistrationID, x);
                return;
            }

            geoOptixWell.sensors = [...geoOptixWell.sensors, ...x.sensors];
            geoOptixWell.wellTPID = x.wellTPID;
        });

        let sensorType = "Continuity Meter";

        const robustReviewStructure = await Promise.all(([...wells.values()].filter(x => x.hasElectricalData || x.sensors.some(x => x.sensorType == sensorType))).map(async x => {
            const firstReadingDate = await this.influxService.getFirstReadingDateTimeForWell(x.wellRegistrationID);

            if (!firstReadingDate) {
                return null;
            }

            let monthlyPumpedVolume:MonthlyPumpedVolumeGallonsDto[] = [];
            let dataSource = "";
            if (x.hasElectricalData) {
                monthlyPumpedVolume = [...await this.influxService.getMonthlyElectricalBasedFlowEstimate(x.wellRegistrationID, firstReadingDate)];
                dataSource = "Electrical Usage";
            }
            else {
                monthlyPumpedVolume = [...await this.influxService.getMonthlyPumpedVolumeForSensor(x.sensors.filter(y => y.sensorType == sensorType), x.wellRegistrationID, firstReadingDate)];
                // monthlyPumpedVolume = [...initialSensorRates.reduce((r, o) => {
                //     const key = o.month + '-' + o.year;
                    
                //     const item = r.get(key) || Object.assign({}, o, {
                //       volumePumpedGallons: 0
                //     });
                    
                //     item.volumePumpedGallons += o.volumePumpedGallons
                  
                //     return r.set(key, item);
                //   }, new Map).values()];
            
                dataSource = sensorType;
            }

            const returnObj: RobustReviewDto = {
                wellRegistrationID: x.wellRegistrationID,
                wellTPID: x.wellTPID,
                lat: x.location.geometry.coordinates[1],
                long: x.location.geometry.coordinates[0],
                dataSource: dataSource,
                monthlyPumpedVolumeGallons: monthlyPumpedVolume
            }

            return returnObj;
        }))
        
        if (robustReviewStructure) {
            req.res?.writeHead(200, {
                'Content-Type': 'application/octet-stream',
                "Content-Disposition": "attachment; filename=\"RobustReviewScenario.json\""
            });
            //We'll have null objects if they don't have a first reading date, and I'd rather 
            //do a second filter here than include the firstReadingDate in above filter and then have to get it a second time
            req.res?.end(JSON.stringify(robustReviewStructure.filter(x => x != null)));
        } else {
            req.res?.status(204).end();
        }
    }

    /**
     * Returns Well details from GeoOptix for a given well along with an array of sensors that have been associated
     * with that Well.
     * @param wellRegistrationID The Well Registration ID for the requested Well
     */
    @Get("{wellRegistrationID}")
    @SuccessResponse(200, "Returns a Well detail object which includes an array of associated sensors")
    @Security(SecurityType.API_KEY)
    @Response<ErrorResult>(400, "If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)")
    @Response<ErrorResult>(401, "Unauthorized to perform request")
    @Response<ErrorResult>(403, "Forbidden")
    @Response<ErrorResult>(500, "If something went wrong within the API")
    public async getWell(
        @Path() wellRegistrationID: string
    ) {
        try {
            const geoOptixWellRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}`, {
                headers: {
                    "x-geooptix-token": secrets.GEOOPTIX_API_KEY
                }
            });
            let resultsObject = abbreviateWellDataResponse(geoOptixWellRequest.data);
            const geoOptixWellSensorsRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}/stations`, {
                headers: {
                    "x-geooptix-token": secrets.GEOOPTIX_API_KEY
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
            throw new InternalServerError(err.message);
        }
    }

    @Get("{wellRegistrationID}/details")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getWellDetails(
        @Path() wellRegistrationID: string
    ): Promise<WellDetailDto> {
        let geooptixWell = await this.geooptixService.getWellSummary(wellRegistrationID);
        const agHubWell = await this.aghubWellService.findByWellRegistrationID(wellRegistrationID);
        const hasElectricalData = agHubWell ? agHubWell.hasElectricalData : false;

        if (!geooptixWell && !agHubWell){
            throw new NotFoundError("Well not found");
        }

        let well: WellDetailDto = (geooptixWell || agHubWell) as WellDetailDto;

        const firstReadingDate = await this.influxService.getFirstReadingDateTimeForWell(wellRegistrationID);
        if (firstReadingDate) {
            firstReadingDate.setHours(0);
            firstReadingDate.setMinutes(0);
            firstReadingDate.setSeconds(0);
            firstReadingDate.setMilliseconds(0);
        }
        const lastReadingDate = await this.influxService.getLastReadingDateTimeForWell(wellRegistrationID);

        if (geooptixWell) {
            well.inGeoOptix = true;
            well.wellTPID = agHubWell?.wellTPID;
            if (agHubWell) {
                well.location = agHubWell.location;
                well.irrigatedAcresPerYear = agHubWell.irrigatedAcresPerYear;
            }
        } else {
            well.inGeoOptix = false;
        }

        well.hasElectricalData = hasElectricalData;
        well.firstReadingDate = firstReadingDate;
        well.lastReadingDate = lastReadingDate;


        const sensors = await this.geooptixService.getSensorsForWell(wellRegistrationID);
        well.sensors = sensors;

        let annualPumpedVolume: AnnualPumpedVolumeDto[] = []

        for (var sensorType of ["Flow Meter", "Continuity Meter"]) {
            const sensorTypeSensors = sensors.filter(x => x.sensorType == sensorType);

            if (!sensorTypeSensors.length){
                continue;
            }

            annualPumpedVolume = [...annualPumpedVolume, ...await this.influxService.getAnnualPumpedVolumeForSensor(sensorTypeSensors, sensorType)];
        }

        if (hasElectricalData) {
            annualPumpedVolume = [...annualPumpedVolume, ...await this.influxService.getAnnualEstimatedPumpedVolumeForWell(wellRegistrationID)]
        }

        well.annualPumpedVolume = annualPumpedVolume;

        return well;
    }

    @Get("{wellRegistrationID}/installation")
    @Hidden()
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getInstallationRecordForWell(
        @Path() wellRegistrationID: string
    ) {
        const installationRecord = await this.geooptixService.getInstallationRecord(wellRegistrationID);

        return installationRecord;
    }

    @Get("{wellRegistrationID}/installation/{installationCanonicalName}/photo/{photoCanonicalName}")
    @Hidden()
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getPhoto(
        @Path() wellRegistrationID: string,
        @Path() installationCanonicalName: string,
        @Path() photoCanonicalName: string,
        @Request() req: RequestWithUserContext
    ) {
        const photoBuffer = await this.geooptixService.getPhoto(wellRegistrationID, installationCanonicalName, photoCanonicalName);

        if (photoBuffer) {
            req.res?.contentType("image/jpeg");
            req.res?.end(photoBuffer);
        } else {
            req.res?.status(204).end();
        }
    }

    /**
     * Returns a time series representing pumped volume at a well, summed on a chosen reporting interval,
     * for a given date range. Each point in the output time series represents the total pumped volume over the previous
     * reporting interval.
     * 
     * @param wellRegistrationID The Well Registration ID for the requested well
     * @param startDateString The start date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
     * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00)
     * @param endDateString The end date for the report, formatted as an ISO date string (eg. 2020-06-23). If a specific
     * time is requested, must contain a timezone (eg. 2020-06-23T17:24:56+00:00). Default's to today's date.
     * @param interval The reporting interval, in minutes. Defaults to 60.
     */
    @Get("{wellRegistrationID}/pumpedVolume")
    @Security(SecurityType.API_KEY)
    @SuccessResponse(200, "Returns the requested time series")
    @Response<ErrorResult>(400, "If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)")
    @Response<ErrorResult>(401, "Unauthorized to perform request")
    @Response<ErrorResult>(403, "Forbidden")
    @Response<ErrorResult>(500, "If something went wrong within the API")
    public async getPumpedVolumeByWell(
        @Path() wellRegistrationID: string,
        @Query("startDate") startDateString: string,
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
            throw new InternalServerError(err.message);
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
