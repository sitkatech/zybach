//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
namespace Zybach.EFModels.Entities
{
    public partial class Well
    {
        public County County => CountyID.HasValue ? County.AllLookupDictionary[CountyID.Value] : null;
        public WellUse WellUse => WellUseID.HasValue ? WellUse.AllLookupDictionary[WellUseID.Value] : null;
    }
}