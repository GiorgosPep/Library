using LibraryAppModelFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAppModelFirst.Data;

public class LibDbContext : DbContext
{
    public LibDbContext(DbContextOptions<LibDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Capability> Capabilities => Set<Capability>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Author ----------
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(a => a.Name).IsRequired().HasMaxLength(150);
            entity.Property(a => a.Country).HasMaxLength(100);
            entity.HasQueryFilter(a => !a.IsDeleted);
        });

        // ---------- Book ----------
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title).IsRequired().HasMaxLength(250);
            entity.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(b => !b.IsDeleted);
        });

        // ---------- User ----------
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Firstname).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Lastname).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Role ----------
        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(r => r.Name).IsUnique();
            entity.HasMany(r => r.Capabilities)
                  .WithMany(c => c.Roles)
                  .UsingEntity<Dictionary<string, object>>(
                      "RoleCapabilities",
                      j => j.HasOne<Capability>().WithMany().HasForeignKey("CapabilityId"),
                      j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"));
        });

        // ---------- Capability ----------
        modelBuilder.Entity<Capability>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).HasMaxLength(250);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        // ---------- Loan ----------
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasOne(l => l.Book)
                  .WithMany(b => b.Loans)
                  .HasForeignKey(l => l.BookId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(l => l.User)
                  .WithMany(u => u.Loans)
                  .HasForeignKey(l => l.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(l => !l.IsDeleted);
        });

        // ---------- Seed: Roles ----------
        modelBuilder.Entity<Role>().HasData(
            new { Id = 1, Name = "Admin" },
            new { Id = 2, Name = "Librarian" },
            new { Id = 3, Name = "Member" }
        );

        // ---------- Seed: Capabilities ----------
        modelBuilder.Entity<Capability>().HasData(
            new { Id = 1, Name = "Books.View", Description = "View the book catalog" },
            new { Id = 2, Name = "Books.Manage", Description = "Create, edit and delete books" },
            new { Id = 3, Name = "Authors.Manage", Description = "Create, edit and delete authors" },
            new { Id = 4, Name = "Loans.ViewOwn", Description = "View own loans" },
            new { Id = 5, Name = "Loans.ViewAll", Description = "View all loans" },
            new { Id = 6, Name = "Loans.Manage", Description = "Create loans and register returns" },
            new { Id = 7, Name = "Users.Manage", Description = "Manage users" },
            new { Id = 8, Name = "Roles.Manage", Description = "Manage roles and capabilities" }
        );

        // ---------- Seed: Role <-> Capability ----------
        modelBuilder.Entity("RoleCapabilities").HasData(
            // Admin: all
            new { RoleId = 1, CapabilityId = 1 },
            new { RoleId = 1, CapabilityId = 2 },
            new { RoleId = 1, CapabilityId = 3 },
            new { RoleId = 1, CapabilityId = 4 },
            new { RoleId = 1, CapabilityId = 5 },
            new { RoleId = 1, CapabilityId = 6 },
            new { RoleId = 1, CapabilityId = 7 },
            new { RoleId = 1, CapabilityId = 8 },
            // Librarian
            new { RoleId = 2, CapabilityId = 1 },
            new { RoleId = 2, CapabilityId = 2 },
            new { RoleId = 2, CapabilityId = 3 },
            new { RoleId = 2, CapabilityId = 5 },
            new { RoleId = 2, CapabilityId = 6 },
            // Member
            new { RoleId = 3, CapabilityId = 1 },
            new { RoleId = 3, CapabilityId = 4 }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Updated = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}