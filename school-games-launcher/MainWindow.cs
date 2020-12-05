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

        private void llblGameDetailsBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxLibraryClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxOptionsClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxAddGameClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxEditGameClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxProfileClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxLoginClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxGameDetailsClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void llblProfileLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.app.Logout();
        }

        private void btnAddGameCancel_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxLibraryMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxOptionsMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxAddGameMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxEditGameMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxProfileMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxRegisterMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxPlayingMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxGameDetailsMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxOptionsHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxAddGameHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxEditGameHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxProfileHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxRegisterHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxGameDetailsHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxLibraryProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxOptionsProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxAddGameProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxEditGameProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxProfileProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxRegisterProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxGameDetailProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxLibraryOptions_Click_1(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxOptionsOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxAddGameOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxEditGameOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxProfileOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxRegisterOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxGameDetailsOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void tabLibrary_Click(object sender, EventArgs e)
        {

        }

        private void btnProfileLogout_Click(object sender, EventArgs e)
        {
            Program.app.Logout();
        }

        private void BtnEditGameRemoveSteam_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SteamId = null;
        }

        private void BtnEditGameGetSteam_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.AutoFillSteamGame();
        }

        private void TbxEditGameName_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.Name = this.tbxEditGameName.Text;
        }

        private void TbxEditGamePath_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.Path = this.tbxEditGamePath.Text;
        }

        private void TbxEditGameAge_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.app.Gui.editGame.SelectedAge = Convert.ToInt32(this.tbxEditGameAge.Text);
            }
            catch { }
        }

        private void TbxEditGameCoverart_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.Coverart = this.tbxEditGameCoverart.Text;
        }

        private void BtnEditGameConfirm_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.Save();
        }

        private void PbxEditGameAge0_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SelectedAge = 0;
        }

        private void PbxEditGameAge6_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SelectedAge = 6;
        }

        private void PbxEditGameAge12_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SelectedAge = 12;
        }

        private void PbxEditGameAge16_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SelectedAge = 16;
        }

        private void PbxEditGameAge18_Click(object sender, EventArgs e)
        {
            Program.app.Gui.editGame.SelectedAge = 18;
        }

        private void BtnEditGameCancel_Click(object sender, EventArgs e)
        {
            Program.app.Gui.gameDetails.Activate();
            Program.app.Gui.gameDetails.SetGame(Program.app.Gui.editGame.EditGame);
        }

        private void pbxEditUserHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxChangePasswordHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxUserRulesHome_Click(object sender, EventArgs e)
        {
            Program.app.Gui.library.Activate();
        }

        private void pbxEditUserProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxChangePasswordProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.profile.Activate();
        }

        private void pbxEditUserOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxChangePasswordOptions_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxUserRulesProfile_Click(object sender, EventArgs e)
        {
            Program.app.Gui.options.Activate();
        }

        private void pbxEditUserMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxChangePasswordMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxUserRulesMinimize_Click(object sender, EventArgs e)
        {
            Program.app.Gui.Minimize();
        }

        private void pbxEditUserClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxChangePasswordClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void pbxUserRulesClose_Click(object sender, EventArgs e)
        {
            Program.app.Exit();
        }

        private void tbxLibrarySearchGame_TextChanged(object sender, EventArgs e)
        {
            Program.app.Gui.library.SearchGames(this.tbxLibrarySearchGame.Text);
        }
    }
}
