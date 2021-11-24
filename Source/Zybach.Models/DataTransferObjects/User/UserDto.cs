namespace Zybach.Models.DataTransferObjects
{
    public partial class UserDto
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}