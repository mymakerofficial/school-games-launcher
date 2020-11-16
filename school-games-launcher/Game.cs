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

        public string Name => name;

        public Game(string name, string path, int age)
        {
            this.name = name;
            this.age = age;
            this.executable = new GameExecutable(path, this);
        }

        public Session Launch(User user) => this.executable.Launch(user);
    }
}
