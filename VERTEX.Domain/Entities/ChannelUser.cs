namespace VERTEX.Domain.Entities
{
    public class ChannelUser
    {
        public int ChannelId { get; set; }
        public int UserId { get; set; }

        public Channel Channel { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
