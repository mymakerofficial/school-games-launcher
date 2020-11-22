using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class PlayPeriod
    {
        private Game game;
        private int startTime;
        private int endTime;
        private int weekDay;
        public PlayPeriod(Game game, int startTime, int endTime, int weekDay)
        {
            this.game = game;
            this.startTime = startTime;
            this.endTime = endTime;
            this.weekDay = weekDay;
        }
        public bool IsActive
        {
            get { return true; }
        }
    }
}
