namespace VERTEX.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();
        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = new List<WorkspaceUser>();
    }
}