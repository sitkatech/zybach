using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;

namespace Zybach.Models.DataTransferObjects
{
    public class BoundingBoxDto
    {
        public double Left;
        public double Bottom;
        public double Right;
        public double Top;

        public BoundingBoxDto(IReadOnlyCollection<Point> pointList)
        {
            if (pointList.Any())
            {
                Left = pointList.Min(x => x.X);
                Right = pointList.Max(x => x.X);
                Bottom = pointList.Min(x => x.Y);
                Top = pointList.Max(x => x.Y);
            }
            else
            {
                Left = -100.22425584641142;
                Right = -102.05544891242484;
                Top = 40.878401166400693;
                Bottom = 41.73706831826739;
            }
        }


        public BoundingBoxDto(IEnumerable<Geometry> geometries) : this(geometries.SelectMany(GetPointsFromDbGeometry).ToList())
        {
        }

        public static List<Point> GetPointsFromDbGeometry(Geometry geometry)
        {
            var pointList = new List<Point>();
            var envelope = geometry.EnvelopeInternal;
            pointList.Add(new Point(envelope.MinX, envelope.MinY));
            pointList.Add(new Point(envelope.MaxX, envelope.MaxY));
            return pointList;
        }
    }
}