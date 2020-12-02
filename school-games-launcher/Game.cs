using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class Game
    {
        private int id;
        private string name;
        private int age;
        private string coverart;
        public int? SteamId { get; set; } = null;
        private GameExecutable executable;
        private DateTime lastPlayed;

        /// <summary>
        /// Id of the game
        /// </summary>
        public int Id { get { return id; } }
        /// <summary>
        /// Whats the name of the game?
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// The age a user needs to be to play this game.
        /// </summary>
        public int Age { get { return age; } }
        /// <summary>
        /// The game executable
        /// </summary>
        public GameExecutable Executable { get { return executable; } }
        /// <summary>
        /// The url to the coverart
        /// </summary>
        public string Coverart { get { return coverart; } }

        public Game(int id, string name, string path, int age, string coverart)
        {
            this.id = id;
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
