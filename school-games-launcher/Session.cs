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
        public Game Game { get { return game; } }
        public User User { get { return user; } }

        public Session(Game game, User user)
        {
            this.game = game;
            this.user = user;
        }
        public void Start()
        {
            Program.app.Gui.playing.Activate();
        }
        public void End()
        {
            Program.app.Gui.library.Activate();
        }
    }
}
