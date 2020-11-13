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
        public List<User> Users => users;
        public List<Game> Games => games;
        public User ActiveUser => activeUser;

        public App()
        {

        }
        private void LoadData()
        {

        }
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
        public bool LoginUser(string username, string password)
        {
            return this.LoginUser(GetUserByName(username), password);
        }
        /// <summary>
        /// Gets a user by name... duh....
        /// </summary>
        public User GetUserByName(string username)
        {
            //boi this is short
            return users.Find(x => x.Name.ToLower() == username.ToLower()); ;
        }
    }
}
