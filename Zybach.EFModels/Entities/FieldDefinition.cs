using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class FieldDefinition
    {
        public static List<FieldDefinitionDto> List(ZybachDbContext dbContext)
        {
            return dbContext.FieldDefinitions.Include(x => x.FieldDefinitionType).Select(x => x.AsDto()).ToList();
        }

        public static FieldDefinitionDto GetByFieldDefinitionTypeID(ZybachDbContext dbContext, int fieldDefinitionTypeID)
        {
            var fieldDefinition = dbContext.FieldDefinitions
                .Include(x => x.FieldDefinitionType)
                .SingleOrDefault(x => x.FieldDefinitionTypeID == fieldDefinitionTypeID);

            return fieldDefinition?.AsDto();
        }

        public static FieldDefinitionDto UpdateFieldDefinition(ZybachDbContext dbContext, int fieldDefinitionTypeID,
            FieldDefinitionDto fieldDefinitionUpdateDto)
        {
            var fieldDefinition = dbContext.FieldDefinitions
                .Include(x => x.FieldDefinitionType)
                .SingleOrDefault(x => x.FieldDefinitionTypeID == fieldDefinitionTypeID);

            // null check occurs in calling endpoint method.
            fieldDefinition.FieldDefinitionValue = fieldDefinitionUpdateDto.FieldDefinitionValue;

            dbContext.SaveChanges();

            return fieldDefinition.AsDto();
        }
    }
}