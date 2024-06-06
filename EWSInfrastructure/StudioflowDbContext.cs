using EWSDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EWSInfrastructure;

public class StudioflowDbContext : DbContext
{
    public StudioflowDbContext(DbContextOptions<StudioflowDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<ProjectFile> Files { get; set; }
}