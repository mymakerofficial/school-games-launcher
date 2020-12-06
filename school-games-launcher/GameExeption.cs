using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class GameExeption
    {
        public Game Game { get; set; }
        public bool Allowed { get; set; }
        public GameExeption(Game game, bool allowed)
        {
            this.Game = game;
            this.Allowed = allowed;
        }
    }
}
