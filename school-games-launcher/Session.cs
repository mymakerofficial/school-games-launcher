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
        private DateTime startTime;
        private DateTime endTime;
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        public Game Game { get { return game; } }
        public User User { get { return user; } }
        /// <summary>
        /// The the start time of the session as a unixtimestamp.
        /// </summary>
        public int StartTimestamp
        {
            get
            {
                return (int)new DateTimeOffset(this.StartTime).ToUnixTimeSeconds();
            }
        }
        /// <summary>
        /// The the end time of the session as a unixtimestamp.
        /// </summary>
        public int EndTimestamp
        {
            get
            {
                return (int)new DateTimeOffset(this.EndTime).ToUnixTimeSeconds();
            }
        }
        public TimeSpan Duration { get { return this.EndTime - this.StartTime; } }
        public Session(Game game, User user)
        {
            this.game = game;
            this.user = user;
        }
        public void Start()
        {
            this.startTime = DateTime.Now;
            Program.app.Gui.playing.Activate();
        }
        public void End()
        {
            this.endTime = DateTime.Now;
            Program.app.Gui.library.Activate();
        }
        public void Set(int startTimestamp, int endTimestamp)
        {
            this.startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(startTimestamp);
            this.endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(endTimestamp);
        }
    }
}
