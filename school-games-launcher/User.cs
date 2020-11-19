using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class User
    {
        private string name;
        private DateTime birthDate;
        private string passwordHash;
        private bool admin;
        private List<PlayPeriod> playPeriods = new List<PlayPeriod>();
        private List<GameExeption> gameExeptions = new List<GameExeption>();

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// The birthday of the user
        /// </summary>
        public DateTime BirthDate { get { return birthDate; } }
        /// <summary>
        /// Is admin?
        /// </summary>
        public bool Admin { get { return admin; } }
        /// <summary>
        /// List of the users play periods (when the user is allowed to play)
        /// </summary>
        public List<PlayPeriod> PlayPeriods { get { return playPeriods; } }
        /// <summary>
        /// List of the users game exeptions (overrides if user is allowed to play a game)
        /// </summary>
        public List<GameExeption> GameExeptions { get { return gameExeptions; } }
        /// <summary>
        /// Calculated age of this user.
        /// </summary>
        public int Age
        {
            get {
                TimeSpan difference = DateTime.Now - this.BirthDate;
                int years = (new DateTime(0) + difference).Year - 1;
                return years;
            }
        }
        /// <summary>
        /// A user is a humon being 
        /// </summary>
        public User(string name, int birthTimestamp, string passwordHash, bool admin)
        {
            this.name = name;
            this.birthDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(birthTimestamp);
            this.passwordHash = passwordHash;
            this.admin = admin;
        }
        /// <summary>
        /// Checks if this user is allowed to play the given game.
        /// </summary>
        public bool AllowedToPlay(Game game)
        {
            bool allowed = false; // nobody is allowed to play games... untill they are.
            if (this.Age >= game.Age) allowed = true; // is user old enough?
            if (this.Admin == true) allowed = true; // admins are allowed to play every game!!!!
            return allowed;
        }
        /// <summary>
        /// Checks if given password is correct
        /// </summary>
        public bool VerifyPassword(string password)
        {
            bool valid = true;
            // you should check if the password is correct lol
            return valid;
        }
    }
}