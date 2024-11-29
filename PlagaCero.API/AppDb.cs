using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options)
        : base(options)
    { }
    
    public DbSet<Test> Tests { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public DbSet<Plant> Plants { get; set; }
    public DbSet<PlantTemperature> PlantTemperatures { get; set; }
    public DbSet<PlantState> PlantStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var DbUser = Environment.GetEnvironmentVariable(AppConfig.DbUserKey)
            ?? throw new Exception($"environment variable '{AppConfig.DbUserKey}' is not set");

        var DbPassword = Environment.GetEnvironmentVariable(AppConfig.DbPasswordKey)
            ?? throw new Exception($"environment variable '{AppConfig.DbPasswordKey}' is not set");

        var connectionString = $"server=localhost;port=8889;user={DbUser};password={DbPassword};database={AppConfig.DatabaseName}";
                optionsBuilder.UseMySql(connectionString, AppConfig.MySqlServerVersion);

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.None); // Disable query logging
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación de uno a muchos entre Author y Book
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);

        // Relación de muchos a muchos entre Student y Course
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity(j => j.ToTable("StudentCourses"));

        // Relación de uno a muchos entre Plant y PlantTemperature
        modelBuilder.Entity<Plant>()
            .HasMany(p => p.PlantTemperatures)
            .WithOne(pt => pt.Plant)
            .HasForeignKey(pt => pt.PlantId)
            .OnDelete(DeleteBehavior.Cascade);  // Si una planta se elimina, se eliminan sus temperaturas

        // Relación de uno a muchos entre Plant y PlantState
        modelBuilder.Entity<Plant>()
            .HasMany(p => p.PlantStates)
            .WithOne(ps => ps.Plant)
            .HasForeignKey(ps => ps.PlantId)
            .OnDelete(DeleteBehavior.Cascade);  // Si una planta se elimina, se eliminan sus estados

        // Configuración personalizada para Test
        modelBuilder.Entity<Test>(e =>
        {
            e.Property(t => t.Name).IsRequired().HasMaxLength(128);
        });
    }
}
