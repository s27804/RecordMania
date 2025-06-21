using Microsoft.EntityFrameworkCore;
using RecordMania.Models;
using Task = RecordMania.Models.Task;

namespace RecordMania.DbContexts;

public class RecordManiaDbContext : DbContext
{
    public DbSet<Task> Task  { get; set; }
    public DbSet<Language> Language { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Record> Record { get; set; }

    public RecordManiaDbContext(DbContextOptions<RecordManiaDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>().HasKey(x => x.Id);
        modelBuilder.Entity<Language>().HasKey(x => x.Id);
        modelBuilder.Entity<Student>().HasKey(x => x.Id);
        modelBuilder.Entity<Record>().HasKey(x => x.Id);
    }
}