//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicket]
namespace Zybach.EFModels.Entities
{
    public partial class SupportTicket
    {
        public int PrimaryKey => SupportTicketID;
        public SupportTicketStatus SupportTicketStatus => SupportTicketStatus.AllLookupDictionary[SupportTicketStatusID];
        public SupportTicketPriority SupportTicketPriority => SupportTicketPriority.AllLookupDictionary[SupportTicketPriorityID];

        public static class FieldLengths
        {
            public const int SupportTicketTitle = 100;
            public const int SupportTicketDescription = 500;
        }
    }
}