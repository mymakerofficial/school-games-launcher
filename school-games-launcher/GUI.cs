using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace school_games_launcher
{
    public class GUI
    {
        public GUILibrary library;
        public GUITab options;
        public GUIAddGame addGame;
        public GUITab editGame;
        public GUITab profile;
        public GUITab login;
        public GUITab register;
        public GUIPlaying playing;
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
            editGame = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabEditGame"]);
            profile = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabProfile"]);
            login = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabLogin"]);
            register = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabRegister"]);
            playing = new GUIPlaying(this.tabControl, (TabPage)this.tabControl.TabPages["tabPlaying"]);

            this.tabControl.Appearance = TabAppearance.FlatButtons;
            this.tabControl.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl.SizeMode = TabSizeMode.Fixed;
        }
    }
    public class GUITab
    {
        private TabPage tabPage;
        private TabControl tabControl;
        public TabPage TabPage { get { return tabPage; } }
        public TabControl TabControl { get { return tabControl; } }
        public bool updateOnActive = false;

        public GUITab(TabControl tabControl, TabPage tabPage)
        {
            this.tabControl = tabControl;
            this.tabPage = tabPage;
        }
        public void Activate()
        {
            this.tabControl.SelectedTab = this.tabPage;
            if(this.updateOnActive) this.Update();
        }
        public virtual void Update()
        {
        }
    }
    public class GUILibrary : GUITab
    {
        public string search = "";
        public List<GroupBox> groups = new List<GroupBox>();
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
        public void UpdateList(List<Game> games)
        {
            // fixes memory leak ishue
            foreach (Control control in groups)
            {
                control.Dispose();
            }
            groups.Clear();
            
            foreach (Game game in games)
            {
                GroupBox group = new System.Windows.Forms.GroupBox();
                group.Name = "gbxLibraryGame_" + game.Name;
                group.Size = new System.Drawing.Size(200, 300);

                PictureBox coverart = new System.Windows.Forms.PictureBox();
                try
                {
                    coverart.Load(game.Coverart);
                }
                catch
                {
                    coverart.Image = global::school_games_launcher.Properties.Resources.game_thumbnail_placeholder;
                }
                coverart.Location = new System.Drawing.Point(20, 20);
                coverart.Size = new System.Drawing.Size(160, 200);
                coverart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

                PictureBox playButton = new System.Windows.Forms.PictureBox();
                playButton.Image = global::school_games_launcher.Properties.Resources.play_button;
                playButton.Location = new System.Drawing.Point(50, 240);
                playButton.Size = new System.Drawing.Size(100, 50);
                playButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                void LaunchGame(object sender, EventArgs e)
                {
                    Program.app.Launch(game);
                }
                playButton.Click += new System.EventHandler(LaunchGame);

                group.Controls.Add(coverart);
                group.Controls.Add(playButton);

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
                    coverartBox.Image = global::school_games_launcher.Properties.Resources.game_thumbnail_placeholder;
                }
            }
        }
    }
    public class GUIAddGame : GUITab
    {
        private string name = "";
        private string path = "";
        private int selectedAge = 0;
        private string coverart = "";
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
        public string Coverart
        {
            get { return path; }
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
                    ((PictureBox)this.TabPage.Controls["pbxAddGameCoverart"]).Image = global::school_games_launcher.Properties.Resources.game_thumbnail_placeholder;
                }
            }
        }
        public GUIAddGame(TabControl tabControl, TabPage tabPage) : base(tabControl, tabPage)
        {

        }

        public override void Update()
        {
            
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
        public void Save()
        {
            // TODO Check if input is valid!!!
            Program.app.CreateGame(this.Name, this.Path, this.selectedAge, this.Coverart);
            Program.app.Gui.library.Activate();
        }
    }
}
