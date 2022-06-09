//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicket]
namespace Zybach.EFModels.Entities
{
    public partial class SupportTicket
    {
        public SupportTicketStatus SupportTicketStatus => SupportTicketStatus.AllLookupDictionary[SupportTicketStatusID];
        public SupportTicketPriority SupportTicketPriority => SupportTicketPriority.AllLookupDictionary[SupportTicketPriorityID];
    }
}