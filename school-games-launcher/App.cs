using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private static readonly HttpClient HttpClient = new HttpClient();
        public List<SteamApiGame> SteamApiGames { get; set; }
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
            else
            {
                this.CheckUser();
            }

            this.LoadSteamGameList();
        }
        private async void LoadSteamGameList()
        {
            try
            {
                var response = await HttpClient.GetAsync($"http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonString);
                    SteamApiGames = json["applist"]["apps"].ToObject<List<SteamApiGame>>();
                }
            }
            catch
            {
                SteamApiGames = new List<SteamApiGame>();
            }
        }
        private bool CheckData()
        {
            if (!Directory.Exists(this.configPath)) Directory.CreateDirectory(this.configPath);

            if (File.Exists(this.configPath + "users.csv") &&
                File.Exists(this.configPath + "games.csv") &&
                File.Exists(this.configPath + "allowed_playtime.csv") &&
                File.Exists(this.configPath + "play_exeptions.csv") &&
                File.Exists(this.configPath + "sessions.csv")) return true;

            this.Users.Add(new User(0, "admin", 1, "", true, Convert.ToString(new Random().Next(0, 5))));
            this.Games.Add(new Game(1, "Counting", "https://counting.cf", 0, "https://counting.essiebes551.tk/733fcb3147fa883f2393c5efe66c4195.jpg"));
            //TODO Maker add more sample data......

            return false;
        }
        public void LoadData()
        {
            // loads users
            Loader loadedUsers = new Loader(this.configPath + "users.csv");
            foreach(List<string> userData in loadedUsers.Data)
            {
                User user = new User(Int32.Parse(userData[0]), userData[1], Int32.Parse(userData[2]), userData[3], Convert.ToBoolean(userData[4]), userData[5]);
                this.Users.Add(user);// puts user in list
            }
            // loads games
            Loader loadedGames = new Loader(this.configPath + "games.csv");
            foreach (List<string> gameData in loadedGames.Data)
            {
                Game game = new Game(Int32.Parse(gameData[0]), gameData[1], gameData[2], Int32.Parse(gameData[3]), gameData[5]);
                if(gameData[4] != "") game.SteamId = Int32.Parse(gameData[4]);
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
            // loads sessions
            Loader loadedSessions = new Loader(this.configPath + "sessions.csv");
            foreach (List<string> sessionData in loadedSessions.Data)
            {
                Session session = new Session(this.GetGameById(Int32.Parse(sessionData[0])), this.GetUserById(Int32.Parse(sessionData[1])));
                session.Set(Int32.Parse(sessionData[2]), Int32.Parse(sessionData[3]));
                this.Sessions.Add(session);
            }

            // var hash = this.ActiveUser.HashPassword("admin");
            this.SaveData();
        }
        public void SaveData()
        {
            // save users
            var usersCsv = new StringBuilder();
            foreach(User user in this.Users)
            {
                var newLine = string.Format("{0},{1},{2},{3},{4},{5}", Convert.ToString(user.Id), user.Name, Convert.ToString(user.BirthTimestamp), user.PasswordHash, Convert.ToString(user.Admin), user.Avatar);
                usersCsv.AppendLine(newLine);
            }
            File.WriteAllText(this.configPath + "users.csv", usersCsv.ToString());
            // save games
            var gamesCsv = new StringBuilder();
            foreach (Game game in this.Games)
            {
                var newLine = string.Format("{0},{1},{2},{3},{4},{5}", Convert.ToString(game.Id), game.Name, game.Executable.Path, Convert.ToString(game.Age), game.SteamId, game.Coverart);
                gamesCsv.AppendLine(newLine);
            }
            File.WriteAllText(this.configPath + "games.csv", gamesCsv.ToString());
            // save PlayPeriods
            var timesCsv = new StringBuilder();
            foreach (User user in this.Users)
            {
                foreach (PlayPeriod period in user.PlayPeriods)
                {
                    var newLine = string.Format("{0},{1},{2},{3}", Convert.ToString(user.Id), Convert.ToString(period.WeekDay), Convert.ToString(period.StartTimestamp), Convert.ToString(period.EndTimestamp));
                    timesCsv.AppendLine(newLine);
                }
            }
            File.WriteAllText(this.configPath + "allowed_playtime.csv", timesCsv.ToString());
            // save GameExeptions
            var exeptionsCsv = new StringBuilder();
            foreach (User user in this.Users)
            {
                foreach (GameExeption exeption in user.GameExeptions)
                {
                    var newLine = string.Format("{0},{1},{2}", Convert.ToString(user.Id), Convert.ToString(exeption.Game.Id), Convert.ToString(exeption.Allowed));
                    exeptionsCsv.AppendLine(newLine);
                }
            }
            File.WriteAllText(this.configPath + "play_exeptions.csv", exeptionsCsv.ToString());
            // save Sessions
            var sessionsCsv = new StringBuilder();
            foreach (Session session in this.Sessions)
            {
                var newLine = string.Format("{0},{1},{2},{3}", Convert.ToString(session.Game.Id), Convert.ToString(session.User.Id), Convert.ToString(session.StartTimestamp), Convert.ToString(session.EndTimestamp));
                sessionsCsv.AppendLine(newLine);
            }
            File.WriteAllText(this.configPath + "sessions.csv", sessionsCsv.ToString());
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
        /// <summary>
        /// Checks if a user is logged in. If not it showes the login page.
        /// </summary>
        /// <returns>Returns true if a user is logged in</returns>
        public bool CheckUser()
        {
            if(this.ActiveUser == null)
            {
                this.Gui.login.Activate();
            }
            return this.ActiveUser != null;
        }
        /// <summary>
        /// Creates a new User and adds it to User list.
        /// </summary>
        public bool CreateUser(string name, DateTime birthDate, string password)
        {
            if (!this.CheckUser() || !this.ActiveUser.Admin) return false;
            User user = new User(this.Users.Count, name, (int)new DateTimeOffset(birthDate).ToUnixTimeSeconds(), "", false, Convert.ToString(new Random().Next(0, 5)));
            user.SetPassword("", password);// set user password
            for(int i = 0; i <= 6; i++)// create default PlayPeriods
            {
                user.PlayPeriods.Add(new PlayPeriod(i, 0, 86400));
            }
            this.Users.Add(user);
            return true;
        }
        /// <summary>
        /// Creates a new Game and adds it to game list.
        /// </summary>
        public bool CreateGame(string name, string path, int age, string coverart, int? steamId = null)
        {
            if (!this.CheckUser() || !this.ActiveUser.Admin) return false;
            Game game = new Game(this.Games.Count, name, path, age, coverart);
            game.SteamId = steamId;
            this.Games.Add(game);
            return true;
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
        public SteamApiGame FindSteamGameByName(string name)
        {
            SteamApiGame game = SteamApiGames.Find(x => x.Name.ToLower() == name.ToLower());
            if(game != null) { return game;  }
            return null;
        }
        public SteamApiGame FindSteamGameById(int id)
        {
            SteamApiGame game = SteamApiGames.Find(x => x.AppId == id);
            if (game != null) { return game; }
            return null;
        }
        public async Task<SteamApiGameDetail> LoadSteamGameDetails(int id)
        {
            try
            {
                var response = await HttpClient.GetAsync($" http://store.steampowered.com/api/appdetails?appids=" + id);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonString);

                    List<string> screenshotList = new List<string>();
                    for (int i = 0; i < json[Convert.ToString(id)]["data"]["screenshots"].Value<dynamic>().Count; i++)
                    {
                        screenshotList.Add(json[Convert.ToString(id)]["data"]["screenshots"][i].Value<dynamic>()["path_thumbnail"].Value);
                    }
                    var game = new SteamApiGameDetail()
                    {
                        Name = json[Convert.ToString(id)]["data"]["name"].Value<string>(),
                        SteamAppid = json[Convert.ToString(id)]["data"]["steam_appid"].Value<int>(),
                        RequiredAge = json[Convert.ToString(id)]["data"]["required_age"].Value<int>(),
                        HeaderImage = json[Convert.ToString(id)]["data"]["header_image"].Value<string>(),
                        ShortDescription = json[Convert.ToString(id)]["data"]["short_description"].Value<string>(),
                        Developer = json[Convert.ToString(id)]["data"]["developers"][0].Value<string>(),
                        Publisher = json[Convert.ToString(id)]["data"]["publishers"][0].Value<string>(),
                        Screenshots = screenshotList
                    };
                    return game;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Searches for games.
        /// </summary>
        /// <param name="rawQuery">the search request</param>
        /// <param name="searchAll">if all games should be searched or just the games visible to the current user</param>
        /// <returns></returns>
        public List<Game> SearchGames(string rawQuery, bool searchAll = false)
        {
            List<Game> gameList = searchAll ? this.Games : this.VisibleGames;
            List<Game> filterList = new List<Game>();

            string[] queries = rawQuery.ToLower().Split(' ');

            foreach(Game game in gameList) {
                int queryMatches = 0;
                foreach(string query in queries) {
                    if (game.Name.ToLower().Contains(query)) queryMatches++;
                }

                if (queryMatches == queries.Length) filterList.Add(game);
            }
            
            return filterList;
        }
        public void Exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
