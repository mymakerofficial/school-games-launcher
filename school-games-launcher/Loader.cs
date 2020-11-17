using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace school_games_launcher
{
    public class Loader
    {
        private string path;
        private StreamReader sr;
        private List<List<string>> data = new List<List<string>>();
        public List<List<string>> Data => data;
        public Loader(string path)
        {
            this.path = path;

            // TODO create exeption handeling

            // load file
            this.sr = File.OpenText(this.path);
            string lineString;
            while (!this.sr.EndOfStream)
            {
                lineString = this.sr.ReadLine();
                List<string> line = new List<string>();
                foreach(string cell in lineString.Split(',')) line.Add(cell);
                this.data.Add(line);
            }
            this.sr.Close();
        }
    }
}
