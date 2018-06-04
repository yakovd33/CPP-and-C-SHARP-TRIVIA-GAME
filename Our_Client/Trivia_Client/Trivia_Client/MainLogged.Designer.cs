namespace Trivia_Client
{
    partial class MainLogged
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainLogged));
            this.panel2 = new System.Windows.Forms.Panel();
            this.exitBtn = new System.Windows.Forms.PictureBox();
            this.sidebar = new System.Windows.Forms.Panel();
            this.sidebarItem3 = new System.Windows.Forms.Panel();
            this.sidebarIcon3 = new System.Windows.Forms.PictureBox();
            this.sidebarItem2 = new System.Windows.Forms.Panel();
            this.sidebarIcon2 = new System.Windows.Forms.PictureBox();
            this.sidebarItem1 = new System.Windows.Forms.Panel();
            this.sidebarActivePanelIndicator = new System.Windows.Forms.Panel();
            this.sidebarIcon1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            this.sidebar.SuspendLayout();
            this.sidebarItem3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon3)).BeginInit();
            this.sidebarItem2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon2)).BeginInit();
            this.sidebarItem1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sidebarActivePanelIndicator);
            this.panel2.Controls.Add(this.exitBtn);
            this.panel2.Controls.Add(this.sidebar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(919, 596);
            this.panel2.TabIndex = 8;
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
            this.exitBtn.TabIndex = 6;
            this.exitBtn.TabStop = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            this.exitBtn.MouseLeave += new System.EventHandler(this.exitBtn_MouseLeave);
            this.exitBtn.MouseHover += new System.EventHandler(this.exitBtn_MouseHover);
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.sidebar.Controls.Add(this.sidebarItem3);
            this.sidebar.Controls.Add(this.sidebarItem2);
            this.sidebar.Controls.Add(this.sidebarItem1);
            this.sidebar.Controls.Add(this.pictureBox2);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(65, 596);
            this.sidebar.TabIndex = 0;
            // 
            // sidebarItem3
            // 
            this.sidebarItem3.Controls.Add(this.sidebarIcon3);
            this.sidebarItem3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sidebarItem3.Location = new System.Drawing.Point(0, 193);
            this.sidebarItem3.Name = "sidebarItem3";
            this.sidebarItem3.Size = new System.Drawing.Size(65, 57);
            this.sidebarItem3.TabIndex = 9;
            // 
            // sidebarIcon3
            // 
            this.sidebarIcon3.Image = global::Trivia_Client.Properties.Resources.gear;
            this.sidebarIcon3.Location = new System.Drawing.Point(24, 15);
            this.sidebarIcon3.Name = "sidebarIcon3";
            this.sidebarIcon3.Size = new System.Drawing.Size(25, 25);
            this.sidebarIcon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sidebarIcon3.TabIndex = 7;
            this.sidebarIcon3.TabStop = false;
            // 
            // sidebarItem2
            // 
            this.sidebarItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.sidebarItem2.Controls.Add(this.sidebarIcon2);
            this.sidebarItem2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sidebarItem2.Location = new System.Drawing.Point(0, 136);
            this.sidebarItem2.Name = "sidebarItem2";
            this.sidebarItem2.Size = new System.Drawing.Size(65, 57);
            this.sidebarItem2.TabIndex = 8;
            // 
            // sidebarIcon2
            // 
            this.sidebarIcon2.Image = global::Trivia_Client.Properties.Resources.gear;
            this.sidebarIcon2.Location = new System.Drawing.Point(24, 14);
            this.sidebarIcon2.Name = "sidebarIcon2";
            this.sidebarIcon2.Size = new System.Drawing.Size(25, 25);
            this.sidebarIcon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sidebarIcon2.TabIndex = 7;
            this.sidebarIcon2.TabStop = false;
            // 
            // sidebarItem1
            // 
            this.sidebarItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.sidebarItem1.Controls.Add(this.sidebarIcon1);
            this.sidebarItem1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sidebarItem1.Location = new System.Drawing.Point(0, 79);
            this.sidebarItem1.Name = "sidebarItem1";
            this.sidebarItem1.Size = new System.Drawing.Size(65, 57);
            this.sidebarItem1.TabIndex = 2;
            // 
            // sidebarActivePanelIndicator
            // 
            this.sidebarActivePanelIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(210)))), ((int)(((byte)(139)))));
            this.sidebarActivePanelIndicator.Location = new System.Drawing.Point(0, 79);
            this.sidebarActivePanelIndicator.Name = "sidebarActivePanelIndicator";
            this.sidebarActivePanelIndicator.Size = new System.Drawing.Size(5, 57);
            this.sidebarActivePanelIndicator.TabIndex = 7;
            // 
            // sidebarIcon1
            // 
            this.sidebarIcon1.Image = global::Trivia_Client.Properties.Resources.gear;
            this.sidebarIcon1.Location = new System.Drawing.Point(24, 16);
            this.sidebarIcon1.Name = "sidebarIcon1";
            this.sidebarIcon1.Size = new System.Drawing.Size(25, 25);
            this.sidebarIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sidebarIcon1.TabIndex = 7;
            this.sidebarIcon1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::Trivia_Client.Properties.Resources.io;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(17, 548);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 31);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // MainLogged
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(919, 596);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainLogged";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainLogged";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            this.sidebar.ResumeLayout(false);
            this.sidebarItem3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon3)).EndInit();
            this.sidebarItem2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon2)).EndInit();
            this.sidebarItem1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sidebarIcon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.PictureBox exitBtn;
        private System.Windows.Forms.Panel sidebarItem1;
        private System.Windows.Forms.PictureBox sidebarIcon1;
        private System.Windows.Forms.Panel sidebarItem3;
        private System.Windows.Forms.PictureBox sidebarIcon3;
        private System.Windows.Forms.Panel sidebarItem2;
        private System.Windows.Forms.PictureBox sidebarIcon2;
        private System.Windows.Forms.Panel sidebarActivePanelIndicator;
    }
}