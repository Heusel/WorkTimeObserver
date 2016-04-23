namespace WindowsFormsWorkTimeApplication
{
  partial class WorkTimeGui
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkTimeGui));
      this.label1 = new System.Windows.Forms.Label();
      this.labelWorkingTime = new System.Windows.Forms.Label();
      this.GuiUpdateTimer = new System.Windows.Forms.Timer(this.components);
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.labelCoffeeBreak = new System.Windows.Forms.Label();
      this.labelLunchBreak = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.contextMenuStrip_Notification = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripMenuItem_About = new System.Windows.Forms.ToolStripMenuItem();
      this.labelDate = new System.Windows.Forms.Label();
      this.labelStartTime = new System.Windows.Forms.Label();
      this.labelCorrection = new System.Windows.Forms.Label();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.timeNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
      this.contextMenuStrip_Notification.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Working Time";
      // 
      // labelWorkingTime
      // 
      this.labelWorkingTime.AutoSize = true;
      this.labelWorkingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
      this.labelWorkingTime.Location = new System.Drawing.Point(35, 33);
      this.labelWorkingTime.Name = "labelWorkingTime";
      this.labelWorkingTime.Size = new System.Drawing.Size(71, 29);
      this.labelWorkingTime.TabIndex = 1;
      this.labelWorkingTime.Text = "00:00";
      // 
      // GuiUpdateTimer
      // 
      this.GuiUpdateTimer.Interval = 20000;
      this.GuiUpdateTimer.Tick += new System.EventHandler(this.GuiUpdateTimer_Tick);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 81);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(36, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Date: ";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 117);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(61, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Correction: ";
      // 
      // labelCoffeeBreak
      // 
      this.labelCoffeeBreak.AutoSize = true;
      this.labelCoffeeBreak.Enabled = false;
      this.labelCoffeeBreak.Location = new System.Drawing.Point(39, 136);
      this.labelCoffeeBreak.Name = "labelCoffeeBreak";
      this.labelCoffeeBreak.Size = new System.Drawing.Size(69, 13);
      this.labelCoffeeBreak.TabIndex = 4;
      this.labelCoffeeBreak.Text = "Coffee Break";
      // 
      // labelLunchBreak
      // 
      this.labelLunchBreak.AutoSize = true;
      this.labelLunchBreak.Enabled = false;
      this.labelLunchBreak.Location = new System.Drawing.Point(40, 153);
      this.labelLunchBreak.Name = "labelLunchBreak";
      this.labelLunchBreak.Size = new System.Drawing.Size(68, 13);
      this.labelLunchBreak.TabIndex = 5;
      this.labelLunchBreak.Text = "Lunch Break";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(13, 99);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(61, 13);
      this.label7.TabIndex = 6;
      this.label7.Text = "Start Time: ";
      // 
      // notifyIcon
      // 
      this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Error;
      this.notifyIcon.ContextMenuStrip = this.contextMenuStrip_Notification;
      this.notifyIcon.Text = "notifyIcon1";
      this.notifyIcon.Visible = true;
      this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_Click);
      // 
      // contextMenuStrip_Notification
      // 
      this.contextMenuStrip_Notification.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_Exit,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.toolStripSeparator2,
            this.toolStripMenuItem_About});
      this.contextMenuStrip_Notification.Name = "contextMenuStrip_Notification";
      this.contextMenuStrip_Notification.Size = new System.Drawing.Size(153, 126);
      this.contextMenuStrip_Notification.Text = "W.T.O.";
      // 
      // toolStripMenuItem_Open
      // 
      this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
      this.toolStripMenuItem_Open.Size = new System.Drawing.Size(160, 22);
      this.toolStripMenuItem_Open.Text = "Open";
      this.toolStripMenuItem_Open.Click += new System.EventHandler(this.toolStripMenuItem_Open_Click);
      // 
      // toolStripMenuItem_Exit
      // 
      this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
      this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(160, 22);
      this.toolStripMenuItem_Exit.Text = "Exit";
      this.toolStripMenuItem_Exit.Click += new System.EventHandler(this.toolStripMenuItem_Exit_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
      // 
      // toolStripMenuItem_About
      // 
      this.toolStripMenuItem_About.Name = "toolStripMenuItem_About";
      this.toolStripMenuItem_About.Size = new System.Drawing.Size(160, 22);
      this.toolStripMenuItem_About.Text = "About";
      this.toolStripMenuItem_About.Click += new System.EventHandler(this.toolStripMenuItem_About_Click);
      // 
      // labelDate
      // 
      this.labelDate.AutoSize = true;
      this.labelDate.Location = new System.Drawing.Point(47, 81);
      this.labelDate.Name = "labelDate";
      this.labelDate.Size = new System.Drawing.Size(61, 13);
      this.labelDate.TabIndex = 7;
      this.labelDate.Text = "06.04.2016";
      // 
      // labelStartTime
      // 
      this.labelStartTime.AutoSize = true;
      this.labelStartTime.Location = new System.Drawing.Point(74, 99);
      this.labelStartTime.Name = "labelStartTime";
      this.labelStartTime.Size = new System.Drawing.Size(34, 13);
      this.labelStartTime.TabIndex = 8;
      this.labelStartTime.Text = "00:00";
      // 
      // labelCorrection
      // 
      this.labelCorrection.AutoSize = true;
      this.labelCorrection.Location = new System.Drawing.Point(74, 117);
      this.labelCorrection.Name = "labelCorrection";
      this.labelCorrection.Size = new System.Drawing.Size(34, 13);
      this.labelCorrection.TabIndex = 9;
      this.labelCorrection.Text = "00:00";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeNowToolStripMenuItem,
            this.toolStripTextBox1});
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
      this.toolStripMenuItem1.Text = "Set Start Time ";
      // 
      // timeNowToolStripMenuItem
      // 
      this.timeNowToolStripMenuItem.Name = "timeNowToolStripMenuItem";
      this.timeNowToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
      this.timeNowToolStripMenuItem.Text = "Time Now";
      this.timeNowToolStripMenuItem.Click += new System.EventHandler(this.timeNowToolStripMenuItem_Click);
      // 
      // toolStripTextBox1
      // 
      this.toolStripTextBox1.Name = "toolStripTextBox1";
      this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
      this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
      // 
      // WorkTimeGui
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(117, 179);
      this.ControlBox = false;
      this.Controls.Add(this.labelCorrection);
      this.Controls.Add(this.labelStartTime);
      this.Controls.Add(this.labelDate);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.labelLunchBreak);
      this.Controls.Add(this.labelCoffeeBreak);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.labelWorkingTime);
      this.Controls.Add(this.label1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "WorkTimeGui";
      this.ShowInTaskbar = false;
      this.Deactivate += new System.EventHandler(this.WorkTimeGui_Deactivate);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkTimeGui_FormClosing);
      this.DoubleClick += new System.EventHandler(this.WorkTimeGui_Click);
      this.contextMenuStrip_Notification.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelWorkingTime;
    private System.Windows.Forms.Timer GuiUpdateTimer;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label labelCoffeeBreak;
    private System.Windows.Forms.Label labelLunchBreak;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Notification;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
    private System.Windows.Forms.Label labelDate;
    private System.Windows.Forms.Label labelStartTime;
    private System.Windows.Forms.Label labelCorrection;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_About;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem timeNowToolStripMenuItem;
    private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
  }
}

