namespace school_games_launcher
{
    partial class LogInDialoge
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tbxUserName = new System.Windows.Forms.TextBox();
            this.tbxUserPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(102, 211);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "yeet";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // tbxUserName
            // 
            this.tbxUserName.Location = new System.Drawing.Point(91, 82);
            this.tbxUserName.Name = "tbxUserName";
            this.tbxUserName.Size = new System.Drawing.Size(100, 20);
            this.tbxUserName.TabIndex = 1;
            // 
            // tbxUserPassword
            // 
            this.tbxUserPassword.Location = new System.Drawing.Point(91, 146);
            this.tbxUserPassword.Name = "tbxUserPassword";
            this.tbxUserPassword.Size = new System.Drawing.Size(100, 20);
            this.tbxUserPassword.TabIndex = 2;
            // 
            // LogInDialoge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(301, 331);
            this.Controls.Add(this.tbxUserPassword);
            this.Controls.Add(this.tbxUserName);
            this.Controls.Add(this.btnConfirm);
            this.Name = "LogInDialoge";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LogInDialoge_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox tbxUserName;
        private System.Windows.Forms.TextBox tbxUserPassword;
    }
}