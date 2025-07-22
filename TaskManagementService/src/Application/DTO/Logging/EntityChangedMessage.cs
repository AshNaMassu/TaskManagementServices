namespace Application.DTO.Logging
{
    public class EntityChangedMessage
    {
        public string Operation { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
    }
}
