namespace VERTEX.Application.DTOs
{
    public class MessageDto
    {
        public required int Id { get; set; }
        public required string Content { get; set; } = null!;
        public required DateTime CreatedAt { get; set; }
        public required int ChannelId { get; set; }
        public required int UserId { get; set; }
    }
}
