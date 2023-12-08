//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
namespace Zybach.EFModels.Entities
{
    public partial class Well
    {
        public int PrimaryKey => WellID;
        public County County => CountyID.HasValue ? County.AllLookupDictionary[CountyID.Value] : null;
        public WellUse WellUse => WellUseID.HasValue ? WellUse.AllLookupDictionary[WellUseID.Value] : null;

        public static class FieldLengths
        {
            public const int WellRegistrationID = 100;
            public const int WellNickname = 100;
            public const int TownshipRangeSection = 100;
            public const int Clearinghouse = 100;
            public const int SiteName = 100;
            public const int SiteNumber = 100;
            public const int ScreenInterval = 100;
            public const int OwnerName = 100;
            public const int OwnerAddress = 100;
            public const int OwnerCity = 100;
            public const int OwnerState = 20;
            public const int OwnerZipCode = 10;
            public const int AdditionalContactName = 100;
            public const int AdditionalContactAddress = 100;
            public const int AdditionalContactCity = 100;
            public const int AdditionalContactState = 20;
            public const int AdditionalContactZipCode = 10;
            public const int Notes = 1000;
        }
    }
}