using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class Game
    {
        private string name;
        private int age;
        private string coverart;
        private GameExecutable executable;
        private DateTime lastPlayed;

        /// <summary>
        /// Whats the name of the game?
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// The age a user needs to be to play this game.
        /// </summary>
        public int Age { get { return age; } }
        /// <summary>
        /// The url to the coverart
        /// </summary>
        public string Coverart { get { return coverart; } }

        public Game(string name, string path, int age, string coverart)
        {
            this.name = name;
            this.age = age;
            this.coverart = coverart;
            this.executable = new GameExecutable(path, this);
        }
        /// <summary>
        /// Launches GameExecutable (if given user is allowed to play) and returns a Session.
        /// </summary>
        public Session Launch(User user) => this.executable.Launch(user);
    }
}
