using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace school_games_launcher
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void llblAddGameCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void llblLoginRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.register.Activate();
        }

        private void llblRegisterLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.login.Activate();
        }

        private void pbxLibraryUserAvatar_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void btnPlayingStop_Click(object sender, EventArgs e)
        {
            Program.app.ActiveSession.End();
        }

        private void btnLoginConfirm_Click(object sender, EventArgs e)
        {
            bool valid = Program.app.LoginUser(this.tbxLoginUsername.Text, this.tbxLoginPassword.Text);
            if (valid)
            {
                Program.app.Gui.library.Activate();
            }
        }

        private void btnProfileLogout_Click(object sender, EventArgs e)
        {
            Program.app.Logout();
        }

        private void pbxLibraryOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.Activate();
        }
    }
}
