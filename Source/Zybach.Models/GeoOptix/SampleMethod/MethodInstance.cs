using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class MethodInstance
    {
        [Required]
        [MinLength(1)]
        public List<RecordSet> RecordSets { get; set; }
        public List<string> ValidationErrors { get; set; }
        public MethodInstance()
        {
            RecordSets = new List<RecordSet>();
            ValidationErrors = new List<string>();
        }
    }
}