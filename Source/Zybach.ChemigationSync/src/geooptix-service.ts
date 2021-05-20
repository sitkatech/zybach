import got from 'got';
const dockerSecrets  = require('@cloudreach/docker-secrets');

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

const headers = {
    "x-geooptix-token": config["GEOOPTIX_API_KEY"]
}
console.log(headers);

const baseUrl = process.env["GEOOPTIX_BASE_URL"];

const getSites = async () =>{
    try {
        const response = await got.get(`${baseUrl}/project-overview-web/water-data-program/sites`, {
            headers
        });

        return response.body;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

const getWorkOrder = async (workOrderCName: string) => {
    try {
        const response =
        await got.get(`${baseUrl}/project-overview-web/water-data-program/workOrders/${workOrderCName}/`,{
            headers
        });

        return response.body;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

const getWorkOrderSamples = async (workOrderCName: string) => {
    try {
        const response =
        await got.get(`${baseUrl}/project-overview-web/water-data-program/workOrders/${workOrderCName}/samples`,{
            headers
        });

        return response.body;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

export {getSites, getWorkOrder, getWorkOrderSamples}