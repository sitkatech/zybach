using System.Collections.Generic;

namespace Zybach.EFModels.Entities
{
    public partial class Well
    {
        public double Longitude => WellGeometry.Coordinate.X;
        public double Latitude => WellGeometry.Coordinate.Y;
    }
}