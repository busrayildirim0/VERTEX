namespace VERTEX.Domain.Entities
{
    public class WorkspaceUser
    {
        public int WorkspaceId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = "Member";

        public Workspace Workspace { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
