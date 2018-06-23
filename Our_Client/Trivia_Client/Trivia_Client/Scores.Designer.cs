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
            this.Scoreboard = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Scoreboard)).BeginInit();
            this.SuspendLayout();
            // 
            // Scoreboard
            // 
            this.Scoreboard.Image = global::Trivia_Client.Properties.Resources.Scoreboard;
            this.Scoreboard.Location = new System.Drawing.Point(172, 20);
            this.Scoreboard.Name = "Scoreboard";
            this.Scoreboard.Size = new System.Drawing.Size(225, 38);
            this.Scoreboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Scoreboard.TabIndex = 0;
            this.Scoreboard.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(472, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Scores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(86)))));
            this.ClientSize = new System.Drawing.Size(568, 374);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Scoreboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Scores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scores";
            ((System.ComponentModel.ISupportInitialize)(this.Scoreboard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Scoreboard;
        private System.Windows.Forms.Button button1;
    }
}