//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[State]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class StateDto
    {
        public long Id { get; set; }
        public JobDto Job { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Data { get; set; }
    }
}