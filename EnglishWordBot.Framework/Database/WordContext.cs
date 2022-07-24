using EnglishWordBot.Framework.Database.Entities.Users;
using EnglishWordBot.Framework.Database.Entities.UserWordMappings;
using EnglishWordBot.Framework.Database.Entities.Words;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Database;

public class WordContext : DbContext
{
    public WordContext(DbContextOptions<WordContext> options) : base(options)
    {
    }

    public DbSet<Word> Words { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserWordMapping> UserWordMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>(e =>
        {
            e.OwnsMany(w => w.Translations);
            e.HasIndex(w => w.Value).IsUnique();
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.ChatId).IsUnique();
        });

        modelBuilder.Entity<UserWordMapping>(e =>
        {
            e.HasIndex(uwm => new
            {
                uwm.UserId,
                uwm.WordId
            }).IsUnique();
        });
    }
}