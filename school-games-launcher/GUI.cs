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
        public GUITab library;
        public GUITab options;
        public GUITab addGame;
        public GUITab editGame;
        public GUITab profile;
        public GUITab login;
        public GUITab register;
        public GUITab playing;
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
            addGame = new GUITab(this.tabControl, (TabPage)this.tabControl.TabPages["tabAddGame"]);
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
            this.UpdateList(Program.app.VisibleGames);
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
                coverart.Load(game.Coverart);
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
                coverartBox.Load(Program.app.ActiveSession.Game.Coverart);
            }
        }
    }
}
