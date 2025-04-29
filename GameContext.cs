using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace ConsoleApp55
{
    public class GameContext : DbContext
    {
        public DbSet<GameLibrary> Games => Set<GameLibrary>();

        public GameContext() => Database.EnsureCreated();

        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=ARSEN;Database=GamesDB;Integrated Security=True;TrustServerCertificate=True;");
            }
        }
    }
}
