//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Schema]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SchemaExtensionMethods
    {
        public static SchemaDto AsDto(this Schema schema)
        {
            var schemaDto = new SchemaDto()
            {
                Version = schema.Version
            };
            DoCustomMappings(schema, schemaDto);
            return schemaDto;
        }

        static partial void DoCustomMappings(Schema schema, SchemaDto schemaDto);

    }
}