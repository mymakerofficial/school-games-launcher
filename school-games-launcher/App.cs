using System;
using System.Collections.Generic;
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
        public List<User> Users => users;
        public List<Game> Games => games;
        public User ActiveUser => activeUser;
        public List<Session> Sessions => sessions;
        public Session ActiveSession => activeSession;

        public App()
        {
            this.LoadData();
        }
        private void CheckData()
        {

        }
        private void LoadData()
        {
            Loader loadedUsers = new Loader("C:/Users/mohem/AppData/Roaming/MyMakerGameLauncher/users.csv");
            foreach(List<string> userData in loadedUsers.Data)
            {
                User user = new User(userData[0], Int32.Parse(userData[1]), userData[2], Convert.ToBoolean(userData[3]));
                this.Users.Add(user);
            }
            Loader loadedGames = new Loader("C:/Users/mohem/AppData/Roaming/MyMakerGameLauncher/games.csv");
            foreach (List<string> gameData in loadedGames.Data)
            {
                Game game = new Game(gameData[0], gameData[1], Int32.Parse(gameData[2]));
                this.Games.Add(game);
            }

            this.LoginUser("admin", "");

            this.Launch("Witch It");
        }
        public void Launch(Game game)
        {
            if(game != null)
            {
                if(this.ActiveUser != null)
                {
                    activeSession = game.Launch(this.ActiveUser);
                }
            }
        }
        public void Launch(string game) => this.Launch(this.GetGameByName(game));
        /// <summary>
        /// Sets active user.
        /// </summary>
        public bool LoginUser(User user, string password)
        {
            bool valid = user.VerifyPassword(password);

            if (valid)
            {
                activeUser = user;
            }

            return valid;
        }
        /// <summary>
        /// Sets active user.
        /// </summary>
        public bool LoginUser(string username, string password) => this.LoginUser(this.GetUserByName(username), password);
        /// <summary>
        /// Gets a user by name... duh....
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
    }
}
