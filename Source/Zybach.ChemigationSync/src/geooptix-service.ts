import axios from 'axios';
import { GeoOptixSite, Sample, WorkOrder } from './models';
const dockerSecrets  = require('@cloudreach/docker-secrets');

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

const headers = {
    "x-geooptix-token": config["GEOOPTIX_API_KEY"]
}

const baseUrl = process.env["GEOOPTIX_BASE_URL"];

const getSites = async () =>{
    const url = `${baseUrl}/projects/water-data-program/sites`;
    try {
        const response = await axios.get(url, {
            headers
        });

        return response.data;
    } catch (error) {
        console.error({
            url, method: "GET", response: error.response.data
        });
        throw new Error(error.message);
    }
}

const getWorkOrder = async (workOrderCName: string) => {
    let response;
    const url =`${baseUrl}/projects/water-data-program/workOrders/${workOrderCName}/`;
    try {
        response =
        await axios.get(url, {
            headers,
            validateStatus: (x: number) => true
        });
    } catch (error) {
        console.error({
            url, method: "GET", response: error.response.data
        });
        throw new Error(error.message);
    }
    
    if (response.status === 404){
        return null
    } 
    
    if (response.status === 200) {
        return response.data;
    } 
    
    
    console.error({
        url, method: "GET", response: response.data
    });
    throw new Error(response.statusText);
}

const getWorkOrderSamples = async (workOrderCName: string) => {
    const url = `${baseUrl}/projects/water-data-program/workOrders/${workOrderCName}/samples`;
    try {
        const response =
        await axios.get(url, {
            headers
        });

        return response.data;
    } catch (error) {
        console.error({
            url, method: "GET", response: error.response.data
        });
        throw new Error(error.message);
    }
}

const createWorkOrder = async (workOrder: WorkOrder) => {
    const url = `${baseUrl}/projects/water-data-program/workOrders`

    try {
        await axios.post(url, workOrder, {
            headers
        })
    } catch (error) {
        console.error({
            url, requestBody: workOrder, method: "POST", response: error.response.data
        });
        throw new Error(error.message);
    }
}

const createSite =  async (site: GeoOptixSite) => {
    const url = `${baseUrl}/projects/water-data-program/sites`
    
    try {
        await axios.post(url, site, {
            headers
        })
    } catch (error) {
        console.error({
            url, requestBody: site, method: "POST", response: error.response.data
        });
        throw new Error(error.message);
    }

}

const createSample = async (sample: Sample) => {
    const url = `${baseUrl}/projects/water-data-program/sites/${sample.SiteCanonicalName}/samples`

    try {
        await axios.post(url, sample, {
            headers
        })
    } catch (error) {
        console.error({
            url, requestBody: sample, method: "POST", response: error.response.data
        });
        throw new Error(error.message);
    }
}

const deleteSample = async (sample: Sample) => {
    const url = `${baseUrl}/projects/water-data-program/sites/${sample.SiteCanonicalName}/samples/${sample.CanonicalName}`
    
    try {
        await axios.delete(url, {
            headers
        })
    } catch (error) {
        console.error({
            url, method: "DELETE", response: error.response.data
        });
        throw new Error(error.message);
    }
}

export {getSites, getWorkOrder, getWorkOrderSamples, createWorkOrder, createSite, createSample, deleteSample}