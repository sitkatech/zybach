using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace Zybach.EFModels.Entities
{
    public partial class WellExtensionMethods
    {
        static partial void DoCustomMappings(Well well, WellDto wellDto)
        {
            wellDto.Longitude = well.Longitude;
            wellDto.Latitude = well.Latitude;
        }

        static partial void DoCustomSimpleDtoMappings(Well well, WellSimpleDto wellSimpleDto)
        {
            wellSimpleDto.WellParticipationName = well.WellParticipation?.WellParticipationDisplayName;
        }

        public static WellInspectionSummaryDto AsWellInspectionSummaryDto(this Well well,
            WaterLevelInspectionSimpleDto waterLevelInspectionSimpleDto, WaterQualityInspectionSimpleDto waterQualityInspectionSimpleDto)
        {
            var wellInspectionSummaryDto = new WellInspectionSummaryDto()
            {
                Well = well.AsSimpleDto(),

                HasWaterLevelInspections = waterLevelInspectionSimpleDto != null,
                LatestWaterLevelInspectionDate = waterLevelInspectionSimpleDto?.InspectionDate,
                HasWaterQualityInspections = waterQualityInspectionSimpleDto != null,
                LatestWaterQualityInspectionDate = waterQualityInspectionSimpleDto?.InspectionDate
            };

            return wellInspectionSummaryDto;
        }

        public static WellWaterLevelInspectionDetailedDto AsWellWaterLevelInspectionDetailedDto(this Well well,
            List<WaterLevelInspectionSimpleDto> waterLevelInspectionSimpleDtos)
        {
            var wellWaterLevelInspectionDetailedDto = new WellWaterLevelInspectionDetailedDto()
            {
                Well = well.AsSimpleDto(),

                WaterLevelInspections = waterLevelInspectionSimpleDtos
            };

            return wellWaterLevelInspectionDetailedDto;
        }

        public static WellWaterQualityInspectionDetailedDto AsWellWaterQualityInspectionDetailedDto(this Well well,
            List<WaterQualityInspectionSimpleDto> waterQualityInspectionSimpleDtos)
        {
            var wellWaterQualityInspectionDetailedDto = new WellWaterQualityInspectionDetailedDto()
            {
                Well = well.AsSimpleDto(),

                WaterQualityInspections = waterQualityInspectionSimpleDtos
            };

            return wellWaterQualityInspectionDetailedDto;
        }

        public static string GetNitrateChartVegaSpec(this Well well, List<WaterQualityInspectionForVegaChartDto> chartDtos)
        {
            return $@"{{
            ""$schema"": ""https://vega.github.io/schema/vega-lite/v4.json"",
            ""description"": ""Lab Nitrates Chart"",
            ""width"": ""container"",
            ""height"": ""container"",
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
            ]
        }}";
        }
    }
}
