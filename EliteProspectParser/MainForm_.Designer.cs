namespace EliteProspectParser
{
    partial class MainForm_
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm_));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_BDSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.lblLeagues = new System.Windows.Forms.Label();
            this.ListLeague = new System.Windows.Forms.CheckedListBox();
            this.btn_startPars = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCaption = new System.Windows.Forms.Label();
            this.log_view = new System.Windows.Forms.ListBox();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainMenu.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1395, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_BDSettings});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // Menu_BDSettings
            // 
            this.Menu_BDSettings.Name = "Menu_BDSettings";
            this.Menu_BDSettings.Size = new System.Drawing.Size(179, 22);
            this.Menu_BDSettings.Text = "Подключение к БД";
            this.Menu_BDSettings.Click += new System.EventHandler(this.Menu_BDSettings_Click);
            // 
            // infoPanel
            // 
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Controls.Add(this.lblLeagues);
            this.infoPanel.Controls.Add(this.ListLeague);
            this.infoPanel.Controls.Add(this.btn_startPars);
            this.infoPanel.Controls.Add(this.lblTime);
            this.infoPanel.Controls.Add(this.lblTValue);
            this.infoPanel.Controls.Add(this.lblStatus);
            this.infoPanel.Controls.Add(this.lblCaption);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.infoPanel.Location = new System.Drawing.Point(0, 24);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(422, 586);
            this.infoPanel.TabIndex = 2;
            // 
            // lblLeagues
            // 
            this.lblLeagues.AutoSize = true;
            this.lblLeagues.Location = new System.Drawing.Point(12, 104);
            this.lblLeagues.Name = "lblLeagues";
            this.lblLeagues.Size = new System.Drawing.Size(104, 13);
            this.lblLeagues.TabIndex = 11;
            this.lblLeagues.Text = "Загружаемые лиги";
            // 
            // ListLeague
            // 
            this.ListLeague.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListLeague.FormattingEnabled = true;
            this.ListLeague.Location = new System.Drawing.Point(11, 128);
            this.ListLeague.Name = "ListLeague";
            this.ListLeague.Size = new System.Drawing.Size(404, 439);
            this.ListLeague.TabIndex = 10;
            this.ListLeague.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListLeague_ItemCheck);
            // 
            // btn_startPars
            // 
            this.btn_startPars.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_startPars.Location = new System.Drawing.Point(11, 59);
            this.btn_startPars.Name = "btn_startPars";
            this.btn_startPars.Size = new System.Drawing.Size(80, 23);
            this.btn_startPars.TabIndex = 9;
            this.btn_startPars.Text = "Старт";
            this.btn_startPars.UseVisualStyleBackColor = true;
            this.btn_startPars.Click += new System.EventHandler(this.btn_startPars_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(8, 32);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(96, 13);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "Времени прошло:";
            // 
            // lblTValue
            // 
            this.lblTValue.AutoSize = true;
            this.lblTValue.Location = new System.Drawing.Point(152, 32);
            this.lblTValue.Name = "lblTValue";
            this.lblTValue.Size = new System.Drawing.Size(49, 13);
            this.lblTValue.TabIndex = 7;
            this.lblTValue.Text = "00:00:00";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(152, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(13, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "0";
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(8, 9);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(141, 13);
            this.lblCaption.TabIndex = 5;
            this.lblCaption.Text = "Всего игроков загружено:";
            // 
            // log_view
            // 
            this.log_view.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.log_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_view.FormattingEnabled = true;
            this.log_view.Location = new System.Drawing.Point(422, 24);
            this.log_view.Name = "log_view";
            this.log_view.Size = new System.Drawing.Size(973, 586);
            this.log_view.TabIndex = 12;
            // 
            // notify
            // 
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Text = "Планировщик";
            this.notify.Visible = true;
            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_MouseDoubleClick);
            // 
            // MainForm_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1395, 610);
            this.Controls.Add(this.log_view);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm_";
            this.Text = "EliteProspectHockey";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_BDSettings;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Button btn_startPars;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblTValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ListBox log_view;
        private System.Windows.Forms.Label lblLeagues;
        private System.Windows.Forms.CheckedListBox ListLeague;
        private System.Windows.Forms.NotifyIcon notify;



    }
}