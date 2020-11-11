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

        public string Name
        {
            get
            {
                return name;
            }
        }

        public void Main(string name, int birthTimestamp, string passwordHash, bool admin)
        {
            this.name = name;
            this.birthTimestamp = birthTimestamp;
            this.passwordHash = passwordHash;
            this.admin = admin;

        }
        public static bool AllowedToPlay(Game game)
        {
            bool allowed = true;



            return allowed;
        }
    }
}
