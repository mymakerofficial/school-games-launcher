using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class Session
    {
        private Game game;
        private User user;
        private int startTimestamp;
        private int endTimestamp;
        public Session(Game game, User user)
        {
            this.game = game;
            this.user = user;
        }
    }
}
