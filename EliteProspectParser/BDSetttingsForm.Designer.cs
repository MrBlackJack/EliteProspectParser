namespace EliteProspectParser
{
    partial class BDSetttingsForm
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
            this.btnPanel = new System.Windows.Forms.Panel();
            this.btn_SaveSetting = new System.Windows.Forms.Button();
            this.btn_TestConnect = new System.Windows.Forms.Button();
            this.PropertyBD = new System.Windows.Forms.PropertyGrid();
            this.btnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.btn_SaveSetting);
            this.btnPanel.Controls.Add(this.btn_TestConnect);
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPanel.Location = new System.Drawing.Point(0, 355);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(388, 66);
            this.btnPanel.TabIndex = 3;
            // 
            // btn_SaveSetting
            // 
            this.btn_SaveSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveSetting.Location = new System.Drawing.Point(236, 35);
            this.btn_SaveSetting.Name = "btn_SaveSetting";
            this.btn_SaveSetting.Size = new System.Drawing.Size(140, 23);
            this.btn_SaveSetting.TabIndex = 1;
            this.btn_SaveSetting.Text = "Сохранить настройки";
            this.btn_SaveSetting.UseVisualStyleBackColor = true;
            this.btn_SaveSetting.Click += new System.EventHandler(this.btn_SaveSetting_Click);
            // 
            // btn_TestConnect
            // 
            this.btn_TestConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_TestConnect.Location = new System.Drawing.Point(236, 6);
            this.btn_TestConnect.Name = "btn_TestConnect";
            this.btn_TestConnect.Size = new System.Drawing.Size(140, 23);
            this.btn_TestConnect.TabIndex = 0;
            this.btn_TestConnect.Text = "Тест подключения к БД";
            this.btn_TestConnect.UseVisualStyleBackColor = true;
            this.btn_TestConnect.Click += new System.EventHandler(this.btn_TestConnect_Click);
            // 
            // PropertyBD
            // 
            this.PropertyBD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyBD.Location = new System.Drawing.Point(0, 0);
            this.PropertyBD.Name = "PropertyBD";
            this.PropertyBD.Size = new System.Drawing.Size(388, 355);
            this.PropertyBD.TabIndex = 2;
            this.PropertyBD.ToolbarVisible = false;
            // 
            // BDSetttingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 421);
            this.Controls.Add(this.PropertyBD);
            this.Controls.Add(this.btnPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BDSetttingsForm";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка подключения к БД";
            this.btnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel btnPanel;
        private System.Windows.Forms.Button btn_SaveSetting;
        private System.Windows.Forms.Button btn_TestConnect;
        private System.Windows.Forms.PropertyGrid PropertyBD;


    }
}