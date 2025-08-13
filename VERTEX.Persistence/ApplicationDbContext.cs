using Microsoft.EntityFrameworkCore;
using VERTEX.Domain.Entities;

namespace VERTEX.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChannelUser> ChannelUsers { get; set; }
        public DbSet<WorkspaceUser> WorkspaceUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChannelUser>()
                .HasKey(cu => new { cu.ChannelId, cu.UserId });

            modelBuilder.Entity<ChannelUser>()
                .HasOne(cu => cu.Channel)
                .WithMany(c => c.ChannelUsers)
                .HasForeignKey(cu => cu.ChannelId);

            modelBuilder.Entity<ChannelUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.ChannelUsers)
                .HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<WorkspaceUser>()
                .HasKey(wu => new { wu.WorkspaceId, wu.UserId });

            modelBuilder.Entity<WorkspaceUser>()
                .HasOne(wu => wu.Workspace)
                .WithMany(w => w.WorkspaceUsers)
                .HasForeignKey(wu => wu.WorkspaceId);

            modelBuilder.Entity<WorkspaceUser>()
                .HasOne(wu => wu.User)
                .WithMany(u => u.WorkspaceUsers)
                .HasForeignKey(wu => wu.UserId);
        }
    }
}
