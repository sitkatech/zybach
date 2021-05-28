import { provideSingleton } from "../util/provide-singleton";
import { InfluxDB, QueryApi } from "@influxdata/influxdb-client";
import secrets from '../secrets'
import { SensorSummaryDto } from "../dtos/well-summary-dto";
import { AnnualPumpedVolumeDto } from "../dtos/annual-pumped-volume-dto";
import { DateTime } from 'luxon';
import SensorMessageAgeDto from "../dtos/sensor-message-age-dto";

@provideSingleton(InfluxService)
export class InfluxService {
    private client: InfluxDB;
    private queryApi: QueryApi;
    private bucket: string;

    constructor() {
        this.client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: secrets.INFLUXDB_TOKEN });
        this.queryApi = this.client.getQueryApi(secrets.INFLUXDB_ORG);
        this.bucket = secrets.INFLUX_BUCKET
    }

    public async getLastReadingDateTime(): Promise<{ [key: string]: Date }> {
        const query = `from(bucket: "${this.bucket}") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume" or r["_measurement"] == "estimated-pumped-volume") \
        |> last() \
        |> group(columns: ["registration-id"])`

        var lastReadingDates: { [key: string]: Date } = await new Promise((resolve, reject) => {
            let results
                : { [key: string]: Date }
                = {}
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results[o["registration-id"]] = new Date(o["_time"])
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        })
        return lastReadingDates;
    }

    public async getFirstReadingDateTime(): Promise<{ [key: string]: Date }> {
        const query = `from(bucket: "${this.bucket}") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume" or r["_measurement"] == "estimated-pumped-volume") \
        |> group(columns: ["registration-id"]) \
        |> first()`

        var firstReadingDates: { [key: string]: Date } = await new Promise((resolve, reject) => {
            let results
                : { [key: string]: Date }
                = {}
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results[o["registration-id"]] = new Date(o["_time"])
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        })
        return firstReadingDates;
    }

    public async getFirstReadingDateTimeForWell(wellRegistrationID: string): Promise<Date> {
        const query = `from(bucket: "${this.bucket}") 
        |> range(start: 2000-01-01T00:00:00Z) 
        |> filter(fn: (r) => r["registration-id"] == "${wellRegistrationID}")
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume" or r["_measurement"] == "estimated-pumped-volume") 
        |> first()`

        var firstReadingDate: Date = await new Promise((resolve, reject) => {
            let resultDate: Date;
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    const currentRowDate = new Date(o["_time"]);
                    // since we are searching across multiple measurements, there may be multiple rows returned,
                    // so we have to check if the date from this row is before the date we already found
                    if (!resultDate || currentRowDate.getTime() < resultDate.getTime()) {
                        resultDate = currentRowDate;
                    }
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(resultDate);
                },
            });
        })

        return firstReadingDate;
    }
    public async getLastReadingDateTimeForWell(wellRegistrationID: string): Promise<Date> {
        const query = `from(bucket: "${this.bucket}") 
        |> range(start: 2000-01-01T00:00:00Z) 
        |> filter(fn: (r) => r["registration-id"] == "${wellRegistrationID}")
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume" or r["_measurement"] == "estimated-pumped-volume") 
        |> last()`

        var lastReadingDate: Date = await new Promise((resolve, reject) => {
            let resultDate: Date;
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    const currentRowDate = new Date(o["_time"]);
                    // since we are searching across multiple measurements, there may be multiple rows returned,
                    // so we have to check if the date from this row is after the date we already found
                    if (!resultDate || currentRowDate.getTime() > resultDate.getTime()) {
                        resultDate = currentRowDate;
                    }
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(resultDate);
                },
            });
        })

        return lastReadingDate;
    }

    public async getPumpedVolumeForSensor(sensors: SensorSummaryDto[], sensorType: string, from: Date): Promise<any[]> {
        const sensorIDFilter = sensors.map(x => `r["sn"] == "${x.sensorName}"`).join(" or ");

        const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            millisecond: 0, minute: 0, second: 0, hour: 0
        });

        const query = `from(bucket: "${this.bucket}") 
        |> range(start: ${startDate.toISO()}) 
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume")
        |> filter(fn: (r) => ${sensorIDFilter}) 
        |> group(columns: ["registration-id"])
        |> aggregateWindow(every: 1d, fn: sum, createEmpty: true, timeSrc: "_start")`

        var results: any[] = await new Promise((resolve, reject) => {
            let results: any = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push({
                        time: new Date(o["_time"]),
                        gallons: o["_value"],
                        dataSource: sensorType
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        });

        return results;
    }

    public async getMonthlyPumpedVolumeForSensor(sensors: SensorSummaryDto[], registrationID: string, from: Date): Promise<any[]> {
        const sensorIDFilter = sensors.map(x => `r["sn"] == "${x.sensorName}"`).join(" or ");

        const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            millisecond: 0, minute: 0, second: 0, hour: 0
        });

        const query = `from(bucket: "${this.bucket}") 
        |> range(start: ${startDate.toISO()}) 
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume")
        |> filter(fn: (r) => ${sensorIDFilter})
        |> filter(fn: (r) => r["registration-id"] == "${registrationID}") 
        |> aggregateWindow(every: 1mo, fn: sum, createEmpty: true, timeSrc: "_start")
        |> group(columns: ["registration-id"])`

        var results: any[] = await new Promise((resolve, reject) => {
            let results: any = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    const date = new Date(o["_time"]);
                    results.push({
                        month: date.getUTCMonth() + 1,
                        year: date.getUTCFullYear(),
                        volumePumpedGallons: o["_value"]
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        });

        return results;
    }

    public async getMonthlyElectricalBasedFlowEstimate(wellRegistrationID: string, from: Date): Promise<any[]> {

        const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            millisecond: 0, minute: 0, second: 0, hour: 0
        });

        const query = `from(bucket: "${this.bucket}") \
        |> range(start: ${startDate.toISO()}) \
        |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume" and r["registration-id"] == "${wellRegistrationID}" ) 
        |> aggregateWindow(every: 1mo, fn: sum, createEmpty: true, timeSrc: "_start")`

        var results: any[] = await new Promise((resolve, reject) => {
            let results: any = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    const date = new Date(o["_time"]);
                    results.push({
                        month: date.getUTCMonth() + 1,
                        year: date.getUTCFullYear(),
                        volumePumpedGallons: o["_value"]
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        })

        return results;
    }

    public async getElectricalBasedFlowEstimateSeries(wellRegistrationID: string, from: Date): Promise<any[]> {

        const startDate = DateTime.fromJSDate(from).setZone("America/Chicago").set({
            millisecond: 0, minute: 0, second: 0, hour: 0
        });

        const query = `from(bucket: "${this.bucket}") \
        |> range(start: ${startDate.toISO()}) \
        |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume" and r["registration-id"] == "${wellRegistrationID}" ) 
        |> aggregateWindow(every: 1d, fn: sum, createEmpty: true, timeSrc: "_start")`

        var results: any[] = await new Promise((resolve, reject) => {
            let results: any = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push({
                        time: new Date(o["_time"]),
                        gallons: o["_value"],
                        dataSource: "Electrical Usage"
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error)
                },
                complete() {
                    resolve(results);
                },
            });
        })

        return results;
    }

    public async getAnnualPumpedVolumeForSensor(sensors: SensorSummaryDto[], sensorType: string): Promise<AnnualPumpedVolumeDto[]> {
        const sensorIDFilter = sensors.map(x => `r["sn"] == "${x.sensorName}"`).join(" or ");

        const query = `from(bucket: "${this.bucket}")
        |> range(start: 2019-01-01T00:00:00.000Z)
        |> filter(fn: (r) => r["_measurement"] == "pumped-volume")
        |> filter(fn: (r) => ${sensorIDFilter})
        |> group(columns: ["registration-id"])
        |> aggregateWindow(every: 1y, fn: sum, createEmpty: true, timeSrc: "_start")`

        var results: AnnualPumpedVolumeDto[] = await new Promise((resolve, reject) => {
            let results: AnnualPumpedVolumeDto[] = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push({
                        year: new Date(o["_time"]).getFullYear(),
                        dataSource: sensorType,
                        gallons: o["_value"]
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error);
                },
                complete() {
                    resolve(results);
                }
            });
        });

        return results;
    }

    public async getAnnualEstimatedPumpedVolumeForWell(wellRegistrationID: string): Promise<AnnualPumpedVolumeDto[]> {
        const query = `from(bucket: "${this.bucket}")
        |> range(start: 2019-01-01T00:00:00.000Z)
        |> filter(fn: (r) => r["registration-id"] == "${wellRegistrationID}" and r["_measurement"] == "estimated-pumped-volume")
        |> aggregateWindow(every: 1y, fn: sum, createEmpty: true, timeSrc: "_start")`

        var results: AnnualPumpedVolumeDto[] = await new Promise((resolve, reject) => {
            let results: AnnualPumpedVolumeDto[] = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push({
                        year: new Date(o["_time"]).getFullYear(),
                        dataSource: "Electrical Usage",
                        gallons: o["_value"]
                    })
                },
                error(error) {
                    console.error(error);
                    reject(error);
                },
                complete() {
                    resolve(results);
                }
            });
        });

        return results;
    }

    public async getAnnualEstimatedPumpedVolumeByWellForYear(year: number): Promise<Map<string, number>> {
        const startDate = DateTime.local(year);
        const endDate = DateTime.local(year + 1);

        const query = `from(bucket: "${this.bucket}")
        |> range(start: ${startDate.toISO()}, stop: ${endDate.toISO()})
        |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume")
        |> aggregateWindow(every: 1y, fn: sum, createEmpty: true, timeSrc: "_start")
        |> group(columns: ["registration-id"], mode: "by")`

        var results: Map<string, number> = await new Promise((resolve, reject) => {
            let results: Map<string, number> = new Map<string, number>()
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.set(o["registration-id"], o["_value"]);
                },
                error(error) {
                    console.error(error);
                    reject(error);
                },
                complete() {
                    resolve(results);
                }
            });
        });

        return results;
    }

    public async getWellRegistrationIdsWithElectricalEstimateAsOfYear(year: number): Promise<string[]> {

        const query = `from(bucket: "${this.bucket}")
        |> range(start: 2019-01-01T00:00:00.000Z, stop: ${year}-12-31T23:59:59.999Z)
        |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume")
        |> group(columns: ["registration-id"], mode: "by")
        |> last()`

        var results: string[] = await new Promise((resolve, reject) => {
            let results: string[] = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push(o["registration-id"]);
                },
                error(error) {
                    console.error(error);
                    reject(error);
                },
                complete() {
                    resolve(results);
                }
            });
        });

        return results;
    }

    public async getLastMessageAgeBySensor(): Promise<Map<string, number>> {
        const query = `ageInSeconds = (x) => { 
            timeNow = uint(v:now())
            timeEvent = uint(v: x)
            return (timeNow - timeEvent) / uint(v:1000000000)
          }
          
          from(bucket: "tpnrd")
            |> range(start: -30d)
            |> filter(fn: (r) => r["_measurement"] == "ingest-count")
            |> group(columns: ["sn"])
            |> last()
            |> map(fn: (r) => ({
              r with eventAge: ageInSeconds(x: r._time)
            }))`

        var results: Map<string,number> = await new Promise((resolve, reject) => {
            let results: Map<string,number> = new Map();
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.set(o["sn"], o["eventAge"]);
                },
                error(error) {
                    console.error(error);
                    reject(error);
                },
                complete() {
                    resolve(results);
                }
            });
        });

        return results;
    }
}