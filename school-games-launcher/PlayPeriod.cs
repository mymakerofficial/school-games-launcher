using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class PlayPeriod
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int WeekDay { get; set; }

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
            this.StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(startTime);
            this.EndTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(endTime);
            this.WeekDay = weekDay;
        }
        public bool IsActive
        {
            get {// checks if current time is during weekDay and between startTime and endTime
                return (int)DateTime.Now.DayOfWeek == this.WeekDay && new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().Add(DateTime.Now.TimeOfDay) >= this.StartTime && new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().Add(DateTime.Now.TimeOfDay) <= this.EndTime; 
            }
        }
    }
}
