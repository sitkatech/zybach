import { provideSingleton } from "../util/provide-singleton";
import { InfluxDB, QueryApi } from "@influxdata/influxdb-client";
import secrets from '../secrets'
import { InternalServerError } from "../errors/internal-server-error";

@provideSingleton(InfluxService)
export class InfluxService {
    private client: InfluxDB;
    private queryApi: QueryApi;
    constructor(){
        
        this.client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: secrets.INFLUXDB_TOKEN });
        this.queryApi = this.client.getQueryApi(secrets.INFLUXDB_ORG);
    }

    public async getLastReadingDatetime(): Promise<{ [key: string]: Date }> {
        const query = 'from(bucket: "tpnrd") \
        |> range(start: 2000-01-01T00:00:00Z) \
        |> filter(fn: (r) => r["_measurement"] == "continuity" or r["_measurement"] == "gallons") \
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
}