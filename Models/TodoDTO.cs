namespace OSB.Models
{
    public class TodoDTO
    {
        public long? Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}