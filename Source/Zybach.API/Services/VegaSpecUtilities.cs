using Newtonsoft.Json;
using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class VegaSpecUtilities
    {
        public static string GetNitrateChartVegaSpec(List<WaterQualityInspectionForVegaChartDto> chartDtos, bool isForWeb)
        {
            var reportDocumentOnlyConfig = @"
                ""config"": {
                    ""axis"": {
                        ""labelFontSize"": 20,
                        ""titleFontSize"": 30
                    }, 
                    ""text"": {
                        ""fontSize"":20
                    }, 
                    ""legend"": {
                        ""labelFontSize"": 30, 
                        ""symbolSize"":500,
                        ""labelLimit"":300
                    }
                }";

            return $@"{{
            ""$schema"": ""https://vega.github.io/schema/vega-lite/v5.1.json"",
            ""description"": ""Lab Nitrates Chart"",
            ""width"": {(isForWeb ? "\"container\"" : 1351)},
            ""height"": {(isForWeb ? "\"container\"" : 500)},
            ""data"": {{ 
                ""values"": {JsonConvert.SerializeObject(chartDtos)}
            }},
            ""encoding"": {{
                ""x"": {{
                  ""field"": ""InspectionDate"",                    
                  ""timeUnit"": ""yearmonthdate"",                    
                  ""type"": ""temporal"",                    
                  ""axis"": {{
                    ""title"": ""Inspection Date""
                    {(!isForWeb ? ",\"labelAngle\":50" : "")}
                  }}
                }},    
                ""color"":{{
                  ""type"":""nominal"",
                  ""scale"":{{
                    ""range"":[""blue"", ""red""], 
                    ""domain"": [""Nitrate Level"", ""Current Nitrate Level""]
                  }}
                }}
            }},           
            ""layer"": 
            [
            {{                
                ""encoding"": {{                    
                ""y"": {{                        
                    ""field"": ""LabNitrates"",
                    ""type"": ""quantitative"",
                    ""axis"": {{                            
                        ""title"": ""Lab Nitrates""
                    }}
                }}
                }},               
                ""layer"": [
                {{ 
                    ""mark"": ""line"",
                    ""encoding"": {{
                    ""color"": {{
                        ""datum"":""Nitrate Level""
                    }}
                    }}
                }},           
                {{
            ""transform"": [
                    {{
                ""filter"": 
                        {{ ""selection"": ""hover"" }}
            }}
                    ], 
                    ""mark"": ""point""
                }}                
                ]           
            }},  
            {{
            ""encoding"": {{
                ""y"": {{
                    ""field"": ""MostRecentDateLabNitrates"",
                    ""type"":""quantitative""
                }}
            }},
                ""layer"": [
                {{
                ""mark"": ""line"",
                    ""encoding"": {{
                    ""color"": {{
                        ""datum"":""Current Nitrate Level""
                    }}
                }}
            }},           
                {{
                ""transform"": [
                    {{
                    ""filter"": 
                        {{ ""selection"": ""hover"" }}
                }}
                    ], 
                    ""mark"": ""point""
                }}                
                ]    
            }},       
            {{
            ""mark"": ""rule"",                
                ""encoding"": {{
                ""opacity"": {{
                    ""condition"": {{
                        ""value"": 0.3, 
                    ""selection"": ""hover""
                    }},                    
                    ""value"": 0
                }},                
                ""tooltip"": [
                    {{
                    ""field"": ""InspectionDate"", 
                    ""type"": ""temporal"", 
                    ""title"": ""Date""
                    }},                    
                    {{
                    ""field"": ""LabNitrates"", 
                    ""type"": ""quantitative"", 
                    ""title"": ""Lab Nitrates""
                    }},
                    {{
                    ""field"": ""MostRecentDateLabNitrates"", 
                    ""type"": ""quantitative"", 
                    ""title"": ""Current Nitrate Level""
                    }}              
                ]}},                
                ""selection"": 
                    {{
                    ""hover"": {{
                        ""type"": ""single"",                        
                            ""fields"": [""InspectionDate""],                        
                            ""nearest"": true,                        
                            ""on"": ""mouseover"",                        
                            ""empty"": ""none"",                        
                            ""clear"": ""mouseout""
                        }}
                    }}
                }}
            ],
            ""title"": {{
                ""text"":""Nitrate Levels""
                {(!isForWeb ? ",\"fontSize\": 30 ": "")}
            }}{ (!isForWeb ? "," + reportDocumentOnlyConfig : "")}
        }}";
        }

        public static string GetWaterLevelChartVegaSpec(List<WaterLevelInspectionForVegaChartDto> chartDtos, bool isForWeb)
        {
            var reportDocumentOnlyConfig = @"
                ""config"": {
                    ""axis"": {
                        ""labelFontSize"": 20,
                        ""titleFontSize"": 30
                    }, 
                    ""text"": {
                        ""fontSize"":20
                    }, 
                    ""legend"": {
                        ""labelFontSize"": 30, 
                        ""symbolSize"":500,
                        ""labelLimit"":300
                    }
                }";

            return $@"{{
            ""$schema"": ""https://vega.github.io/schema/vega-lite/v5.1.json"",
            ""description"": ""Water Level Chart"",
            ""width"": {(isForWeb ? "\"container\"" : 1351)},
            ""height"": {(isForWeb ? "\"container\"" : 500)},
            ""data"": {{ 
                ""values"": {JsonConvert.SerializeObject(chartDtos)}
            }},
            ""encoding"": {{
                ""x"": {{
                  ""field"": ""InspectionDate"",                    
                  ""timeUnit"": ""yearmonthdate"",                    
                  ""type"": ""temporal"",                    
                  ""axis"": {{
                    ""title"": ""Inspection Date""
                    {(!isForWeb ? ",\"labelAngle\":50" : "")}
                  }}
                }},    
                ""color"":{{
                  ""type"":""nominal"",
                  ""scale"":{{
                    ""range"":[""blue"", ""red""], 
                    ""domain"": [""Depth"", ""Current Depth""]
                  }}
                }}
            }},           
            ""layer"": 
            [
            {{                
                ""encoding"": {{                    
                ""y"": {{                        
                    ""field"": ""Measurement"",
                    ""type"": ""quantitative"",
                    ""axis"": {{                            
                        ""title"": ""Depth to Groundwater""
                    }}
                }}
                }},               
                ""layer"": [
                {{ 
                    ""mark"": ""line"",
                    ""encoding"": {{
                    ""color"": {{
                        ""datum"":""Depth""
                    }}
                    }}
                }},           
                {{
            ""transform"": [
                    {{
                ""filter"": 
                        {{ ""selection"": ""hover"" }}
            }}
                    ], 
                    ""mark"": ""point""
                }}                
                ]           
            }},  
            {{
            ""encoding"": {{
                ""y"": {{
                    ""field"": ""MostRecentMeasurement"",
                    ""type"":""quantitative""
                }}
            }},
                ""layer"": [
                {{
                ""mark"": ""line"",
                    ""encoding"": {{
                    ""color"": {{
                        ""datum"":""Current Depth""
                    }}
                }}
            }},           
                {{
                ""transform"": [
                    {{
                    ""filter"": 
                        {{ ""selection"": ""hover"" }}
                }}
                    ], 
                    ""mark"": ""point""
                }}                
                ]    
            }},       
            {{
            ""mark"": ""rule"",                
                ""encoding"": {{
                ""opacity"": {{
                    ""condition"": {{
                        ""value"": 0.3, 
                    ""selection"": ""hover""
                    }},                    
                    ""value"": 0
                }},                
                ""tooltip"": [
                    {{
                    ""field"": ""InspectionDate"", 
                    ""type"": ""temporal"", 
                    ""title"": ""Date""
                    }},                    
                    {{
                    ""field"": ""Measurement"", 
                    ""type"": ""quantitative"", 
                    ""title"": ""Depth""
                    }},
                    {{
                    ""field"": ""MostRecentMeasurement"", 
                    ""type"": ""quantitative"", 
                    ""title"": ""Current Depth""
                    }}              
                ]}},                
                ""selection"": 
                    {{
                    ""hover"": {{
                        ""type"": ""single"",                        
                            ""fields"": [""InspectionDate""],                        
                            ""nearest"": true,                        
                            ""on"": ""mouseover"",                        
                            ""empty"": ""none"",                        
                            ""clear"": ""mouseout""
                        }}
                    }}
                }}
            ],
            ""title"": {{
                ""text"":""Depth to Groundwater""
                {(!isForWeb ? ",\"fontSize\": 30 " : "")}
            }}{ (!isForWeb ? "," + reportDocumentOnlyConfig : "")}
        }}";
        }
    }
}