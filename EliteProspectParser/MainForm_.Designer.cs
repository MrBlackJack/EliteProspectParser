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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.log_view = new System.Windows.Forms.ListBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblStatus);
            this.splitContainer1.Panel1.Controls.Add(this.lblCaption);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.log_view);
            this.splitContainer1.Size = new System.Drawing.Size(852, 454);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 0;
            // 
            // log_view
            // 
            this.log_view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.log_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_view.FormattingEnabled = true;
            this.log_view.Location = new System.Drawing.Point(0, 0);
            this.log_view.Name = "log_view";
            this.log_view.Size = new System.Drawing.Size(646, 454);
            this.log_view.TabIndex = 11;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(3, 9);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(141, 13);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Всего игроков загружено:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(147, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(13, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "0";
            // 
            // MainForm_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 454);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm_";
            this.Text = "EliteProspectHockey";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ListBox log_view;



    }
}