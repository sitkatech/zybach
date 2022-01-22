using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WellContactInfoDto
    {
        public string WellRegistrationID { get; set; }
        [StringLength(100, ErrorMessage = "TRS cannot exceed 100 characters. ")]
        public string TownshipRangeSection { get; set; }
        public int? CountyID { get; set; }
        public string County { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters. ")]
        public string OwnerName { get; set; }
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters. ")]
        public string OwnerAddress { get; set; }
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters. ")]
        public string OwnerCity { get; set; }
        [StringLength(20, ErrorMessage = "State cannot exceed 20 characters. ")]
        public string OwnerState { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip codes must be formatted in either 5 digit or hyphenated 5+4 digit format")]
        public string OwnerZipCode { get; set; }


        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters. ")]
        public string AdditionalContactName { get; set; }
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters. ")]
        public string AdditionalContactAddress { get; set; }
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters. ")]
        public string AdditionalContactCity { get; set; }
        [StringLength(20, ErrorMessage = "State cannot exceed 20 characters. ")]
        public string AdditionalContactState { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Zip codes must be formatted in either 5 digit or hyphenated 5+4 digit format")]
        public string AdditionalContactZipCode { get; set; }

        [StringLength(100, ErrorMessage = "Well Nickname cannot exceed 100 characters. ")]
        public string WellNickname { get; set; }
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters. ")]
        public string Notes { get; set; }
    }
}
