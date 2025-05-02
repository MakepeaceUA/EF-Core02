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

                bool run = true;
                while (run)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1 - Поиск по названию игры");
                    Console.WriteLine("2 - Поиск по студии");
                    Console.WriteLine("3 - Поиск по названию и студии");
                    Console.WriteLine("4 - Поиск по жанру");
                    Console.WriteLine("5 - Поиск по году");
                    Console.WriteLine("6 - Все одиночные игры");
                    Console.WriteLine("7 - Все многопользовательские игры");
                    Console.WriteLine("8 - Игра с макс. продажами");
                    Console.WriteLine("9 - Игра с мин. продажами");
                    Console.WriteLine("10 - Топ-3 популярных игр");
                    Console.WriteLine("11 - Топ-3 непопулярных игр");
                    Console.WriteLine("12 - Добавить игру");
                    Console.WriteLine("13 - Изменить игру");
                    Console.WriteLine("14 - Удалить игру");
                    Console.WriteLine("0 - Выход");

                    var choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1": SearchName(context); break;
                        case "2": SearchStudio(context); break;
                        case "3": SearchNameStudio(context); break;
                        case "4": SearchGenre(context); break;
                        case "5": SearchYear(context); break;
                        case "6": ShowMode(context, "Singleplayer"); break;
                        case "7": ShowMode(context, "Multiplayer"); break;
                        case "8": ShowMaxCopies(context); break;
                        case "9": ShowMinCopies(context); break;
                        case "10": ShowTop(context, top: true); break;
                        case "11": ShowTop(context, top: false); break;
                        case "12": Add(context); break;
                        case "13": Update(context); break;
                        case "14": Delete(context); break;
                        case "0": run = false; break;
                    }

                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        static void SearchName(GameContext context)
        {
            Console.Write("Введите название игры: ");
            string name = Console.ReadLine();
            var res = context.Games.Where(g => g.Name.Contains(name)).ToList();
            PrintGames(res);
        }

        static void SearchStudio(GameContext context)
        {
            Console.Write("Введите студию: ");
            string studio = Console.ReadLine();
            var res = context.Games.Where(g => g.Developer.Contains(studio)).ToList();
            PrintGames(res);
        }

        static void SearchNameStudio(GameContext context)
        {
            Console.Write("Введите название игры: ");
            string name = Console.ReadLine();
            Console.Write("Введите студию: ");
            string studio = Console.ReadLine();
            var res = context.Games.Where(g => g.Name.Contains(name) && g.Developer.Contains(studio)).ToList();
            PrintGames(res);
        }

        static void SearchGenre(GameContext context)
        {
            Console.Write("Введите жанр: ");
            string genre = Console.ReadLine();
            var res = context.Games.Where(g => g.Genre.Contains(genre)).ToList();
            PrintGames(res);
        }

        static void SearchYear(GameContext context)
        {
            Console.Write("Введите год: ");
            int year = int.Parse(Console.ReadLine());
            var res = context.Games.Where(g => g.Date.Year == year).ToList();
            PrintGames(res);
        }

        static void ShowMode(GameContext context, string mode)
        {
            var res = context.Games.Where(g => g.GameMode == mode).ToList();
            PrintGames(res);
        }

        static void ShowMaxCopies(GameContext context)
        {
            var game = context.Games.OrderByDescending(g => g.Copies).FirstOrDefault();
            PrintGames(new List<GameLibrary> { game! });
        }

        static void ShowMinCopies(GameContext context)
        {
            var game = context.Games.OrderBy(g => g.Copies).FirstOrDefault();
            PrintGames(new List<GameLibrary> { game! });
        }

        static void ShowTop(GameContext context, bool top)
        {
            var res = top ? context.Games.OrderByDescending(g => g.Copies).Take(3).ToList()
                :context.Games.OrderBy(g => g.Copies).Take(3).ToList();
            PrintGames(res);
        }

        static void Add(GameContext context)
        {
            Console.Write("Название: ");
            string name = Console.ReadLine()!;
            Console.Write("Разработчик: ");
            string dev = Console.ReadLine()!;

            if (context.Games.Any(g => g.Name == name && g.Developer == dev))
            {
                Console.WriteLine("Такая игра уже существует.");
                return;
            }

            Console.Write("Жанр: ");
            string genre = Console.ReadLine()!;
            Console.Write("Дата (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Режим: ");
            string mode = Console.ReadLine()!;
            Console.Write("Копии: ");
            int copies = int.Parse(Console.ReadLine()!);

            context.Games.Add(new GameLibrary { Name = name, Developer = dev, Genre = genre, Date = date, GameMode = mode, Copies = copies });
            context.SaveChanges();
            Console.WriteLine("Игра добавлена.");
        }

        static void Update(GameContext context)
        {
            Console.Write("Название игры для редактирования: ");
            string name = Console.ReadLine()!;
            var game = context.Games.FirstOrDefault(g => g.Name == name);

            if (game == null)
            {
                Console.WriteLine("Игра не найдена.");
                return;
            }

            Console.Write("Новое название: ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                game.Name = newName;
            } 

            Console.Write("Новая студия: ");
            string dev = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dev))
            {
                game.Developer = dev;
            }

            Console.Write("Новый жанр: ");
            string? genre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genre))
            {
                game.Genre = genre;
            }
            Console.Write("Новая дата (yyyy-mm-dd): ");
            string date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date))
            {
                game.Date = DateTime.Parse(date);
            }

            Console.Write("Новый режим: ");
            string mode = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(mode))
            {
                game.GameMode = mode;
            }

            Console.Write("Новое количество копий: ");
            string copies = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(copies))
            {
                game.Copies = int.Parse(copies);
            }

            context.SaveChanges();
            Console.WriteLine("Игра обновлена.");
        }

        static void Delete(GameContext context)
        {
            Console.Write("Название игры: ");
            string name = Console.ReadLine()!;
            Console.Write("Разработчик: ");
            string dev = Console.ReadLine()!;

            var game = context.Games.FirstOrDefault(g => g.Name == name && g.Developer == dev);
            if (game == null)
            {
                Console.WriteLine("Игра не найдена.");
                return;
            }

            Console.Write("Удалить игру? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                context.Games.Remove(game);
                context.SaveChanges();
                Console.WriteLine("Игра удалена.");
            }
        }

        static void PrintGames(List<GameLibrary> games)
        {
            if (!games.Any())
            {
                Console.WriteLine("Игры не найдены.");
                return;
            }

            foreach (var game in games)
            {
                Console.WriteLine($"Название: {game.Name}\nРазработчик: {game.Developer}\nЖанр: {game.Genre}\nДата выхода: {game.Date.ToShortDateString()}\nРежим: {game.GameMode}\nПродано копий: {game.Copies}\n");
            }
        }
    }
}
