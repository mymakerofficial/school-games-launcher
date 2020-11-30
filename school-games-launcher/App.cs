using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_games_launcher
{
    public class App
    {
        private static List<User> users = new List<User>();
        private static List<Game> games = new List<Game>();
        private static User activeUser;
        private static List<Session> sessions = new List<Session>();
        private static Session activeSession;
        private static GUI gui;
        private string configPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/MyMakerGameLauncher/";
        /// <summary>
        /// List of all existing users
        /// </summary>
        public List<User> Users { get { return users; } }
        /// <summary>
        /// List of all existing games
        /// </summary>
        public List<Game> Games { get { return games; } }
        /// <summary>
        /// Currently logged in user
        /// </summary>
        public User ActiveUser { get { return activeUser; } }
        /// <summary>
        /// All gaming sessions ever
        /// </summary>
        public List<Session> Sessions { get { return sessions; } }
        /// <summary>
        /// Currently active gaming session
        /// </summary>
        public Session ActiveSession { get { return activeSession; } }
        /// <summary>
        /// The GUI Object of the app.
        /// </summary>
        public GUI Gui { get { return gui; } }
        /// <summary>
        /// List of games the active user is allowed to play
        /// </summary>
        public List<Game> VisibleGames {
            get {
                List<Game> games = new List<Game>();
                if(this.ActiveUser != null)
                {
                    foreach (Game game in this.Games)
                    {
                        if (this.ActiveUser.AllowedToPlay(game)) games.Add(game);
                    }
                }
                return games;
            }
        }

        public App()
        {
            gui = new GUI();
        }
        public void Run()
        {
            this.Gui.Run();
            this.Gui.form.Load += new System.EventHandler(this.Load);
        }
        private void Load(object sender, System.EventArgs e)
        {
            if (this.CheckData())
            {
                this.LoadData();
                this.CheckUser();
            }
        }
        private bool CheckData()
        {
            return true;
        }
        private void LoadData()
        {
            // loads users
            Loader loadedUsers = new Loader(this.configPath + "users.csv");
            foreach(List<string> userData in loadedUsers.Data)
            {
                User user = new User(Int32.Parse(userData[0]), userData[1], Int32.Parse(userData[2]), userData[3], Convert.ToBoolean(userData[4]));
                this.Users.Add(user);// puts user in list
            }
            // loads games
            Loader loadedGames = new Loader(this.configPath + "games.csv");
            foreach (List<string> gameData in loadedGames.Data)
            {
                Game game = new Game(Int32.Parse(gameData[0]), gameData[1], gameData[2], Int32.Parse(gameData[3]), gameData[5]);
                this.Games.Add(game);// puts game in list
            }
            // loads allowed play times
            Loader loadedTimes = new Loader(this.configPath + "allowed_playtime.csv");
            foreach (List<string> timeData in loadedTimes.Data)
            {
                PlayPeriod period = new PlayPeriod(Int32.Parse(timeData[1]), Int32.Parse(timeData[2]), Int32.Parse(timeData[3]));
                this.GetUserById(Int32.Parse(timeData[0])).PlayPeriods.Add(period);// adds play period to correct player
            }
            // loads game exeptions
            Loader loadedExeptions = new Loader(this.configPath + "play_exeptions.csv");
            foreach (List<string> exeptionData in loadedExeptions.Data)
            {
                GameExeption exeption = new GameExeption(this.GetGameById(Int32.Parse(exeptionData[1])), Convert.ToBoolean(exeptionData[2]));
                this.GetUserById(Int32.Parse(exeptionData[0])).GameExeptions.Add(exeption);// adds game exeption to correct player
            }

            //var hash = this.ActiveUser.HashPassword("admin");
        }
        /// <summary>
        /// Launches given game as active user.
        /// </summary>
        public void Launch(Game game)
        {
            if(game != null)
            {
                if(this.CheckUser())
                {
                    activeSession = game.Launch(this.ActiveUser);
                    this.Sessions.Add(this.ActiveSession);
                    this.Gui.playing.Update();
                }
            }
        }
        public void Launch(string game) => this.Launch(this.GetGameByName(game));
        /// <summary>
        /// Sets active user.
        /// </summary>
        public bool LoginUser(User user, string password)
        {
            bool valid = false;
            if (user != null)
            {
                valid = user.VerifyPassword(password);

                if (valid)
                {
                    activeUser = user;
                }
            }

            return valid;
        }
        /// <summary>
        /// Sets active user.
        /// </summary>
        public bool LoginUser(string username, string password) => this.LoginUser(this.GetUserByName(username), password);
        /// <summary>
        /// Logsout the active user
        /// </summary>
        public void Logout()
        {
            activeUser = null;
            this.CheckUser();
        }
        public bool CheckUser()
        {
            if(this.ActiveUser == null)
            {
                this.Gui.login.Activate();
            }
            return this.ActiveUser != null;
        }
        /// <summary>
        /// Gets a user by name... what did you expect?
        /// </summary>
        public User GetUserByName(string username)
        {
            return users.Find(x => x.Name.ToLower() == username.ToLower());
        }
        /// <summary>
        /// Gets a game by name.
        /// </summary>
        public Game GetGameByName(string name)
        {
            return games.Find(x => x.Name.ToLower() == name.ToLower());
        }
        /// <summary>
        /// Gets a user by id
        /// </summary>
        public User GetUserById(int id)
        {
            return users.Find(x => x.Id == id);
        }
        /// <summary>
        /// Gets a game by id
        /// </summary>
        public Game GetGameById(int id)
        {
            return games.Find(x => x.Id == id);
        }
    }
}
