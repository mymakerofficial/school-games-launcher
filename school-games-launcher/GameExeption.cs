using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class GameExeption
    {
        private Game game;
        private bool allowed;
        public GameExeption(Game game, bool allowed)
        {
            this.game = game;
            this.allowed = allowed;
        }
    }
}
