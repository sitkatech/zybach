namespace Zybach.EFModels.Entities
{
    public partial class User
    {
        public string FullName => $"{FirstName} {LastName}";
        public string FullNameLastFirst => $"{LastName}, {FirstName}";
    }
}