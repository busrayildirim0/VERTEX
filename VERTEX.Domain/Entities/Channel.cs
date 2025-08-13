namespace VERTEX.Domain.Entities
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }

        public Workspace Workspace { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();
    }
}
