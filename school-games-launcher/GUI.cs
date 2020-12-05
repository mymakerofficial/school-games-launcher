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
        public GUITab options;
        public GUIAddGame addGame;
        public GUIEditGame editGame;
        public GUIProfile profile;
        public GUITab login;
        public GUITab register;
        public GUIPlaying playing;
        public GUIGameDetails gameDetails;
        public GUIEditUser editUser;
        public GUIChangePassword changePassword;
        public GUITab userRules;

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
            options = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabOptions"]);
            addGame = new GUIAddGame(this.tabControl, (TabPage)this.tabControl.TabPages["tabAddGame"]);
            addGame.resetOnActive = true;
            editGame = new GUIEditGame(this.tabControl, (TabPage)this.tabControl.TabPages["tabEditGame"]);
            profile = new GUIProfile(this.tabControl, (TabPage)this.tabControl.TabPages["tabProfile"]);
            profile.updateOnActive = true;
            login = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabLogin"]);
            login.resetOnActive = true;
            register = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabRegister"]);
            register.resetOnActive = true;
            playing = new GUIPlaying(this.tabControl, (TabPage)this.tabControl.TabPages["tabPlaying"]);
            gameDetails = new GUIGameDetails(this.tabControl, (TabPage)this.tabControl.TabPages["tapGameDetails"]);
            gameDetails.updateOnActive = true;
            gameDetails.Setup();
            editUser = new GUIEditUser(this.tabControl, (TabPage)this.tabControl.TabPages["tabEditUser"]);
            changePassword = new GUIChangePassword(this.tabControl, (TabPage)this.tabControl.TabPages["tabChangePassword"]);
            changePassword.resetOnActive = true;


            this.tabControl.Appearance = TabAppearance.FlatButtons;
            this.tabControl.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl.SizeMode = TabSizeMode.Fixed;
        }
        public void Minimize()
        {
            form.WindowState = FormWindowState.Minimized;
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
        public List<Panel> groups = new List<Panel>();
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
            // fixes memory leak ishue
            foreach (Control control in groups)
            {
                control.Dispose();
            }
            groups.Clear();

            int margin = 12;
            foreach (Game game in games)
            {
                Panel group = new System.Windows.Forms.Panel();
                group.Name = "gbxLibraryGame_" + game.Name;
                group.Size = new System.Drawing.Size(306 + (margin * 2), 143 + (margin * 2));

                PictureBox coverart = new System.Windows.Forms.PictureBox();
                try
                {
                    coverart.Load(game.Coverart);
                }
                catch
                {
                    coverart.Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                }
                coverart.Location = new System.Drawing.Point(margin, margin);
                coverart.Size = new System.Drawing.Size(306, 143);
                coverart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                coverart.BackColor = System.Drawing.Color.Black;
                void LaunchGame(object sender, EventArgs e)
                {
                    Program.app.Gui.gameDetails.SetGame(game);
                    Program.app.Gui.gameDetails.Activate();
                    //Program.app.Launch(game);
                }
                coverart.Click += new System.EventHandler(LaunchGame);

                group.Controls.Add(coverart);

                groups.Add(group);
            }

            this.TabPage.Controls["flpLibraryGameList"].Controls.Clear();
            foreach (Control control in groups)
            {
                this.TabPage.Controls["flpLibraryGameList"].Controls.Add(control);
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
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                ((Label)this.TabPage.Controls["lblProfileName"]).Text = user.Name;
                ((Label)this.TabPage.Controls["lblProfileBirth"]).Text = String.Format("Age: {0} ({1})", user.Age, user.BirthDate.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")));
                ((Label)this.TabPage.Controls["lblProfileId"]).Text = String.Format("ID: {0}", user.Id);

                switch (user.Avatar)
                {
                    case "0":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_0;
                        break;
                    case "1":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_1;
                        break;
                    case "2":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_2;
                        break;
                    case "3":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_3;
                        break;
                    case "4":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_4;
                        break;
                    case "5":
                        ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_5;
                        break;
                    default:
                        try
                        {
                            ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Load(value.Avatar);
                        }
                        catch
                        {
                            ((PictureBox)this.TabPage.Controls["pbxProfileAvatar"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                        }
                        break;
                }
            }
        }
        public GUIProfile(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            this.User = Program.app.ActiveUser;
        }
       
        public void Edit()
        {
            Program.app.Gui.editUser.Activate();
            Program.app.Gui.editUser.EditUser = user;
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
        private DateTime birthDate;
        public User EditUser
        {
            get { return editUser; }
            set
            {
                editUser = value;
                Name = value.Name;
                Avatar = value.Avatar;
                BirthDate = value.BirthDate;
                ((Label)this.TabPage.Controls["lblEditUserNameOriginal"]).Text = value.Name;
                ((Label)this.TabPage.Controls["lblEditUserId"]).Text = "ID: " + Convert.ToString(value.Id);


                switch (editUser.Avatar)
                {
                    case "0":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_0;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_0;
                        break;
                    case "1":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_1;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_1;
                        break;
                    case "2":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_2;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_2;
                        break;
                    case "3":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_3;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_3;
                        break;
                    case "4":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_4;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_4;
                        break;
                    case "5":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.avatar_5;
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.avatar_5;
                        break;
                    default:
                        try
                        {
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Load(value.Avatar);
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Load(value.Avatar);
                        }
                        catch
                        {
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOld"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatarOriginal"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                        }
                        break;
                }
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

                switch (avatar)
                {
                    case "0":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_0;
                        break;
                    case "1":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_1;
                        break;
                    case "2":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_2;
                        break;
                    case "3":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_3;
                        break;
                    case "4":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_4;
                        break;
                    case "5":
                        ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_5;
                        break;
                    default:
                        try
                        {
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Load(avatar);
                        }
                        catch
                        {
                            ((PictureBox)this.TabPage.Controls["pbxEditUserAvatar"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                        }
                        break;
                }
            }
        }
        public GUIEditUser(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }
        public override void Update()
        {

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
                if(Program.app.ActiveUser.Admin) EditUser.BirthDate = BirthDate;
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
            Program.app.Gui.options.Activate();
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


                switch (editUser.Avatar)
                {
                    case "0":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_0;
                        break;
                    case "1":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_1;
                        break;
                    case "2":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_2;
                        break;
                    case "3":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_3;
                        break;
                    case "4":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_4;
                        break;
                    case "5":
                        ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.avatar_5;
                        break;
                    default:
                        try
                        {
                            ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Load(value.Avatar);
                        }
                        catch
                        {
                            ((PictureBox)this.TabPage.Controls["pbxChangePasswordAvatar"]).Image = global::school_games_launcher.Properties.Resources.game_coverart_placeholder;
                        }
                        break;
                }
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
}
