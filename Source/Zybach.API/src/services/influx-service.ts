import { provideSingleton } from "../util/provide-singleton";
import { InfluxDB, QueryApi } from "@influxdata/influxdb-client";
import secrets from '../secrets'

@provideSingleton(InfluxService)
export class InfluxService {
    private client: InfluxDB;
    private queryApi: QueryApi;
    constructor(){
        
        this.client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: secrets.INFLUXDB_TOKEN });
        this.queryApi = this.client.getQueryApi(secrets.INFLUXDB_ORG);
    }

    public async getLastReadingDateTime(): Promise<{ [key: string]: Date }> {
        const query = 'from(bucket: "tpnrd-qa") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "continuity" or r["_measurement"] == "gallons" or r["_measurement"] == "estimated-pumped-volume") \
        |> last() \
        |> group(columns: ["registration-id"])'

        var lastReadingDates: { [key: string]: Date } = await new Promise((resolve,reject) => {
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
        const query = 'from(bucket: "tpnrd-qa") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "continuity" or r["_measurement"] == "gallons" or r["_measurement"] == "estimated-pumped-volume") \
        |> first() \
        |> group(columns: ["registration-id"])'

        var lastReadingDates: { [key: string]: Date } = await new Promise((resolve,reject) => {
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

    // todo: type this
    public async getElectricalBasedFlowEstimateSeries(wellRegistrationID: string) {
        // todo: this query needs to fill missing days with zeroes?
        const query = `from(bucket: "tpnrd-qa") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume" and r["registration-id"] == "${wellRegistrationID}" ) `

        var results = await new Promise((resolve,reject) => {
            let results: any = [];
            this.queryApi.queryRows(query, {
                next(row, tableMeta) {
                    const o = tableMeta.toObject(row);
                    results.push({
                        time: new Date(o["_time"]),
                        gallons: o["_value"],
                        dataSource: "Electrical Data"
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
}