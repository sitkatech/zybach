using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroolTool.EFModels.Entities;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Union;
using Newtonsoft.Json;

namespace DroolTool.API.Controllers
{
    public class NeighborhoodExplorerController : ControllerBase
    {
        private readonly DroolToolDbContext _dbContext;

        public NeighborhoodExplorerController(DroolToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("neighborhood-explorer/get-mask")]
        public ActionResult<string> GetNeighborhoodExplorerMask()
        {
            var watersheds = _dbContext.Watershed.Select(x => x.WatershedGeometry4326);
            var geometry = UnaryUnionOp.Union(watersheds);
            var feature = new Feature() { Geometry = geometry };
            var gjw = new GeoJsonWriter
            {
                SerializerSettings =
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    Formatting = Formatting.Indented
                }
            };
            var featureCollection = new FeatureCollection { feature };
            var write = gjw.Write(featureCollection);
            return Ok(write);
        }
    }
}
