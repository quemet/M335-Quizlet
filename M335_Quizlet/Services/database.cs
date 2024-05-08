using M335_Quizlet.Models;
using Microsoft.EntityFrameworkCore;

namespace M335_Quizlet.Services
{
    public class Database : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public Database()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "database.sqlite");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}