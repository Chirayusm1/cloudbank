using CloudBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudBank.Infrastructure.Persistence;

public class CloudBankDbContext : DbContext
{
    public CloudBankDbContext(DbContextOptions<CloudBankDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder);
        ConfigureAccount(modelBuilder);
        ConfigureTransaction(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            entity.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });
    }

    private static void ConfigureAccount(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Balance)
                .HasPrecision(18, 2);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(x => x.AccountNumber)
                .IsUnique();

            entity.HasOne(x => x.User)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(x => x.FromAccount)
                .WithMany(x => x.OutgoingTransactions)
                .HasForeignKey(x => x.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ToAccount)
                .WithMany(x => x.IncomingTransactions)
                .HasForeignKey(x => x.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.CreatedAt);
        });
    }
}