using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;

namespace LibrarySystem.Data;

public class MysqlDbContext : DbContext
{
    public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options)
    {
    }

    public DbSet<User> users { get; set; }
    public DbSet<Book> books { get; set; }
    public DbSet<Loan> loans { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasIndex(l => new { l.UserId, l.BookId })
                .IsUnique();

            entity.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId) 
                .OnDelete(DeleteBehavior.Restrict);
        });
        
    }
}