using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class PlayPeriod
    {
        private DateTime startTime;
        private DateTime endTime;
        private int weekDay;
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        public int WeekDay { get { return weekDay; } }
        public int StartTimestamp
        {
            get
            {
                return (int)new DateTimeOffset(this.StartTime).ToUnixTimeSeconds();
            }
        }
        public int EndTimestamp
        {
            get
            {
                return (int)new DateTimeOffset(this.EndTime).ToUnixTimeSeconds();
            }
        }
        public PlayPeriod(int weekDay, int startTime, int endTime)
        {
            this.startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(startTime);
            this.endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(endTime);
            this.weekDay = weekDay;
        }
        public bool IsActive
        {
            get {// checks if current time is during weekDay and between startTime and endTime
                return (int)DateTime.Now.DayOfWeek == this.weekDay && new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().Add(DateTime.Now.TimeOfDay) >= this.startTime && new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().Add(DateTime.Now.TimeOfDay) <= this.endTime; 
            }
        }
    }
}
