//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[State]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class StateExtensionMethods
    {
        public static StateDto AsDto(this State state)
        {
            var stateDto = new StateDto()
            {
                Id = state.Id,
                Job = state.Job.AsDto(),
                Name = state.Name,
                Reason = state.Reason,
                CreatedAt = state.CreatedAt,
                Data = state.Data
            };
            DoCustomMappings(state, stateDto);
            return stateDto;
        }

        static partial void DoCustomMappings(State state, StateDto stateDto);

    }
}