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
        private Image libaryIcon;
        private Image libaryCabsule;
        private Image libaryHero;
        private Image libaryLogo;
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

        public Game(string name, string path, int age)
        {
            this.name = name;
            this.age = age;
            this.executable = new GameExecutable(path, this);
        }
        /// <summary>
        /// Launches GameExecutable (if given user is allowed to play) and returns a Session.
        /// </summary>
        public Session Launch(User user) => this.executable.Launch(user);
    }
}
