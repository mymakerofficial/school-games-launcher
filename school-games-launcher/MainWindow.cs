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

        private void BtnLibrarySaveData_Click(object sender, EventArgs e)
        {
            Program.app.SaveData();
        }

        private void BtnRegisterConfirm_Click(object sender, EventArgs e)
        {
            if(this.tbxRegisterPassword.Text == this.tbxRegisterPasswordConfirm.Text)
            {
                Program.app.CreateUser(this.tbxRegisterUsername.Text, this.dtpRegisterBirthday.Value, this.tbxRegisterPassword.Text);
                Program.app.Gui.login.Activate();
            }
        }

        private void btnAddGameConfirm_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.Save();
        }

        private void lblAddGameAge_Click(object sender, EventArgs e)
        {

        }

        private void pbxAddGameAge0_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.SelectedAge = 0;
        }

        private void pbxAddGameAge6_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.SelectedAge = 6;
        }

        private void pbxAddGameAge12_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.SelectedAge = 12;
        }

        private void pbxAddGameAge16_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.SelectedAge = 16;
        }

        private void pbxAddGameAge18_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.SelectedAge = 18;
        }

        private void lblAddGamePath_Click(object sender, EventArgs e)
        {

        }

        private void tbxAddGameAge_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.app.Gui.addGame.SelectedAge = Convert.ToInt32(this.tbxAddGameAge.Text);
            }
            catch { }
        }

        private void tbxAddGameName_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.Name = this.tbxAddGameName.Text;
        }

        private void tbxAddGamePath_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.Path = this.tbxAddGamePath.Text;
        }

        private void tbxAddGameCoverart_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.Coverart = this.tbxAddGameCoverart.Text;
        }

        private void btnOptionsAddUser_Click(object sender, EventArgs e)
        {
            Program.app.Gui.register.Activate();
        }

        private void llblRegisterCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void btnAddGameFindSteamGame_Click(object sender, EventArgs e)
        {
            Program.app.Gui.addGame.AutoFillSteamGame();
        }
    }
}
