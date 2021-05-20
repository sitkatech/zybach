import axios from 'axios';
const dockerSecrets  = require('@cloudreach/docker-secrets');

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

const headers = {
    "x-geooptix-token": config["GEOOPTIX_API_KEY"]
}
console.log(headers);

const baseUrl = process.env["GEOOPTIX_BASE_URL"];

const getSites = async () =>{
    try {
        const response = await axios.get(`${baseUrl}/project-overview-web/water-data-program/sites`, {
            headers
        });

        return response.data;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

const getWorkOrder = async (workOrderCName: string) => {
    let response;
    try {
        response =
        await axios.get(`${baseUrl}/project-overview-web/water-data-program/workOrders/${workOrderCName}/`,{
            headers,
            validateStatus: (x: number) => true
        });
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
    
    if (response.status === 404){
        return null
    } 
    
    if (response.status === 200) {
        return response.data;
    } 
    
    throw new Error(response.statusText);
}

const getWorkOrderSamples = async (workOrderCName: string) => {
    try {
        const response =
        await axios.get(`${baseUrl}/project-overview-web/water-data-program/workOrders/${workOrderCName}/samples`,{
            headers
        });

        return response.data;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

export {getSites, getWorkOrder, getWorkOrderSamples}