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
        private int id;
        private string name;
        private DateTime birthDate;
        private string passwordHash;
        private bool admin;
        private List<PlayPeriod> playPeriods = new List<PlayPeriod>();
        private List<GameExeption> gameExeptions = new List<GameExeption>();

        /// <summary>
        /// Id of the user
        /// </summary>
        public int Id { get { return id; } }
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
        /// Is this user currently allowed to play.
        /// </summary>
        public bool InPlayPeriod
        {
            get
            {
                foreach (PlayPeriod period in this.PlayPeriods)
                {
                    if(period.IsActive) return true;
                }
                return this.Admin;
            }
        }
        /// <summary>
        /// A user is a humon being 
        /// </summary>
        public User(int id, string name, int birthTimestamp, string passwordHash, bool admin)
        {
            this.id = id;
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
            if (this.Admin == true) return true; // admins are allowed to play every game!!!!

            bool allowed = true; // nobody is allowed to play games... untill they are.

            if (this.Age < game.Age) allowed = false; // is user old enough?

            GameExeption exeption = this.GetGameExeption(game);
            if (exeption != null) allowed = exeption.Allowed; // a game exeption overrides every condition exept admin and playperiod

            if (!this.InPlayPeriod) allowed = false; // players are only allowed to play if a play period is active
            return allowed;
        }
        public GameExeption GetGameExeption(Game game)
        {
            foreach(GameExeption exeption in this.gameExeptions)
            {
                if (exeption.Game == game) return exeption;
            }
            return null;
        }
        /// <summary>
        /// Checks if given password is correct
        /// </summary>
        public bool VerifyPassword(string password) => this.HashPassword(password) == this.passwordHash;
        /// <summary>
        /// Hashes a password
        /// </summary>
        private string HashPassword(string password)
        {
            if(password != "")
            {
                // Use input string to calculate MD5 hash
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var inputBytes = Encoding.ASCII.GetBytes(password);
                    var hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    var sb = new StringBuilder();
                    foreach (var t in hashBytes)
                    {
                        sb.Append(t.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
            else
            {
                // return empty string if given password is empty
                return "";
            }
        }
    }
}