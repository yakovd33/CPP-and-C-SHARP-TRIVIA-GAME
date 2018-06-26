namespace Trivia_Client
{
    partial class Scores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scores));
            this.scoresList = new System.Windows.Forms.Panel();
            this.stamp = new System.Windows.Forms.PictureBox();
            this.usernameTitle = new System.Windows.Forms.PictureBox();
            this.exitBtn = new System.Windows.Forms.PictureBox();
            this.Scoreboard = new System.Windows.Forms.PictureBox();
            this.scoreTitle = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.stamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scoreboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // scoresList
            // 
            this.scoresList.Location = new System.Drawing.Point(42, 112);
            this.scoresList.Name = "scoresList";
            this.scoresList.Size = new System.Drawing.Size(659, 198);
            this.scoresList.TabIndex = 10;
            // 
            // stamp
            // 
            this.stamp.Location = new System.Drawing.Point(281, 317);
            this.stamp.Name = "stamp";
            this.stamp.Size = new System.Drawing.Size(180, 85);
            this.stamp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.stamp.TabIndex = 12;
            this.stamp.TabStop = false;
            // 
            // usernameTitle
            // 
            this.usernameTitle.Image = global::Trivia_Client.Properties.Resources.username;
            this.usernameTitle.Location = new System.Drawing.Point(152, 76);
            this.usernameTitle.Name = "usernameTitle";
            this.usernameTitle.Size = new System.Drawing.Size(148, 30);
            this.usernameTitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.usernameTitle.TabIndex = 9;
            this.usernameTitle.TabStop = false;
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exitBtn.BackgroundImage")));
            this.exitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.Location = new System.Drawing.Point(699, 12);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(30, 30);
            this.exitBtn.TabIndex = 7;
            this.exitBtn.TabStop = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // Scoreboard
            // 
            this.Scoreboard.Image = global::Trivia_Client.Properties.Resources.Scoreboard;
            this.Scoreboard.Location = new System.Drawing.Point(258, 12);
            this.Scoreboard.Name = "Scoreboard";
            this.Scoreboard.Size = new System.Drawing.Size(225, 38);
            this.Scoreboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Scoreboard.TabIndex = 0;
            this.Scoreboard.TabStop = false;
            // 
            // scoreTitle
            // 
            this.scoreTitle.Image = global::Trivia_Client.Properties.Resources.score;
            this.scoreTitle.Location = new System.Drawing.Point(491, 76);
            this.scoreTitle.Name = "scoreTitle";
            this.scoreTitle.Size = new System.Drawing.Size(78, 30);
            this.scoreTitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.scoreTitle.TabIndex = 11;
            this.scoreTitle.TabStop = false;
            // 
            // Scores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(59)))), ((int)(((byte)(67)))));
            this.ClientSize = new System.Drawing.Size(741, 414);
            this.Controls.Add(this.stamp);
            this.Controls.Add(this.scoresList);
            this.Controls.Add(this.usernameTitle);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.Scoreboard);
            this.Controls.Add(this.scoreTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Scores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scores";
            ((System.ComponentModel.ISupportInitialize)(this.stamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scoreboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Scoreboard;
        private System.Windows.Forms.PictureBox exitBtn;
        private System.Windows.Forms.PictureBox usernameTitle;
        private System.Windows.Forms.Panel scoresList;
        private System.Windows.Forms.PictureBox scoreTitle;
        private System.Windows.Forms.PictureBox stamp;
    }
}