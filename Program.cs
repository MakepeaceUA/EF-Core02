using Microsoft.EntityFrameworkCore;

namespace ConsoleApp55
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameContext>();
            optionsBuilder.UseSqlServer("Server=ARSEN;Database=GameDB;Integrated Security=True;TrustServerCertificate=True;");

            using (var context = new GameContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();

                if (!context.Games.Any())
                {
                    context.Games.AddRange(
                        new GameLibrary
                        {
                            Name = "The Witcher 3",
                            Developer = "CD Projekt",
                            Genre = "RPG",
                            Date = new DateTime(2015, 1, 01),
                            GameMode = "Singleplayer",
                            Copies = 30000000
                        },
                        new GameLibrary
                        {
                            Name = "World of Warcraft",
                            Developer = "Blizzard Entertainment",
                            Genre = "MMO RPG",
                            Date = new DateTime(2004, 1, 01),
                            GameMode = "Multiplayer",
                            Copies = 40000000
                        }
                    );
                    context.SaveChanges();
                }

                var games = context.Games.ToList();
                foreach (var game in games)
                {
                    Console.WriteLine($"Название: {game.Name}\n Разработчик: {game.Developer}\n Жанр: {game.Genre}\n Дата выхода: {game.Date.ToShortDateString()}\n Игровой мод: {game.GameMode}\n Продано копий: {game.Copies}\n\n");
                }
            }
        }
    }
}
