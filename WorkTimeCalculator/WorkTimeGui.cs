using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsWorkTimeApplication
{
  public partial class WorkTimeGui : Form
  {
    TimeCalc WorkTime;
    Boolean bSmall;
    enum ShowNotification {not,worktime, Alarmtime} ;
    ShowNotification showNotification;
    Boolean wasShownNotification;
    
    
    public WorkTimeGui()
    { 
      showNotification = ShowNotification.not;
      WorkTime = new TimeCalc();
        
      InitializeComponent();
      SetBalloonTip();
      bSmall = false;
      WorkTimeGui_Deactivate(null, null);
      wasShownNotification = false;
      GuiUpdateTimer_Tick(null, null);
      GuiUpdateTimer.Enabled = true;
    }

    private void ShowNotificationMsg(ShowNotification notify, string str)
    {
      if (showNotification != notify)
      {
        notifyIcon1.BalloonTipText = str;
        notifyIcon1.Visible = true;
        notifyIcon1.ShowBalloonTip(30000);

        showNotification = notify;
        notifyIcon1_DoubleClick(null,null);
      }    
    }

    private void GuiUpdateTimer_Tick(object sender, EventArgs e)
    {
      WorkTime.update();
      label2.Text = WorkTime.getWorkTime();

      if (WorkTime.isDailyWorkTotalLimit())
      {
        label2.ForeColor = Color.Red;
        notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
        ShowNotificationMsg( ShowNotification.Alarmtime, "ALARM: " + WorkTime.getWorkTime());
      }
      else if (WorkTime.isDailyWorkDone())
      {
        label2.ForeColor = Color.Green;
        notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
        ShowNotificationMsg(ShowNotification.worktime, "Work Time: " + WorkTime.getWorkTime());
      }
      else
      {
        label2.ForeColor = SystemColors.ControlText;
      }

     
      notifyIcon1.BalloonTipText = "worktim: " + WorkTime.getWorkTime();
      notifyIcon1.Text = "worktime: " + WorkTime.getWorkTime();

      label3.Text = "Date: " + WorkTime.getStartDate();
      label7.Text = "Start time: " + WorkTime.getStartTime.ToShortTimeString();
      label4.Text = "Correction: " + WorkTime.getCorrectionTime();

      label5.Enabled = WorkTime.isCoffeeBreak();
      label6.Enabled = WorkTime.isLunchBreak();
    }

    private void WorkTimeGui_Click(object sender, EventArgs e)
    {
      bSmall = !bSmall;

      if (bSmall)
        this.Height = 100;
      else
        this.Height = 210;
    }

    private void SetBalloonTip()
    {
      notifyIcon1.Icon = this.Icon;
      notifyIcon1.BalloonTipTitle = "W.T.O.";
      notifyIcon1.BalloonTipText = "worktime: " + WorkTime.getWorkTime();
      notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
      this.Click += new EventHandler(Form1_Click);
    }

    void Form1_Click(object sender, EventArgs e)
    {
    
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    private void notifyIcon1_DoubleClick(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
      
    }

    private void WorkTimeGui_Deactivate(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
      this.Hide();
    }

    private void WorkTimeGui_FormClosing(object sender, FormClosingEventArgs e)
    {
      GuiUpdateTimer.Enabled = false;
      WorkTime.log();
    }
  }
}
