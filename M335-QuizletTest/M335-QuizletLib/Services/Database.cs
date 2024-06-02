using Microsoft.EntityFrameworkCore;
using M335_QuizletLib.Models;

namespace M335_QuizletLib.Services
{
    public class Database : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public Database()
        {
            this.Database.EnsureCreatedAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "database.sqlite");
            options.UseSqlite($"Filename={dbPath}");
        }
    }
}