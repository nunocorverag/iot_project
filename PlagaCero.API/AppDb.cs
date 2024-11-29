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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = $"server=localhost;user={"erick"};password={"12917erick"};database={AppConfig.DatabaseName}";

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
        
        // Configuración personalizada para Test
        modelBuilder.Entity<Test>(e =>
        {
            e.Property(t => t.Name).IsRequired().HasMaxLength(128);
        });
    }
}
