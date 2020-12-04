using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace school_games_launcher
{
    public class GameExecutable
    {
        private string path;
        private Game game;
        public string Path { get { return path; } }
        public Game Game { get { return game; } }
        public GameExecutable(string path, Game game)
        {
            this.path = path;
            this.game = game;
        }
        /// <summary>
        /// Launches GameExecutable (if given user is allowed to play) and returns a Session.
        /// </summary>
        public Session Launch(User user)
        {
            if (user.AllowedToPlay(this.game))
            {
                Session session = new Session(this.game, user);
                var game = Process.Start(this.path);
                session.Start();
                return session;
            }else
            {
                return null;
            }
        }
        public void SetPath(string path)
        {
            this.path = path;
        }
    }
}
