using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp55
{
    public class GameLibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Developer { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public DateTime Date { get; set; }

        public string GameMode { get; set; } = "Singleplayer"; 
        public int Copies { get; set; }
    }
}
