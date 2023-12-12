//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FieldDefinition]
namespace Zybach.EFModels.Entities
{
    public partial class FieldDefinition
    {
        public int PrimaryKey => FieldDefinitionID;
        public FieldDefinitionType FieldDefinitionType => FieldDefinitionType.AllLookupDictionary[FieldDefinitionTypeID];

        public static class FieldLengths
        {

        }
    }
}