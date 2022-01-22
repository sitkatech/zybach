//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellExtensionMethods
    {
        public static WellDto AsDto(this Well well)
        {
            var wellDto = new WellDto()
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                StreamflowZone = well.StreamflowZone?.AsDto(),
                CreateDate = well.CreateDate,
                LastUpdateDate = well.LastUpdateDate,
                WellNickname = well.WellNickname,
                TownshipRangeSection = well.TownshipRangeSection,
                County = well.County?.AsDto(),
                WellParticipation = well.WellParticipation?.AsDto(),
                WellUse = well.WellUse?.AsDto(),
                RequiresChemigation = well.RequiresChemigation,
                RequiresWaterLevelInspection = well.RequiresWaterLevelInspection,
                WellDepth = well.WellDepth,
                Clearinghouse = well.Clearinghouse,
                PageNumber = well.PageNumber,
                SiteName = well.SiteName,
                SiteNumber = well.SiteNumber,
                ScreenInterval = well.ScreenInterval,
                ScreenDepth = well.ScreenDepth,
                OwnerName = well.OwnerName,
                OwnerAddress = well.OwnerAddress,
                OwnerCity = well.OwnerCity,
                OwnerState = well.OwnerState,
                OwnerZipCode = well.OwnerZipCode,
                AdditionalContactName = well.AdditionalContactName,
                AdditionalContactAddress = well.AdditionalContactAddress,
                AdditionalContactCity = well.AdditionalContactCity,
                AdditionalContactState = well.AdditionalContactState,
                AdditionalContactZipCode = well.AdditionalContactZipCode,
                IsReplacement = well.IsReplacement,
                Notes = well.Notes
            };
            DoCustomMappings(well, wellDto);
            return wellDto;
        }

        static partial void DoCustomMappings(Well well, WellDto wellDto);

        public static WellSimpleDto AsSimpleDto(this Well well)
        {
            var wellSimpleDto = new WellSimpleDto()
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                StreamflowZoneID = well.StreamflowZoneID,
                CreateDate = well.CreateDate,
                LastUpdateDate = well.LastUpdateDate,
                WellNickname = well.WellNickname,
                TownshipRangeSection = well.TownshipRangeSection,
                CountyID = well.CountyID,
                WellParticipationID = well.WellParticipationID,
                WellUseID = well.WellUseID,
                RequiresChemigation = well.RequiresChemigation,
                RequiresWaterLevelInspection = well.RequiresWaterLevelInspection,
                WellDepth = well.WellDepth,
                Clearinghouse = well.Clearinghouse,
                PageNumber = well.PageNumber,
                SiteName = well.SiteName,
                SiteNumber = well.SiteNumber,
                ScreenInterval = well.ScreenInterval,
                ScreenDepth = well.ScreenDepth,
                OwnerName = well.OwnerName,
                OwnerAddress = well.OwnerAddress,
                OwnerCity = well.OwnerCity,
                OwnerState = well.OwnerState,
                OwnerZipCode = well.OwnerZipCode,
                AdditionalContactName = well.AdditionalContactName,
                AdditionalContactAddress = well.AdditionalContactAddress,
                AdditionalContactCity = well.AdditionalContactCity,
                AdditionalContactState = well.AdditionalContactState,
                AdditionalContactZipCode = well.AdditionalContactZipCode,
                IsReplacement = well.IsReplacement,
                Notes = well.Notes
            };
            DoCustomSimpleDtoMappings(well, wellSimpleDto);
            return wellSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(Well well, WellSimpleDto wellSimpleDto);
    }
}