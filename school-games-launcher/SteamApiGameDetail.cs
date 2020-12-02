using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class SteamApiGameDetail
    {
        public string Name { get; set; }
        public int SteamAppid { get; set; }
        public int RequiredAge { get; set; }
        public string HeaderImage { get; set; }
        public string ShortDescription { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
    }
}
