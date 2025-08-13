using System.Collections.Generic;

namespace VERTEX.Domain.Entities
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Channel> Channels { get; set; } = new List<Channel>();
        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = new List<WorkspaceUser>();

    }
}