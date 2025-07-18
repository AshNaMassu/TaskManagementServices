namespace Domain.Entities
{
    public class ActivityLog
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
        public DateTime EventTime { get; set; }
    }
}