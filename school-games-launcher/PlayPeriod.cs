using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class PlayPeriod
    {
        public Game game;
        public int startTime;
        public int endTime;
        public int weekDay;
        public void Main(Game game, int startTime, int endTime, int weekDay)
        {

        }
        public bool IsActive(Game game)
        {

            return true;
        }
    }
}
