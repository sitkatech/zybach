using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class SampleMethodDto
    {
        public int SampleMethodID { get; set; }
        public int SampleID { get; set; }
        public string MethodSchemaCanonicalName { get; set; }
        public int MethodSchemaVersionNumber { get; set; }
        public MethodInstance MethodInstance { get; set; }
        public SampleMethodStatusDto Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserID { get; set; }


        public List<RecordSet> GetFlattenedRecordSets()
        {
            var result = new List<RecordSet>();
            var rootRecordSets = MethodInstance.RecordSets;
            result.AddRange(rootRecordSets);
            var nested = GetNestedRecordSets(result);
            result.AddRange(nested);
            return result;
        }

        public List<RecordSet> GetNestedRecordSets(List<RecordSet> recordSets)
        {
            var result = new List<RecordSet>();

            foreach (var recordSet in recordSets)
            {
                if (recordSet.Records != null && recordSet.Records.Any())
                {
                    foreach (var recordSetRecord in recordSet.Records)
                    {
                        if (recordSetRecord.RecordInstance.Fields != null && recordSetRecord.RecordInstance.Fields.Count > 0)
                        {
                            List<RecordSet> nestedRecordSets = recordSetRecord.RecordInstance.Fields
                                .Where(x => x.RawValue != null && x.RawValue.Type == JTokenType.Object)
                                .Select(x => x.RawValue.ToObject<RecordSet>())
                                .ToList();

                            result.AddRange(nestedRecordSets);
                            var nestedResults = GetNestedRecordSets(nestedRecordSets);
                            result.AddRange(nestedResults);
                        }
                    }

                }
            }

            return result;
        }

    }

    public class SampleMethodStatusDto
    {
        public int SampleMethodStatusID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

