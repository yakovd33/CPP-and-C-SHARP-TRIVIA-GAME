namespace Trivia_Client
{
    partial class LogInScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogInScreen));
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.usernameWrap = new System.Windows.Forms.Panel();
            this.passwordWrap = new System.Windows.Forms.Panel();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.SignupBtn = new System.Windows.Forms.Button();
            this.loginFeedbackLabel = new System.Windows.Forms.Label();
            this.pass_reset = new System.Windows.Forms.Label();
            this.logoCopy = new System.Windows.Forms.PictureBox();
            this.logo = new System.Windows.Forms.PictureBox();
            this.exitBtn = new System.Windows.Forms.PictureBox();
            this.showPassBtn = new System.Windows.Forms.PictureBox();
            this.usernameWrap.SuspendLayout();
            this.passwordWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showPassBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // usernameBox
            // 
            this.usernameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.usernameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.usernameBox.Location = new System.Drawing.Point(9, 7);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(400, 19);
            this.usernameBox.TabIndex = 0;
            this.usernameBox.TabStop = false;
            this.usernameBox.Text = "Username";
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            this.usernameBox.Enter += new System.EventHandler(this.usernameBox_Enter);
            // 
            // usernameWrap
            // 
            this.usernameWrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.usernameWrap.Controls.Add(this.usernameBox);
            this.usernameWrap.Location = new System.Drawing.Point(259, 257);
            this.usernameWrap.Name = "usernameWrap";
            this.usernameWrap.Size = new System.Drawing.Size(420, 36);
            this.usernameWrap.TabIndex = 1;
            // 
            // passwordWrap
            // 
            this.passwordWrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.passwordWrap.Controls.Add(this.showPassBtn);
            this.passwordWrap.Controls.Add(this.passwordBox);
            this.passwordWrap.Location = new System.Drawing.Point(259, 310);
            this.passwordWrap.Name = "passwordWrap";
            this.passwordWrap.Size = new System.Drawing.Size(420, 36);
            this.passwordWrap.TabIndex = 2;
            // 
            // passwordBox
            // 
            this.passwordBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.passwordBox.Location = new System.Drawing.Point(9, 7);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(410, 19);
            this.passwordBox.TabIndex = 0;
            this.passwordBox.TabStop = false;
            this.passwordBox.Text = "Password";
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            this.passwordBox.Enter += new System.EventHandler(this.passwordBox_Enter);
            this.passwordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordBox_KeyDown);
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(83)))), ((int)(((byte)(101)))));
            this.loginBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginBtn.FlatAppearance.BorderSize = 0;
            this.loginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.loginBtn.Location = new System.Drawing.Point(260, 361);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(200, 36);
            this.loginBtn.TabIndex = 3;
            this.loginBtn.TabStop = false;
            this.loginBtn.Text = "Login";
            this.loginBtn.UseVisualStyleBackColor = false;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // SignupBtn
            // 
            this.SignupBtn.BackColor = System.Drawing.Color.Transparent;
            this.SignupBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SignupBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(83)))), ((int)(((byte)(101)))));
            this.SignupBtn.FlatAppearance.BorderSize = 3;
            this.SignupBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SignupBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(83)))), ((int)(((byte)(101)))));
            this.SignupBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignupBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignupBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.SignupBtn.Location = new System.Drawing.Point(478, 361);
            this.SignupBtn.Name = "SignupBtn";
            this.SignupBtn.Size = new System.Drawing.Size(200, 36);
            this.SignupBtn.TabIndex = 4;
            this.SignupBtn.TabStop = false;
            this.SignupBtn.Text = "Signup";
            this.SignupBtn.UseVisualStyleBackColor = false;
            this.SignupBtn.Click += new System.EventHandler(this.SignupBtn_Click);
            // 
            // loginFeedbackLabel
            // 
            this.loginFeedbackLabel.AutoSize = true;
            this.loginFeedbackLabel.ForeColor = System.Drawing.Color.Maroon;
            this.loginFeedbackLabel.Location = new System.Drawing.Point(256, 437);
            this.loginFeedbackLabel.Name = "loginFeedbackLabel";
            this.loginFeedbackLabel.Size = new System.Drawing.Size(52, 13);
            this.loginFeedbackLabel.TabIndex = 7;
            this.loginFeedbackLabel.Text = "feedback";
            this.loginFeedbackLabel.Visible = false;
            // 
            // pass_reset
            // 
            this.pass_reset.AutoSize = true;
            this.pass_reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.pass_reset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.pass_reset.Location = new System.Drawing.Point(256, 414);
            this.pass_reset.Name = "pass_reset";
            this.pass_reset.Size = new System.Drawing.Size(92, 13);
            this.pass_reset.TabIndex = 9;
            this.pass_reset.Text = "Forgot Password?";
            this.pass_reset.Click += new System.EventHandler(this.pass_reset_Click);
            // 
            // logoCopy
            // 
            this.logoCopy.Image = global::Trivia_Client.Properties.Resources.logo;
            this.logoCopy.Location = new System.Drawing.Point(377, 180);
            this.logoCopy.Name = "logoCopy";
            this.logoCopy.Size = new System.Drawing.Size(165, 46);
            this.logoCopy.TabIndex = 8;
            this.logoCopy.TabStop = false;
            this.logoCopy.Visible = false;
            // 
            // logo
            // 
            this.logo.Image = global::Trivia_Client.Properties.Resources.logo;
            this.logo.Location = new System.Drawing.Point(377, 180);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(165, 46);
            this.logo.TabIndex = 6;
            this.logo.TabStop = false;
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exitBtn.BackgroundImage")));
            this.exitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.Location = new System.Drawing.Point(877, 12);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(30, 30);
            this.exitBtn.TabIndex = 5;
            this.exitBtn.TabStop = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            this.exitBtn.MouseLeave += new System.EventHandler(this.exitBtn_MouseLeave);
            this.exitBtn.MouseHover += new System.EventHandler(this.exitBtn_MouseHover);
            // 
            // showPassBtn
            // 
            this.showPassBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.showPassBtn.Image = global::Trivia_Client.Properties.Resources.eye;
            this.showPassBtn.Location = new System.Drawing.Point(387, 7);
            this.showPassBtn.Name = "showPassBtn";
            this.showPassBtn.Size = new System.Drawing.Size(22, 22);
            this.showPassBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.showPassBtn.TabIndex = 9;
            this.showPassBtn.TabStop = false;
            this.showPassBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.showPassBtn_MouseDown);
            this.showPassBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.showPassBtn_MouseUp);
            // 
            // LogInScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(919, 596);
            this.ControlBox = false;
            this.Controls.Add(this.pass_reset);
            this.Controls.Add(this.logoCopy);
            this.Controls.Add(this.loginFeedbackLabel);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.SignupBtn);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.passwordWrap);
            this.Controls.Add(this.usernameWrap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogInScreen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "v";
            this.Load += new System.EventHandler(this.LogInScreen_Load);
            this.usernameWrap.ResumeLayout(false);
            this.usernameWrap.PerformLayout();
            this.passwordWrap.ResumeLayout(false);
            this.passwordWrap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showPassBtn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Panel usernameWrap;
        private System.Windows.Forms.Panel passwordWrap;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.Button SignupBtn;
        private System.Windows.Forms.PictureBox exitBtn;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Label loginFeedbackLabel;
        private System.Windows.Forms.PictureBox logoCopy;
        private System.Windows.Forms.PictureBox showPassBtn;
        private System.Windows.Forms.Label pass_reset;
    }
}

