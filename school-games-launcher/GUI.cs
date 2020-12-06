using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Globalization;

namespace school_games_launcher
{
    public class GUI
    {
        public GUILibrary library;
        public GUIOptions options;
        public GUIAddGame addGame;
        public GUIEditGame editGame;
        public GUIProfile profile;
        public GUITab login;
        public GUIRegister register;
        public GUIPlaying playing;
        public GUIGameDetails gameDetails;
        public GUIEditUser editUser;
        public GUIChangePassword changePassword;
        public GUITab userRules;
        public GUIWelcome welcome;

        public MainWindow form;
        public TabControl tabControl;
        public GUI()
        {
            form = new MainWindow();
        }
        public void Run()
        {
            new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(true);
                Application.Run(form);
            }).Start();
            form.Load += new System.EventHandler(this.Setup);
        }
        public void Setup(object sender, System.EventArgs e)
        {
            this.tabControl = (TabControl)this.form.Controls["TabControl"];
            library = new GUILibrary(this.tabControl, (TabPage)this.tabControl.TabPages["tabLibrary"]);
            library.updateOnActive = true;
            options = new GUIOptions(this.tabControl, (TabPage)this.tabControl.TabPages["tabOptions"]);
            options.updateOnActive = true;
            addGame = new GUIAddGame(this.tabControl, (TabPage)this.tabControl.TabPages["tabAddGame"]);
            addGame.resetOnActive = true;
            editGame = new GUIEditGame(this.tabControl, (TabPage)this.tabControl.TabPages["tabEditGame"]);
            profile = new GUIProfile(this.tabControl, (TabPage)this.tabControl.TabPages["tabProfile"]);
            profile.updateOnActive = true;
            login = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabLogin"]);
            login.resetOnActive = true;
            register = new GUIRegister(this.tabControl, (TabPage)this.tabControl.TabPages["tabRegister"]);
            register.updateOnActive = true;
            playing = new GUIPlaying(this.tabControl, (TabPage)this.tabControl.TabPages["tabPlaying"]);
            gameDetails = new GUIGameDetails(this.tabControl, (TabPage)this.tabControl.TabPages["tapGameDetails"]);
            gameDetails.updateOnActive = true;
            gameDetails.Setup();
            editUser = new GUIEditUser(this.tabControl, (TabPage)this.tabControl.TabPages["tabEditUser"]);
            editUser.updateOnActive = true;
            changePassword = new GUIChangePassword(this.tabControl, (TabPage)this.tabControl.TabPages["tabChangePassword"]);
            changePassword.resetOnActive = true;

            welcome = new GUIWelcome(this.tabControl, (TabPage)this.tabControl.TabPages["tabWelcome"]);
            welcome.updateOnActive = true;

            this.tabControl.Appearance = TabAppearance.FlatButtons;
            this.tabControl.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl.SizeMode = TabSizeMode.Fixed;
        }
        public void Minimize()
        {
            form.WindowState = FormWindowState.Minimized;
        }
        public void SetAvatar(PictureBox pictureBox, string avatar)
        {
            switch (avatar)
            {
                case "0":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_0;
                    break;
                case "1":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_1;
                    break;
                case "2":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_2;
                    break;
                case "3":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_3;
                    break;
                case "4":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_4;
                    break;
                case "5":
                    pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_5;
                    break;
                default:
                    try
                    {
                        pictureBox.Load(avatar);
                    }
                    catch
                    {
                        pictureBox.Image = global::school_games_launcher.Properties.Resources.avatar_placeholder;
                    }
                    break;
            }
        }
    }
    public class GUITab
    {
        private TabPage tabPage;
        private TabControl tabControl;
        public TabPage TabPage { get { return tabPage; } }
        public TabControl TabControl { get { return tabControl; } }
        public bool updateOnActive = false;
        public bool resetOnActive = false;

        public GUITab(TabControl tabControl, TabPage tabPage)
        {
            this.tabControl = tabControl;
            this.tabPage = tabPage;
        }
        public void Activate()
        {
            this.tabControl.SelectedTab = this.tabPage;
            if (this.updateOnActive) this.Update();
            if (this.resetOnActive) this.Reset();
        }
        public virtual void Setup()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void Reset()
        {
        }
    }
    public class GUILibrary : GUITab
    {
        public string search = "";
        public List<Panel> panels = new List<Panel>();
        public List<Game> games;
        public GUILibrary(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage) 
        {

        }

        public override void Update()
        {
            if (Program.app.ActiveUser != null)
            {
                this.TabPage.Controls["lblLibraryNotAllowedToPlay"].Visible = !Program.app.ActiveUser.InPlayPeriod;
                this.TabPage.Controls["lblLibrarySearchGame"].Visible = this.TabPage.Controls["tbxLibrarySearchGame"].Visible = Program.app.VisibleGames.Count > 0;
                this.UpdateList(Program.app.VisibleGames);
            }  
        }
        public void SearchGames(string query)
        {
            List<Game> list = Program.app.SearchGames(query);

            this.UpdateList(list);
        }
        public void UpdateList(List<Game> games)
        {
            this.games = games;

            // fixes memory leak ishue
            foreach (Control control in panels)
            {
                control.Dispose();
            }
            panels.Clear();


            int margin = 12;
            foreach (Game game in this.games)
            {
                // creates a panel
                Panel panel = new System.Windows.Forms.Panel();
                panel.Name = "pnlLibraryGame_" + game.Name;
                panel.Size = new System.Drawing.Size(306 + (margin * 2), 143 + (margin * 2));

                // create PictureBox for coverart
                PictureBox coverart = new System.Windows.Forms.PictureBox();
                coverart.Name = "pbxLibraryGame_" + game.Name;
                coverart.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                coverart.Location = new System.Drawing.Point(margin, margin);
                coverart.Size = new System.Drawing.Size(306, 143);
                coverart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                coverart.BackColor = System.Drawing.Color.Black;

                // create function to go to game page
                void LaunchGame(object sender, EventArgs e)
                {
                    Program.app.Gui.gameDetails.Activate();
                    Program.app.Gui.gameDetails.SetGame(game);
                }
                // add function as EventHandler
                panel.Click += new System.EventHandler(LaunchGame);
                coverart.Click += new System.EventHandler(LaunchGame);

                // add the PictureBox to the Panel
                panel.Controls.Add(coverart);

                // add Panel to array
                panels.Add(panel);
            }

            // add the panels to the Form
            this.TabPage.Controls["flpLibraryGameList"].Controls.Clear();
            foreach (Control control in panels)
            {
                this.TabPage.Controls["flpLibraryGameList"].Controls.Add(control);
            }

            // Load images in different thread.
            // This makes the loading of the library way smoother.
            Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                for(int i = 0; i < panels.Count(); i++)
                {
                    try
                    {
                        PictureBox coverart = (PictureBox)panels[i].Controls[0];
                        coverart.Load(this.games[i].Coverart);
                    }
                    catch { }
                }
            });
        }
    }
    public class GUIOptions : GUITab
    {
        public List<GroupBox> gamePanels = new List<GroupBox>();
        public List<GroupBox> userPanels = new List<GroupBox>();
        public List<Game> games;
        public List<User> users;
        public GUIOptions(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            this.UpdateGameList(Program.app.Games);
            this.UpdateUserList(Program.app.Users);
            if(Program.app.ActiveUser != null)
                ((Button)this.TabPage.Controls["btnOptionsAddGame"]).Visible =
                    ((Button)this.TabPage.Controls["btnOptionsAddUser"]).Visible = Program.app.ActiveUser.Admin;
        }

        public void UpdateGameList(List<Game> games)
        {
            this.games = games;

            // fixes memory leak ishue
            foreach (Control control in gamePanels)
            {
                control.Dispose();
            }
            gamePanels.Clear();


            foreach (Game game in this.games)
            {
                // creates a panel
                GroupBox panel = new System.Windows.Forms.GroupBox();
                panel.Name = "gbxOptionsGame_" + game.Name;
                panel.Size = new System.Drawing.Size(360, 80);

                // create PictureBox for coverart
                PictureBox coverart = new System.Windows.Forms.PictureBox();
                coverart.Name = "pbxOptionsGame_" + game.Name;
                coverart.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                coverart.Location = new System.Drawing.Point(15, 15);
                coverart.Size = new System.Drawing.Size(100, 50);
                coverart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                coverart.BackColor = System.Drawing.Color.Black;

                Label name = new System.Windows.Forms.Label();
                name.Text = game.Name;
                name.Size = new System.Drawing.Size(100, 15);
                name.Location = new System.Drawing.Point(130, 25);

                Label id = new System.Windows.Forms.Label();
                id.Text = String.Format("ID: {0}", game.Id);
                id.ForeColor = System.Drawing.Color.Silver;
                id.Location = new System.Drawing.Point(130, 45);

                // create function to go to game page
                void LaunchGame(object sender, EventArgs e)
                {
                    Program.app.Gui.gameDetails.SetGame(game);
                    Program.app.Gui.gameDetails.Activate();
                    //Program.app.Launch(game);
                }
                // add function as EventHandler
                panel.Click += new System.EventHandler(LaunchGame);
                coverart.Click += new System.EventHandler(LaunchGame);
                name.Click += new System.EventHandler(LaunchGame);
                id.Click += new System.EventHandler(LaunchGame);

                // add the elements to the Panel
                panel.Controls.Add(coverart);
                panel.Controls.Add(name);
                panel.Controls.Add(id);

                // add Panel to array
                gamePanels.Add(panel);
            }

            // add the panels to the Form
            this.TabPage.Controls["flpOptionsGames"].Controls.Clear();
            foreach (Control control in gamePanels)
            {
                this.TabPage.Controls["flpOptionsGames"].Controls.Add(control);
            }

            // Load images in different thread.
            // This makes the loading of the library way smoother.
            Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                for (int i = 0; i < gamePanels.Count(); i++)
                {
                    try
                    {
                        PictureBox coverart = (PictureBox)gamePanels[i].Controls[0];
                        coverart.Load(this.games[i].Coverart);
                    }
                    catch { }
                }
            });
        }
        public void UpdateUserList(List<User> users)
        {
            this.users = users;

            // fixes memory leak ishue
            foreach (Control control in userPanels)
            {
                control.Dispose();
            }
            userPanels.Clear();


            foreach (User user in this.users)
            {
                // creates a panel
                GroupBox panel = new System.Windows.Forms.GroupBox();
                panel.Name = "gbxOptionsGame_" + user.Name;
                panel.Size = new System.Drawing.Size(360, 80);

                // create PictureBox for coverart
                PictureBox avatar = new System.Windows.Forms.PictureBox();
                Program.app.Gui.SetAvatar(avatar, user.Avatar);
                avatar.Location = new System.Drawing.Point(15, 15);
                avatar.Size = new System.Drawing.Size(50, 50);
                avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                avatar.BackColor = System.Drawing.Color.Black;

                Label name = new System.Windows.Forms.Label();
                name.Text = user.Name;
                name.Size = new System.Drawing.Size(100, 15);
                name.Location = new System.Drawing.Point(80, 25);

                Label id = new System.Windows.Forms.Label();
                id.Text = String.Format("ID: {0}", user.Id);
                id.ForeColor = System.Drawing.Color.Silver;
                id.Location = new System.Drawing.Point(80, 45);

                // create function to go to open profile
                void OpenProfile(object sender, EventArgs e)
                {
                    Program.app.Gui.profile.Activate();
                    Program.app.Gui.profile.User = user;
                }
                // add function as EventHandler
                panel.Click += new System.EventHandler(OpenProfile);
                avatar.Click += new System.EventHandler(OpenProfile);
                name.Click += new System.EventHandler(OpenProfile);
                id.Click += new System.EventHandler(OpenProfile);

                // add the elements to the Panel
                panel.Controls.Add(avatar);
                panel.Controls.Add(name);
                panel.Controls.Add(id);

                // add Panel to array
                userPanels.Add(panel);
            }

            // add the panels to the Form
            this.TabPage.Controls["flpOptionsUsers"].Controls.Clear();
            foreach (Control control in userPanels)
            {
                this.TabPage.Controls["flpOptionsUsers"].Controls.Add(control);
            }
        }
    }
    public class GUIPlaying : GUITab
    {
        public GUIPlaying(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            if(Program.app.ActiveSession != null)
            {
                PictureBox coverartBox = (PictureBox)this.TabPage.Controls["pbxPlayingCoverart"];

                try
                {
                    coverartBox.Load(Program.app.ActiveSession.Game.Coverart);
                }
                catch
                {
                    coverartBox.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                }
            }
        }
    }
    public class GUIProfile : GUITab
    {
        private User user;
        private List<Session> sessions;
        private List<Control> activityPanels = new List<Control>();
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                ((Label)this.TabPage.Controls["lblProfileName"]).Text = user.Name;
                ((Label)this.TabPage.Controls["lblProfileBirth"]).Text = String.Format("Age: {0} ({1})", user.Age, user.BirthDate.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")));
                ((Label)this.TabPage.Controls["lblProfileId"]).Text = String.Format("ID: {0}", user.Id);
                ((Label)this.TabPage.Controls["lblProfileTimePlayed"]).Text = String.Format("Time played: {0}{1}", Math.Floor(Program.app.GetUserTimePlayed(user).TotalHours) > 0 ? Math.Floor(Program.app.GetUserTimePlayed(user).TotalHours) : Math.Floor(Program.app.GetUserTimePlayed(user).TotalMinutes), Math.Floor(Program.app.GetUserTimePlayed(user).TotalHours) > 0 ? "h" : "m");

                if (Program.app.ActiveUser != null)
                    ((Button)this.TabPage.Controls["btnProfileEdit"]).Visible = Program.app.ActiveUser.Admin || Program.app.ActiveUser == user;

                Program.app.Gui.SetAvatar(((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]), user.Avatar);
                this.UpdateActivityList(user);
            }
        }
        public GUIProfile(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            this.User = Program.app.ActiveUser;
        }

        public void UpdateActivityList(User user)
        {
            this.sessions = Program.app.GetUserSessions(user);
            this.sessions.Reverse();

            // fixes memory leak ishue
            foreach (Control control in activityPanels)
            {
                control.Dispose();
            }
            activityPanels.Clear();


            for (int i = 0; i < this.sessions.Count && i < 7; i++)
            {
                Session session = this.sessions[i];

                // creates a panel
                Panel panel = new System.Windows.Forms.Panel();
                panel.Size = new System.Drawing.Size(1000, 137);

                // create PictureBox for coverart
                PictureBox coverart = new System.Windows.Forms.PictureBox();
                coverart.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                coverart.Location = new System.Drawing.Point(15, 15);
                coverart.Size = new System.Drawing.Size(230, 107);
                coverart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                coverart.BackColor = System.Drawing.Color.Black;

                Label name = new System.Windows.Forms.Label();
                name.Text = session.Game.Name;
                name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                name.Size = new System.Drawing.Size(500, 30);
                name.Location = new System.Drawing.Point(270, 25);

                Label info = new System.Windows.Forms.Label();
                info.Text = String.Format("Date / Time: {0}       Duration: {1:hh}h {1:mm}m", session.StartTime.ToString("g", CultureInfo.CreateSpecificCulture("de-DE")), session.Duration);
                info.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                info.ForeColor = System.Drawing.Color.LightGray;
                info.Size = new System.Drawing.Size(500, 30);
                info.Location = new System.Drawing.Point(270, 60);


                // create function to go to game page
                void LaunchGame(object sender, EventArgs e)
                {
                    Program.app.Gui.gameDetails.SetGame(session.Game);
                    Program.app.Gui.gameDetails.Activate();
                    //Program.app.Launch(game);
                }
                // add function as EventHandler
                coverart.Click += new System.EventHandler(LaunchGame);

                // add the elements to the Panel
                panel.Controls.Add(coverart);
                panel.Controls.Add(name);
                panel.Controls.Add(info);

                // add Panel to array
                activityPanels.Add(panel);
            }

            // add the panels to the Form
            this.TabPage.Controls["flpProfileLastPlayed"].Controls.Clear();
            foreach (Control control in activityPanels)
            {
                this.TabPage.Controls["flpProfileLastPlayed"].Controls.Add(control);
            }

            // Load images in different thread.
            // This makes the loading of the library way smoother.
            Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                for (int i = 0; i < activityPanels.Count(); i++)
                {
                    try
                    {
                        PictureBox coverart = (PictureBox)activityPanels[i].Controls[0];
                        coverart.Load(this.sessions[i].Game.Coverart);
                    }
                    catch { }
                }
            });
        }
       
        public void Edit()
        {
            Program.app.Gui.editUser.Activate();
            Program.app.Gui.editUser.EditUser = user;
        }
    }
    public class GUIRegister : GUITab
    {
        public GUIRegister(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            ((DateTimePicker)this.TabPage.Controls["dtpRegisterBirthday"]).MaxDate = DateTime.Now;
        }
    }
    public class GUIAddGame : GUITab
    {
        private string name = "";
        private string path = "";
        private int selectedAge = 0;
        private string coverart = "";
        private int? steamId = null;
        private SteamApiGameDetail GameDetails;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ((TextBox)this.TabPage.Controls["tbxAddGameName"]).Text = Convert.ToString(name);
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                ((TextBox)this.TabPage.Controls["tbxAddGamePath"]).Text = Convert.ToString(path);
            }
        }
        public int SelectedAge
        {
            get { return selectedAge; }
            set
            {
                selectedAge = value;
                this.UpdateAgeDisplay();
            }
        }
        public int? SteamId
        {
            get { return steamId; }
            set
            {
                steamId = value;
                ((Label)this.TabPage.Controls["lblAddGameSteamId"]).Text = steamId == null ? "No game Found" : "SteamID: " + Convert.ToString(steamId);
            }
        }
        public string Coverart
        {
            get { return coverart; }
            set
            {
                coverart = value;
                ((TextBox)this.TabPage.Controls["tbxAddGameCoverart"]).Text = Convert.ToString(coverart);
                try
                {
                    ((PictureBox)this.TabPage.Controls["pbxAddGameCoverart"]).Load(coverart);
                }
                catch
                {
                    ((PictureBox)this.TabPage.Controls["pbxAddGameCoverart"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                }
            }
        }
        public GUIAddGame(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            
        }

        public override void Reset()
        {
            Name = "";
            Path = "";
            SelectedAge = 0;
            Coverart = "";
            SteamId = null;
        }
        private void UpdateAgeDisplay()
        {
            ((TextBox)this.TabPage.Controls["tbxAddGameAge"]).Text = Convert.ToString(selectedAge);
            ((PictureBox)this.TabPage.Controls["pbxAddGameAge0"]).Image = selectedAge == 0 ? global::school_games_launcher.Properties.Resources.age_0_selected : global::school_games_launcher.Properties.Resources.age_0;
            ((PictureBox)this.TabPage.Controls["pbxAddGameAge6"]).Image = selectedAge == 6 ? global::school_games_launcher.Properties.Resources.age_6_selected : global::school_games_launcher.Properties.Resources.age_6;
            ((PictureBox)this.TabPage.Controls["pbxAddGameAge12"]).Image = selectedAge == 12 ? global::school_games_launcher.Properties.Resources.age_12_selected : global::school_games_launcher.Properties.Resources.age_12;
            ((PictureBox)this.TabPage.Controls["pbxAddGameAge16"]).Image = selectedAge == 16 ? global::school_games_launcher.Properties.Resources.age_16_selected : global::school_games_launcher.Properties.Resources.age_16;
            ((PictureBox)this.TabPage.Controls["pbxAddGameAge18"]).Image = selectedAge == 18 ? global::school_games_launcher.Properties.Resources.age_18_selected : global::school_games_launcher.Properties.Resources.age_18;
        }
        public async void AutoFillSteamGame(SteamApiGame game)
        {
            if (game != null)
            {
                GameDetails = await Program.app.LoadSteamGameDetails(game.AppId);
                SteamId = game.AppId;
                Name = game.Name;
                Path = "steam://rungameid/" + Convert.ToString(game.AppId);
                if(GameDetails != null)
                {
                    SelectedAge = GameDetails.RequiredAge;
                    Coverart = GameDetails.HeaderImage;
                }
            }
            else
            {
                SteamId = null;
            }
        }
        public void AutoFillSteamGame()
        {
            if (Name != "")
            {
                SteamApiGame game = null;
                try
                {
                    game = Program.app.FindSteamGameById(Convert.ToInt32(Name));
                }
                catch { }
                if(game == null) game = Program.app.FindSteamGameByName(Name);
                this.AutoFillSteamGame(game);
            }
            else
            {
                SteamId = null;
            }
        }
        public void Save()
        {
            // TODO Check if input is valid!!!
            Program.app.CreateGame(this.Name, this.Path, this.selectedAge, this.Coverart, this.SteamId);
            Program.app.Gui.library.Activate();
        }
    }
    public class GUIEditGame : GUITab
    {
        private Game editGame;
        private string name = "";
        private string path = "";
        private int selectedAge = 0;
        private string coverart = "";
        private int? steamId = null;
        private SteamApiGameDetail GameDetails;
        public Game EditGame
        {
            get { return editGame; }
            set
            {
                editGame = value;
                Name = value.Name;
                Path = value.Executable.Path;
                SelectedAge = value.Age;
                Coverart = value.Coverart;
                SteamId = value.SteamId;
                ((Label)this.TabPage.Controls["lblEditGameId"]).Text = "ID: " + Convert.ToString(value.Id);
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ((TextBox)this.TabPage.Controls["tbxEditGameName"]).Text = Convert.ToString(name);
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                ((TextBox)this.TabPage.Controls["tbxEditGamePath"]).Text = Convert.ToString(path);
            }
        }
        public int SelectedAge
        {
            get { return selectedAge; }
            set
            {
                selectedAge = value;
                this.UpdateAgeDisplay();
            }
        }
        public int? SteamId
        {
            get { return steamId; }
            set
            {
                steamId = value;
                ((Label)this.TabPage.Controls["lblEditGameSteamId"]).Text = steamId == null ? "SteamID: null" : "SteamID: " + Convert.ToString(steamId);
            }
        }
        public string Coverart
        {
            get { return coverart; }
            set
            {
                coverart = value;
                ((TextBox)this.TabPage.Controls["tbxEditGameCoverart"]).Text = Convert.ToString(coverart);
                try
                {
                    ((PictureBox)this.TabPage.Controls["pbxEditGameCoverart"]).Load(coverart);
                }
                catch
                {
                    ((PictureBox)this.TabPage.Controls["pbxEditGameCoverart"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                }
            }
        }
        public GUIEditGame(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {

        }

        public override void Reset()
        {
            Name = "";
            Path = "";
            SelectedAge = 0;
            Coverart = "";
            SteamId = null;
        }
        private void UpdateAgeDisplay()
        {
            ((TextBox)this.TabPage.Controls["tbxEditGameAge"]).Text = Convert.ToString(selectedAge);
            ((PictureBox)this.TabPage.Controls["pbxEditGameAge0"]).Image = selectedAge == 0 ? global::school_games_launcher.Properties.Resources.age_0_selected : global::school_games_launcher.Properties.Resources.age_0;
            ((PictureBox)this.TabPage.Controls["pbxEditGameAge6"]).Image = selectedAge == 6 ? global::school_games_launcher.Properties.Resources.age_6_selected : global::school_games_launcher.Properties.Resources.age_6;
            ((PictureBox)this.TabPage.Controls["pbxEditGameAge12"]).Image = selectedAge == 12 ? global::school_games_launcher.Properties.Resources.age_12_selected : global::school_games_launcher.Properties.Resources.age_12;
            ((PictureBox)this.TabPage.Controls["pbxEditGameAge16"]).Image = selectedAge == 16 ? global::school_games_launcher.Properties.Resources.age_16_selected : global::school_games_launcher.Properties.Resources.age_16;
            ((PictureBox)this.TabPage.Controls["pbxEditGameAge18"]).Image = selectedAge == 18 ? global::school_games_launcher.Properties.Resources.age_18_selected : global::school_games_launcher.Properties.Resources.age_18;
        }
        public async void AutoFillSteamGame(SteamApiGame game)
        {
            if (game != null)
            {
                GameDetails = await Program.app.LoadSteamGameDetails(game.AppId);
                SteamId = game.AppId;
                Name = game.Name;
                Path = "steam://rungameid/" + Convert.ToString(game.AppId);
                if (GameDetails != null)
                {
                    SelectedAge = GameDetails.RequiredAge;
                    Coverart = GameDetails.HeaderImage;
                }
            }
            else
            {
                SteamId = null;
            }
        }
        public void AutoFillSteamGame()
        {
            if (Name != "")
            {
                SteamApiGame game = null;
                try
                {
                    game = Program.app.FindSteamGameById(Convert.ToInt32(Name));
                }
                catch { }
                if (game == null) game = Program.app.FindSteamGameByName(Name);
                this.AutoFillSteamGame(game);
            }
            else
            {
                SteamId = null;
            }
        }
        public void Save()
        {
            // TODO Check if input is valid!!!
            EditGame.Set(Name, Path, SelectedAge, Coverart, SteamId);
            Program.app.Gui.gameDetails.Activate();
            Program.app.Gui.gameDetails.SetGame(EditGame);
        }
        public void Delete()
        {
            Program.app.Games.Remove(this.EditGame);
            Program.app.Gui.library.Activate();

        }
    }
    public class GUIGameDetails : GUITab
    {
        private Game game;
        public List<PictureBox> ScreenshotList;
        public Game Game { get { return game; } }
        public GUIGameDetails(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {
            
        }
        public override void Setup()
        {
            ((PictureBox)this.TabPage.Controls["pbxGameDetailsPlayButton"]).Click += this.LaunchGame;
            ((LinkLabel)this.TabPage.Controls["llblGameDetailsEdit"]).Click += this.EditGame;
        }
        public override void Update()
        {
            if(Program.app.ActiveUser != null) ((LinkLabel)this.TabPage.Controls["llblGameDetailsEdit"]).Visible = Program.app.ActiveUser.Admin;
        }
        public void LaunchGame(object sender, EventArgs e)
        {
            Program.app.Launch(Game);
        }
        public void EditGame(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.Activate();
            Program.app.Gui.editGame.EditGame = Game;
        }
        public async void SetGame(Game game)
        {
            this.game = game;

            // set basic information
            ((Label)this.TabPage.Controls["lblGameDetailsName"]).Text = game.Name;
            ((Label)this.TabPage.Controls["lblGameDetailsAge"]).Text = "Age: " + Convert.ToString(game.Age);
            ((Label)this.TabPage.Controls["lblGameDetailsId"]).Text = "ID: " + Convert.ToString(game.Id);
            
            // clear screenshots
            this.TabPage.Controls["flpGameDetailsImages"].Controls.Clear();

            // set coverart
            PictureBox coverart = ((PictureBox)this.TabPage.Controls["pbxGameDetailsCoverart"]); 
            try
            {
                coverart.Load(game.Coverart);
            }
            catch
            {
                coverart.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
            }

            // set age image
            ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Visible = true;
            switch (game.Age)
            {
                case 0:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Image = global::school_games_launcher.Properties.Resources.age_0;
                    break;
                case 6:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Image = global::school_games_launcher.Properties.Resources.age_6;
                    break;
                case 12:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Image = global::school_games_launcher.Properties.Resources.age_12;
                    break;
                case 16:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Image = global::school_games_launcher.Properties.Resources.age_16;
                    break;
                case 18:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Image = global::school_games_launcher.Properties.Resources.age_18;
                    break;
                default:
                    ((PictureBox)this.TabPage.Controls["pbxGameDetailsAge"]).Visible = false;
                    break;
            }

            // clear steam info labels
            ((Label)this.TabPage.Controls["lblGameDetailsDeveloper"]).Text = "";
            ((Label)this.TabPage.Controls["lblGameDetailsPublisher"]).Text = "";
            ((TextBox)this.TabPage.Controls["tbxGameDetailsDescription"]).Text = "";
            ((Label)this.TabPage.Controls["lblGameDetailsSteamId"]).Text = "";

            // if game is steam game
            if (game.SteamId.HasValue)
            {
                // display loading in labels
                ((Label)this.TabPage.Controls["lblGameDetailsDeveloper"]).Text = "Developer: [loading]";
                ((Label)this.TabPage.Controls["lblGameDetailsPublisher"]).Text = "Publisher: [loading]";
                ((Label)this.TabPage.Controls["lblGameDetailsSteamId"]).Text = "SteamID: " + game.SteamId;

                // load game details
                var gameDetails = await Program.app.LoadSteamGameDetails(game.SteamId.Value);

                // if loaded successfully
                if(gameDetails != null)
                {
                    // fill info
                    ((Label)this.TabPage.Controls["lblGameDetailsDeveloper"]).Text = "Developer: " + gameDetails.Developer;
                    ((Label)this.TabPage.Controls["lblGameDetailsPublisher"]).Text = "Publisher: " + gameDetails.Publisher;
                    ((TextBox)this.TabPage.Controls["tbxGameDetailsDescription"]).Text = gameDetails.ShortDescription;

                    ScreenshotList = new List<PictureBox>();

                    // display game screenshots
                    for (int i = 0; i < gameDetails.Screenshots.Count; i++)
                    {
                        string screenshot = gameDetails.Screenshots[i];

                        PictureBox coverartPictureBox = new System.Windows.Forms.PictureBox();
                        try
                        {
                            coverartPictureBox.Load(screenshot);
                        }
                        catch
                        {
                            coverartPictureBox.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                        }
                        coverartPictureBox.Size = new System.Drawing.Size(240, 135);
                        coverartPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                        coverartPictureBox.BackColor = System.Drawing.Color.Black;

                        coverartPictureBox.Visible = i < 6;

                        ScreenshotList.Add(coverartPictureBox);
                        this.TabPage.Controls["flpGameDetailsImages"].Controls.Add(coverartPictureBox);
                    }
                    if(ScreenshotList.Count() > 6)
                    {
                        PictureBox morePictureBox = new System.Windows.Forms.PictureBox();
                        morePictureBox.Image = global::school_games_launcher.Properties.Resources.show_more_images;
                        morePictureBox.Size = new System.Drawing.Size(240, 135);
                        morePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                        morePictureBox.BackColor = System.Drawing.Color.Black;

                        ScreenshotList.Add(morePictureBox);

                        void ShowAllScreenshots(object sender, EventArgs e)
                        {
                            foreach (PictureBox coverartPictureBox in ScreenshotList)
                            {
                                coverartPictureBox.Visible = true;
                            }
                            ScreenshotList[ScreenshotList.Count() - 1].Visible = false;
                        }
                        morePictureBox.Click += new System.EventHandler(ShowAllScreenshots);

                        this.TabPage.Controls["flpGameDetailsImages"].Controls.Add(morePictureBox);
                    }
                }
            }
        }
    }
    public class GUIEditUser : GUITab
    {
        private User editUser;
        private string name = "";
        private string avatar = "";
        private bool admin = false;
        private DateTime birthDate;
        public User EditUser
        {
            get { return editUser; }
            set
            {
                editUser = value;
                Name = value.Name;
                Avatar = value.Avatar;
                Admin = value.Admin;
                BirthDate = value.BirthDate;
                ((Label)this.TabPage.Controls["lblEditUserNameOriginal"]).Text = value.Name;
                ((Label)this.TabPage.Controls["lblEditUserId"]).Text = "ID: " + Convert.ToString(value.Id);

                Program.app.Gui.SetAvatar(((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]), editUser.Avatar);
                Program.app.Gui.SetAvatar(((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]), editUser.Avatar);

                ((Label)this.TabPage.Controls["lblEditUserAdminWarning0"]).Visible = editUser.Id == 0;
                ((CheckBox)this.TabPage.Controls["cbxEditUserAdmin"]).Enabled = editUser.Id != 0;
                ((LinkLabel)this.TabPage.Controls["llblEditUserDelete"]).Visible = Program.app.ActiveUser.Admin && !editUser.Admin;

                ((Label)this.TabPage.Controls["lblEditUserBirthdate"]).Visible =
                    ((DateTimePicker)this.TabPage.Controls["dtpEditUserBirthdate"]).Visible =
                    ((CheckBox)this.TabPage.Controls["cbxEditUserAdmin"]).Visible = Program.app.ActiveUser.Admin;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ((TextBox)this.TabPage.Controls["tbxEditUserName"]).Text = Convert.ToString(name);
            }
        }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                ((DateTimePicker)this.TabPage.Controls["dtpEditUserBirthdate"]).Value = value;
            }
        }
        public string Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
                ((TextBox)this.TabPage.Controls["tbxEditUserAvatarInput"]).Text = Convert.ToString(avatar);

                Program.app.Gui.SetAvatar(((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]), avatar);
            }
        }
        public bool Admin
        {
            get { return admin; }
            set
            {
                admin = value;
                ((CheckBox)this.TabPage.Controls["cbxEditUserAdmin"]).Checked = admin;
            }
        }
        public GUIEditUser(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }
        public override void Update()
        {
            ((DateTimePicker)this.TabPage.Controls["dtpEditUserBirthdate"]).MaxDate = DateTime.Now;
        }
        public override void Reset()
        {
            Name = "";
            Avatar = "";
        }
        public void SetOldAvatar()
        {
            this.Avatar = this.EditUser.Avatar;
        }
        public void Save()
        {
            if (Program.app.ActiveUser.Admin || Program.app.ActiveUser == EditUser)
            {
                EditUser.Name = Name;
                EditUser.Avatar = Avatar;
                EditUser.Admin = Admin;
                if (Program.app.ActiveUser.Admin) EditUser.BirthDate = BirthDate;
                Program.app.Gui.profile.Activate();
                Program.app.Gui.profile.User = EditUser;
            }
        }
        public void Delete()
        {
            if(Program.app.ActiveUser.Admin && Program.app.ActiveUser != EditUser)
            {
                Program.app.Users.Remove(EditUser);
            }
        }
        public void Cancel()
        {
            Program.app.Gui.profile.Activate();
            Program.app.Gui.profile.User = this.EditUser;
        }
        public void ChangePassword()
        {
            Program.app.Gui.changePassword.Activate();
            Program.app.Gui.changePassword.EditUser = EditUser;
        }
    }
    public class GUIChangePassword : GUITab
    {
        private User editUser;
        public User EditUser
        {
            get { return editUser; }
            set
            {
                editUser = value;
                ((Label)this.TabPage.Controls["lblChangePasswordName"]).Text = value.Name;
                ((Label)this.TabPage.Controls["lblChangePasswordId"]).Text = "ID: " + Convert.ToString(value.Id);

                Program.app.Gui.SetAvatar(((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]), editUser.Avatar);
            }
        }
        public GUIChangePassword(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }
        public override void Update()
        {

        }
        public override void Reset()
        {
            ((TextBox)this.TabPage.Controls["tbxChangePasswordOld"]).Text = "";
            ((TextBox)this.TabPage.Controls["tbxChangePasswordNew"]).Text = "";
            ((TextBox)this.TabPage.Controls["tbxChangePasswordNewRepeat"]).Text = "";
        }
        public void Cancel()
        {
            Program.app.Gui.editUser.Activate();
        }
        public void Save()
        {
            if(Program.app.ActiveUser == EditUser)
            {
                if (((TextBox)this.TabPage.Controls["tbxChangePasswordNew"]).Text == ((TextBox)this.TabPage.Controls["tbxChangePasswordNewRepeat"]).Text)
                {
                    EditUser.SetPassword(((TextBox)this.TabPage.Controls["tbxChangePasswordOld"]).Text, ((TextBox)this.TabPage.Controls["tbxChangePasswordNew"]).Text);
                    Program.app.Gui.editUser.Activate();
                }
            }
        }
    }
    public class GUIWelcome : GUITab
    {
        public GUIWelcome(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            if (Program.app.ActiveUser != null)
            {
                ((GroupBox)this.TabPage.Controls["gbxWelcomePassword"]).Enabled = (Program.app.ActiveUser.Name == "admin" && Program.app.ActiveUser.Id == 0 && Program.app.ActiveUser.PasswordHash == "");
            }
        }

        public void SetPassword()
        {
            var password = ((GroupBox)this.TabPage.Controls["gbxWelcomePassword"]).Controls["tbxWelcomePassword"].Text;
            Program.app.GetUserByName("admin").SetPassword("", password);
            this.Update();
        }

        public void AddGame()
        {
            Program.app.Gui.addGame.Activate();
        }

        public void Skip()
        {
            Program.app.Gui.library.Activate();
        }
    }
}
