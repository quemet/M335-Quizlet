using Microsoft.EntityFrameworkCore;
using M335_Quizlet.Models;

namespace M335_Quizlet.Services
{
    public class Database : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }

        public Database()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "database.sqlite");
            options.UseSqlite($"Filename={dbPath}");
        }
    }
}