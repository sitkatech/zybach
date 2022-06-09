namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketSimpleDto
    {
        public WellMinimalDto Well { get; set; }
        public SensorSimpleDto Sensor { get; set; }
        public UserSimpleDto CreatorUser { get; set; }
        public UserSimpleDto AssigneeUser { get; set; }
        public SupportTicketStatusSimpleDto Status{ get; set; }
        public SupportTicketPrioritySimpleDto Priority{ get; set; }
    }
}
