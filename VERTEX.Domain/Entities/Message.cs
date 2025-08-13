namespace VERTEX.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ChannelId { get; set; }
        public int UserId { get; set; }

        public Channel Channel { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
