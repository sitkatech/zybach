using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class UserExtensionMethods
    {
        static partial void DoCustomMappings(User user, UserDto userDto)
        {
            userDto.FullName = user.FullName;
            userDto.FullNameLastFirst = user.FullNameLastFirst;
        }

        static partial void DoCustomSimpleDtoMappings(User user, UserSimpleDto userSimpleDto)
        {
            userSimpleDto.FullName = user.FullName;
            userSimpleDto.FullNameLastFirst = user.FullNameLastFirst;
        }
    }
}