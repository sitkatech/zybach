using System;
using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace Zybach.Models.GeoOptix
{
    public class Site
    {
        public string CanonicalName { get; set; }
        public string Description { get; set; }
        public Feature Location { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<string> Tags { get; set; }
    }
}