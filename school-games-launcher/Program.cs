﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace school_games_launcher
{

    public static class Program
    {

        public static App app;

        /// <summary>
        /// The main entrace point of this program. This is where shid beginns.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            app = new App();

            app.Run();
        }
    }
}
