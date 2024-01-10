//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketComment]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketCommentDto
    {
        public int SupportTicketCommentID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public UserDto CreatorUser { get; set; }
        public SupportTicketDto SupportTicket { get; set; }
        public string CommentNotes { get; set; }
    }

    public partial class SupportTicketCommentSimpleDto
    {
        public int SupportTicketCommentID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public System.Int32 CreatorUserID { get; set; }
        public System.Int32 SupportTicketID { get; set; }
        public string CommentNotes { get; set; }
    }

}