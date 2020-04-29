using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DroolTool.EFModels.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

            return Ok(buildFeatureCollectionAndWriteGeoJson(new List<Feature> { new Feature() { Geometry = geometry } }));
        }

        [HttpGet("neighborhood-explorer/get-serviced-neighborhood-ids")]
        public ActionResult<List<int>> GetServicedNeighborhoodIds()
        {
            return Ok(_dbContext.Neighborhood.Where(x => x.BackboneSegment.Any()).Select(x => x.NeighborhoodID).ToList());
        }

        [HttpGet("neighborhood-explorer/get-stormshed/{neighborhoodID}")]
        public ActionResult<string> GetStormshed([FromRoute]int neighborhoodID)
        {
            var backboneAccumulated = new List<BackboneSegment>();

            var backboneSegments = _dbContext.BackboneSegment
                .Include(x => x.Neighborhood)
                .Include(x => x.DownstreamBackboneSegment)
                .Include(x => x.InverseDownstreamBackboneSegment)
                .ToList();

            var startingPoint = _dbContext.Neighborhood
                .Include(x => x.BackboneSegment)
                .Single(x => x.NeighborhoodID == neighborhoodID).BackboneSegment.ToList();

            var lookingAt = backboneSegments.Where(x => startingPoint.Contains(x) && x.BackboneSegmentTypeID != (int)BackboneSegmentTypeEnum.Channel).ToList();

            while (lookingAt.Any())
            {
                backboneAccumulated.AddRange(lookingAt);

                var newEntities = backboneSegments.Where(x => lookingAt.Contains(x));

                var downFromHere = newEntities.Where(x => x.DownstreamBackboneSegment != null)
                    .Select(x => x.DownstreamBackboneSegment)
                    .Where(x => x.BackboneSegmentTypeID != (int)BackboneSegmentTypeEnum.Channel)
                    .ToList()
                    .Distinct()
                    .Except(backboneAccumulated);

                var upFromHere = newEntities.SelectMany(x => x.InverseDownstreamBackboneSegment)
                    .Where(x => x.BackboneSegmentTypeID != (int)BackboneSegmentTypeEnum.Channel)
                    .ToList()
                    .Distinct()
                    .Except(backboneAccumulated);

                lookingAt = upFromHere.Union(downFromHere).ToList();
            }

            var listBackboneAccumulated = backboneAccumulated.Select(x => x.Neighborhood)
                .ToList()
                .Distinct()
                .Where(x => x != null)
                .ToList();

            var feature = new Feature()
            {
                Geometry = UnaryUnionOp.Union(listBackboneAccumulated.Select(x => x.NeighborhoodGeometry4326).ToList()),
                Attributes = new AttributesTable()
            };

            feature.Attributes.Add("NeighborhoodIDs", listBackboneAccumulated.Select(x => x.NeighborhoodID).ToList());

            return Ok(buildFeatureCollectionAndWriteGeoJson(new List<Feature> { feature }));
        }

        [HttpGet("neighborhood-explorer/get-downstream-backbone-trace/{neighborhoodID}")]
        public ActionResult<string> GetDownstreamBackboneTrace([FromRoute] int neighborhoodID)
        {
            var backboneDownstream = new List<BackboneSegment>();

            var neighborhoods = _dbContext.Neighborhood
                .Include(x => x.BackboneSegment);

            var backboneSegments = _dbContext.BackboneSegment
                .Include(x => x.DownstreamBackboneSegment)
                .ToList();

            var lookingAt = neighborhoods.Single(x => x.NeighborhoodID == neighborhoodID).BackboneSegment;

            while (lookingAt.Any())
            {
                backboneDownstream.AddRange(lookingAt);

                lookingAt = backboneSegments.Where(x => lookingAt.Contains(x) && x.DownstreamBackboneSegment != null)
                    .Select(x => x.DownstreamBackboneSegment)
                    .ToList()
                    .Distinct()
                    .ToList();
            }

            var featureList = backboneDownstream.Select(x =>
            {
                var geometry = UnaryUnionOp.Union(x.BackboneSegmentGeometry4326);
                var feature = new Feature() { Geometry = geometry, Attributes = new AttributesTable() };
                feature.Attributes.Add("dummy", "dummy");
                return feature;
            }).ToList();

            return Ok(buildFeatureCollectionAndWriteGeoJson(featureList));
        }

        private string buildFeatureCollectionAndWriteGeoJson(List<Feature> featureList)
        {
            var featureCollection = new FeatureCollection();

            foreach (var feature in featureList)
            {
                featureCollection.Add(feature);
            }

            var gjw = new GeoJsonWriter
            {
                SerializerSettings =
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    Formatting = Formatting.Indented
                }
            };

            return gjw.Write(featureCollection);
        }
    }
}
