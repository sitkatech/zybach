using System;

namespace Zybach.Models.GeoOptix
{
    public class Station
    {
        public string SiteCanonicalName { get; set; }
        public string Name { get; set; }
        public Definition Definition { get; set; }
        public DateTime CreateDate { get; set; }
    }
}