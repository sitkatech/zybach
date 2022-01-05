//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public StreamFlowZoneDto StreamflowZone { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string WellNickname { get; set; }
        public string TownshipRangeSection { get; set; }
        public CountyDto County { get; set; }
        public WellParticipationDto WellParticipation { get; set; }
        public WellUseDto WellUse { get; set; }
        public bool RequiresChemigation { get; set; }
        public bool RequiresWaterLevelInspection { get; set; }
        public decimal? WellDepth { get; set; }
        public string Clearinghouse { get; set; }
        public int? PageNumber { get; set; }
        public string SiteName { get; set; }
        public string SiteNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZipCode { get; set; }
        public string AdditionalContactName { get; set; }
        public string AdditionalContactAddress { get; set; }
        public string AdditionalContactCity { get; set; }
        public string AdditionalContactState { get; set; }
        public string AdditionalContactZipCode { get; set; }
        public bool IsReplacement { get; set; }
        public string Notes { get; set; }
    }

    public partial class WellSimpleDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public int? StreamflowZoneID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string WellNickname { get; set; }
        public string TownshipRangeSection { get; set; }
        public int? CountyID { get; set; }
        public int? WellParticipationID { get; set; }
        public int? WellUseID { get; set; }
        public bool RequiresChemigation { get; set; }
        public bool RequiresWaterLevelInspection { get; set; }
        public decimal? WellDepth { get; set; }
        public string Clearinghouse { get; set; }
        public int? PageNumber { get; set; }
        public string SiteName { get; set; }
        public string SiteNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZipCode { get; set; }
        public string AdditionalContactName { get; set; }
        public string AdditionalContactAddress { get; set; }
        public string AdditionalContactCity { get; set; }
        public string AdditionalContactState { get; set; }
        public string AdditionalContactZipCode { get; set; }
        public bool IsReplacement { get; set; }
        public string Notes { get; set; }
    }

}