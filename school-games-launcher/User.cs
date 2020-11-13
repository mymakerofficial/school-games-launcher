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
        private int birthTimestamp;
        private string passwordHash;
        private bool admin;
        private List<PlayPeriod> playPeriods = new List<PlayPeriod>();
        private List<GameExeption> gameExeptions = new List<GameExeption>();

        public string Name => name;
        public int BirthTimestamp => birthTimestamp;
        public bool Admin => admin;
        public List<PlayPeriod> PlayPeriods => playPeriods;
        public List<GameExeption> GameExeptions => gameExeptions;

        public User(string name, int birthTimestamp, string passwordHash, bool admin)
        {
            this.name = name;
            this.birthTimestamp = birthTimestamp;
            this.passwordHash = passwordHash;
            this.admin = admin;

        }
        private void LoadData()
        {

        }
        /// <summary>
        /// Checks if this user is allowed to play the given game.
        /// </summary>
        public bool AllowedToPlay(Game game)
        {
            bool allowed = true;



            return allowed;
        }
        /// <summary>
        /// Checks if given password is correct
        /// </summary>
        public bool VerifyPassword(string password)
        {
            bool valid = true;

            return valid;
        }
    }
}