import axios from 'axios';
const dockerSecrets  = require('@cloudreach/docker-secrets');

const config = JSON.parse(dockerSecrets.Inspection_Fetch_Secret);

const headers = {
    "x-geooptix-token": config["GEOOPTIX_API_KEY"]
}

const baseUrl = process.env["GEOOPTIX_BASE_URL"];

const getSamples = async () =>{
    const url = `${baseUrl}/project-overview-web/water-data-program/samples`;
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

export {getSamples}