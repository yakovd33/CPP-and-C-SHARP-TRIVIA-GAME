namespace Trivia_Client
{
    partial class Pass_Reset_Screen
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
            this.emailBox = new System.Windows.Forms.TextBox();
            this.usernameWrap = new System.Windows.Forms.Panel();
            this.loginBtn = new System.Windows.Forms.Button();
            this.logo = new System.Windows.Forms.PictureBox();
            this.exitBtn = new System.Windows.Forms.PictureBox();
            this.logoCopy = new System.Windows.Forms.PictureBox();
            this.usernameWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoCopy)).BeginInit();
            this.SuspendLayout();
            // 
            // emailBox
            // 
            this.emailBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.emailBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.emailBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.emailBox.Location = new System.Drawing.Point(9, 7);
            this.emailBox.Name = "emailBox";
            this.emailBox.Size = new System.Drawing.Size(400, 19);
            this.emailBox.TabIndex = 0;
            this.emailBox.TabStop = false;
            this.emailBox.Text = "E-mail";
            this.emailBox.Enter += new System.EventHandler(this.usernameBox_Enter);
            // 
            // usernameWrap
            // 
            this.usernameWrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.usernameWrap.Controls.Add(this.emailBox);
            this.usernameWrap.Location = new System.Drawing.Point(259, 257);
            this.usernameWrap.Name = "usernameWrap";
            this.usernameWrap.Size = new System.Drawing.Size(420, 36);
            this.usernameWrap.TabIndex = 1;
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(83)))), ((int)(((byte)(101)))));
            this.loginBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginBtn.FlatAppearance.BorderSize = 0;
            this.loginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(145)))), ((int)(((byte)(156)))));
            this.loginBtn.Location = new System.Drawing.Point(377, 331);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(200, 36);
            this.loginBtn.TabIndex = 3;
            this.loginBtn.TabStop = false;
            this.loginBtn.Text = "OK";
            this.loginBtn.UseVisualStyleBackColor = false;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
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
            // Pass_Reset_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(919, 596);
            this.ControlBox = false;
            this.Controls.Add(this.logoCopy);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.usernameWrap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Pass_Reset_Screen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.usernameWrap.ResumeLayout(false);
            this.usernameWrap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoCopy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Panel usernameWrap;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.PictureBox exitBtn;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.PictureBox logoCopy;
    }
}

