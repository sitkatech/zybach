
export enum DataSourceFilterOption {
    FLOW = "Flowmeter",
    CONTINUITY = "Continuity Devices",
    ELECTRICAL = "Electrical Data",
    NODATA = "No Estimate Available"
  }
  
  export const DataSourceSensorTypeMap = {
    "Flowmeter": "FlowMeter",
    "Continuity Devices": "PumpMonitor",
    "Electrical Data": "Electrical Data",
    "No Estimate Available": "N/A"
  }
  