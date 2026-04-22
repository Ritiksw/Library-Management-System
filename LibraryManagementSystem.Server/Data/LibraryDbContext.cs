using LibraryManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Server.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(b => b.ISBN).IsUnique();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(m => m.Email).IsUnique();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", Genre = "Fiction", PublishedYear = 1925, TotalCopies = 3, AvailableCopies = 3 },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0061120084", Genre = "Fiction", PublishedYear = 1960, TotalCopies = 2, AvailableCopies = 2 },
            new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", Genre = "Dystopian", PublishedYear = 1949, TotalCopies = 4, AvailableCopies = 4 },
            new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518", Genre = "Romance", PublishedYear = 1813, TotalCopies = 2, AvailableCopies = 2 },
            new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "978-0316769488", Genre = "Fiction", PublishedYear = 1951, TotalCopies = 3, AvailableCopies = 3 },
            new Book { Id = 6, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Genre = "Technology", PublishedYear = 2008, TotalCopies = 2, AvailableCopies = 2 },
            new Book { Id = 7, Title = "Design Patterns", Author = "Gang of Four", ISBN = "978-0201633610", Genre = "Technology", PublishedYear = 1994, TotalCopies = 1, AvailableCopies = 1 },
            new Book { Id = 8, Title = "The Hobbit", Author = "J.R.R. Tolkien", ISBN = "978-0547928227", Genre = "Fantasy", PublishedYear = 1937, TotalCopies = 3, AvailableCopies = 3 }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, FullName = "Alice Johnson", Email = "alice@example.com", Phone = "555-0101", MembershipDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Member { Id = 2, FullName = "Bob Smith", Email = "bob@example.com", Phone = "555-0102", MembershipDate = new DateTime(2024, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Member { Id = 3, FullName = "Carol Davis", Email = "carol@example.com", Phone = "555-0103", MembershipDate = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Member { Id = 4, FullName = "David Wilson", Email = "david@example.com", Phone = "555-0104", MembershipDate = new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
