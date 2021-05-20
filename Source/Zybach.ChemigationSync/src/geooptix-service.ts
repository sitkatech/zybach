import axios from 'axios';
import { GeoOptixSite, Sample, WorkOrder } from 'models';
const dockerSecrets  = require('@cloudreach/docker-secrets');

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

const headers = {
    "x-geooptix-token": config["GEOOPTIX_API_KEY"]
}
console.log(headers);

const baseUrl = process.env["GEOOPTIX_BASE_URL"];

const getSites = async () =>{
    try {
        const response = await axios.get(`${baseUrl}/projects/water-data-program/sites`, {
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
        await axios.get(`${baseUrl}/projects/water-data-program/workOrders/${workOrderCName}/`,{
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
        await axios.get(`${baseUrl}/projects/water-data-program/workOrders/${workOrderCName}/samples`,{
            headers
        });

        return response.data;
    } catch (error) {
        console.error(error)
        throw new Error(error.message);
    }
}

const createWorkOrder = async (workOrder: WorkOrder) => {
    try {
        const url = `${baseUrl}/projects/water-data-program/workOrders`
        await axios.post(url, workOrder, {
            headers
        })
    } catch (error) {
        console.error(error);
        throw new Error(error.message);
    }
}

const createSite =  async (site: GeoOptixSite) => {
    
    try {
        const url = `${baseUrl}/projects/water-data-program/sites`
        await axios.post(url, site, {
            headers
        })
    } catch (error) {
        console.error(error);
        throw new Error(error.message);
    }

}

const createSample = async (sample: Sample) => {

    try {
        const url = `${baseUrl}/projects/water-data-program/workOrders/${sample.WorkOrderCanonicalName}/samples`
        await axios.post(url, sample, {
            headers
        })
    } catch (error) {
        console.error(error);
        throw new Error(error.message);
    }
}

const deleteSample = async (sample: Sample) => {
    
    try {
        const url = `${baseUrl}/projects/water-data-program/workOrders/${sample.WorkOrderCanonicalName}/samples/${sample.CanonicalName}`
        await axios.delete(url, {
            headers
        })
    } catch (error) {
        console.error(error);
        throw new Error(error.message);
    }
}

export {getSites, getWorkOrder, getWorkOrderSamples, createWorkOrder, createSite, createSample, deleteSample}