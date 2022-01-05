namespace Zybach.Models.DataTransferObjects
{
    public class WellContactInfoDto
    {
        public string TownshipRangeSection { get; set; }
        public int CountyID { get; set; }
        public string County { get; set; }

        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZipCode { get; set; }

        public string AdditionalContactName { get; set; }
        public string AdditionalContactAddress { get; set; }
        public string AdditionalContactCity { get; set; }
        public string AdditionalContactState { get; set; }
        public string AdditionalContactZipCode { get; set; }

        public string WellNickname { get; set; }
        public string Notes { get; set; }
    }
}
