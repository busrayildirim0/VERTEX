namespace VERTEX.Application.DTOs
{
    public class ChannelDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = null!;
        public required int WorkspaceId { get; set; }
    }
}
