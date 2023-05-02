using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
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
                  ""type"": ""temporal"",                    
                  ""axis"": {{
                    ""title"": ""Inspection Date"",
                    ""labelExpr"": ""timeFormat(datum.value, '%Y')""
                    {(!isForWeb ? ",\"labelAngle\":50" : "")}
                  }}
                }},    
                ""color"":{{
                  ""type"":""nominal"",
                  ""scale"":{{
                    ""range"":[""blue""], 
                    ""domain"": [""Nitrate Level""]
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
                {(!isForWeb ? ",\"fontSize\": 30 " : "")}
            }}{(!isForWeb ? "," + reportDocumentOnlyConfig : "")}
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
                  ""type"": ""temporal"",                    
                  ""axis"": {{
                    ""title"": ""Inspection Date"",
                     ""labelExpr"": ""timeFormat(datum.value, '%Y')""
                    {(!isForWeb ? ",\"labelAngle\":50" : "")}
                  }}
                }},    
                ""color"":{{
                  ""type"":""nominal"",
                  ""field"":""WellRegistrationID"",
                  ""legend"": {{ ""title"": ""Well"" }}
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
                        ""title"": ""Depth to Groundwater (ft)""
                    }},
                    ""scale"": {{
                        ""reverse"": ""true""
                    }}
                }}
                }},               
                ""layer"": [          
                {{
                    ""mark"": ""line""
                }},           
                {{
                    ""transform"": [
                        {{
                            ""filter"": {{ ""selection"": ""hover"" }}
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
                    ""field"": ""WellRegistrationID"", 
                    ""type"": ""text"", 
                    ""title"": ""Well Reg #""
                    }} 
                ]}},                
                ""selection"": {{
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
            }}{(!isForWeb ? "," + reportDocumentOnlyConfig : "")}
        }}";
        }

        public static string GetSensorDetailChartSpecAsOneSeries(Sensor sensor)
        {
            var yAxisTitle = $"\"{sensor.SensorType.YAxisTitle}\"";
            var yAxisScaleReverse = (sensor.SensorType.ReverseYAxisScale ? "true" : "false");
            var vegaLiteChartSpec =
                $@"
{{
  ""$schema"": ""https://vega.github.io/schema/vega-lite/v5.1.json"",
  ""description"": ""A chart"",
  ""width"": ""container"",
  ""height"": ""container"",
  ""data"": {{ ""name"": ""TimeSeries"" }},
  ""encoding"": {{
    ""x"": {{
      ""field"": ""MeasurementDate"",
      ""timeUnit"": ""yearmonthdate"",
      ""type"": ""temporal"",
      ""axis"": {{
        ""title"": ""Date""
      }}
    }}
  }},
  ""layer"": [
    {{
      ""mark"": {{
        ""type"": ""line""
      }},
      ""encoding"": {{
        ""color"": {{
          ""field"": ""DataSourceName"",
          ""type"": ""nominal"",
          ""legend"": {{
            ""title"": ""Data Source""
          }},
          ""scale"": {{
            ""domain"": [
              ""{sensor.SensorType.SensorTypeDisplayName}""
            ],
            ""range"": [
              ""{sensor.SensorType.ChartColor}""
            ]
          }}
        }}
      }},
      ""layer"": [
        {{
          ""mark"": ""line""
        }},
        {{
          ""transform"": [
            {{
              ""filter"": {{
                ""selection"": ""hover""
              }}
            }}
          ],
          ""mark"": ""point""
        }}
      ]
    }},
    {{
      ""mark"": {{
        ""type"": ""line"",
        ""color"": ""{sensor.SensorType.AnomalousChartColor}""
      }},
      ""transform"": [
        {{
          ""as"": ""MeasurementValue"",
          ""calculate"": ""if(datum.IsAnomalous == true, datum.MeasurementValue, null)""
        }},
        {{
          ""calculate"": ""true"",
          ""as"": ""baseline""
        }}
      ]
    }},
    {{
      ""mark"": {{
        ""type"": ""circle"",
        ""color"": ""{sensor.SensorType.AnomalousChartColor}""
      }},
      ""transform"": [
        {{
          ""filter"": ""datum.IsAnomalous == true""
        }},
        {{
          ""calculate"": ""true"",
          ""as"": ""baseline""
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
            ""field"": ""MeasurementDate"",
            ""type"": ""temporal"",
            ""title"": ""Date""
          }},
          {{
            ""field"": ""MeasurementValueString"",
            ""type"": ""ordinal"",
            ""title"": {yAxisTitle}
          }}
        ]
      }},
      ""selection"": {{
        ""hover"": {{
          ""type"": ""single"",
          ""fields"": [
            ""MeasurementDate"",
            ""MeasurementValueString""
          ],
          ""nearest"": true,
          ""on"": ""mouseover"",
          ""empty"": ""none"",
          ""clear"": ""mouseout""
        }}
      }}
    }}
  ],
  ""encoding"": {{
    ""x"": {{
      ""field"": ""MeasurementDate"",
      ""type"": ""temporal"",
      ""timeUnit"": ""yearmonthdate"",
      ""axis"": {{
        ""title"": ""Date"",
        ""grid"": false
      }}
    }},
    ""y"": {{
      ""field"": ""MeasurementValue"",
      ""type"": ""quantitative"",
      ""axis"": {{
        ""title"": {yAxisTitle},
        ""grid"": false
      }},
      ""scale"": {{
        ""reverse"": {yAxisScaleReverse}
      }}
    }},
    ""detail"": {{
      ""field"": ""DataSourceName"",
      ""type"": ""nominal""
    }}
  }}
}}";
            return vegaLiteChartSpec;
        }

        public static string GetSensorTypeChartSpec(Sensor sensor)
        {
            return GetSensorTypeChartSpec(new List<Sensor>{sensor}, sensor.SensorType);
        }

        public static string GetSensorTypeChartSpec(List<Sensor> sensors, SensorType sensorType)
        {
            if (!sensors.Any())
            {
                return null;
            }
            var domains = new List<string>();
            var chartColors = new List<string>();
            var tooltipFields = new List<string>();
            foreach (var sensor in sensors)
            {
                domains.AddRange(sensor.GetChartDomains());
                chartColors.AddRange(sensor.GetChartColors());
                tooltipFields.AddRange(sensor.GetChartTooltipFields());
            }
            // this assumes that the sensors passed in are all of the same y axis scale and x axis scale, as in their Sensor Types are comparable
            return GetSensorTypeChartSpec(sensorType.YAxisTitle, sensorType.ReverseYAxisScale, domains, chartColors, tooltipFields);
        }

        public static string GetSensorTypeChartSpec(string yAxisTitle, bool reverseYAxisScale, List<string> domains, List<string> chartColors, List<string> tooltipFields)
        {
            var yAxisScaleReverse = (reverseYAxisScale ? "true" : "false");
            var vegaLiteChartSpec =
                $@"
{{
  ""$schema"": ""https://vega.github.io/schema/vega-lite/v5.1.json"",
  ""description"": ""A chart"",
  ""width"": ""container"",
  ""height"": ""container"",
  ""data"": {{ ""name"": ""TimeSeries"" }},
  ""encoding"": {{
    ""x"": {{
      ""field"": ""MeasurementDate"",
      ""timeUnit"": ""yearmonthdate"",
      ""type"": ""temporal"",
      ""axis"": {{
        ""title"": ""Date""
      }}
    }}
  }},
  ""layer"": [
    {{
      ""encoding"": {{
        ""y"": {{
          ""field"": ""MeasurementValue"",
          ""type"": ""quantitative"",
          ""axis"": {{
            ""title"": ""{yAxisTitle}""
          }},
          ""scale"": {{
            ""reverse"": {yAxisScaleReverse},
            ""zero"":false,
            ""padding"":10
          }}
        }},
        ""color"": {{
          ""field"": ""DataSourceName"",
          ""type"": ""nominal"",
          ""legend"": {{ ""title"": ""Data Source"", ""orient"": ""bottom"" }},
          ""scale"": {{
            ""domain"": [{string.Join(", ", domains)}],
            ""range"": [{string.Join(", ", chartColors)}]
          }}
        }}
      }},
      ""layer"": [
        {{ ""mark"": ""line"" }},
        {{ ""transform"": [{{ ""filter"": {{ ""param"": ""hover"", ""empty"": false }} }}], ""mark"": ""point"" }}
      ]
    }},
    {{
      ""transform"": [
        {{
            ""pivot"": ""DataSourceName"",
            ""value"": ""MeasurementValueString"",
            ""groupby"": [""MeasurementDate""],
            ""op"": ""max""
        }}
      ],
      ""mark"": {{""type"":""rule"",""opacity"":0}},
      ""encoding"": {{
        ""opacity"": {{
          ""condition"": {{ ""value"": 0.3, ""param"": ""hover"",""empty"":false }},
          ""value"": 0
        }},
        ""tooltip"": [
          {{ ""field"": ""MeasurementDate"", ""type"": ""temporal"", ""title"": ""Date"" }},
          {string.Join(", ", tooltipFields)}
        ]
      }},
      ""params"": [
            {{
                ""name"": ""hover"",
                ""select"": {{""type"": ""point"", ""on"": ""mouseover"", ""nearest"": true, ""clear"":""mouseout"", ""fields"":[""MeasurementDate""]}}
            }}
        ]
    }}
  ]
}}";
            return vegaLiteChartSpec;
        }
    }
}