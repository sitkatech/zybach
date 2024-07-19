//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[BlobResource]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class BlobResourceDto
    {
        public int BlobResourceID { get; set; }
        public Guid BlobResourceGUID { get; set; }
        public string BlobResourceCanonicalName { get; set; }
        public string OriginalBaseFilename { get; set; }
        public string OriginalFileExtension { get; set; }
        public UserDto CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public partial class BlobResourceSimpleDto
    {
        public int BlobResourceID { get; set; }
        public Guid BlobResourceGUID { get; set; }
        public string BlobResourceCanonicalName { get; set; }
        public string OriginalBaseFilename { get; set; }
        public string OriginalFileExtension { get; set; }
        public System.Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
    }

}