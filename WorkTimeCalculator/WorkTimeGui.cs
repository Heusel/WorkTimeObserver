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
      wasShownNotification = false;
      GuiUpdateTimer_Tick(null, null);
      WorkTimeGui_Deactivate(null, null);
      GuiUpdateTimer.Enabled = true;
    }

    private void ShowNotificationMsg(ShowNotification notify, string str)
    {
      if (showNotification != notify)
      {
        notifyIcon.BalloonTipText = str;
        notifyIcon.Visible = true;
        notifyIcon.ShowBalloonTip(30000);

        showNotification = notify;
        notifyIcon1_DoubleClick(null,null);
      }    
    }

    private void GuiUpdateTimer_Tick(object sender, EventArgs e)
    {
      WorkTime.update();
      labelWorkingTime.Text = WorkTime.getWorkTime();

      if (WorkTime.isDailyWorkTotalLimit())
      {
        labelWorkingTime.ForeColor = Color.Red;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
        notifyIcon.Icon = (Icon)global::WorkTimeObserver.Properties.Resources.Worktime_Red;
        ShowNotificationMsg( ShowNotification.Alarmtime, "ALARM: Working Time of " + WorkTime.getWorkTime() + " is too high!!");
      }
      else if (WorkTime.isDailyWorkDone())
      {
        labelWorkingTime.ForeColor = Color.Green;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
        notifyIcon.Icon = (Icon)global::WorkTimeObserver.Properties.Resources.Worktime_Green;
        ShowNotificationMsg(ShowNotification.worktime, "Working Time: " + WorkTime.getWorkTime());
      }
      else
      {
        labelWorkingTime.ForeColor = SystemColors.ControlText;
      }

     
      notifyIcon.BalloonTipText = "Working Time: " + WorkTime.getWorkTime();
      notifyIcon.Text = "Working Time: " + WorkTime.getWorkTime();

      labelDate.Text       = WorkTime.getStartDate();
      labelStartTime.Text  = WorkTime.getStartTime.ToShortTimeString();
      labelCorrection.Text = WorkTime.getCorrectionTime();

      labelCoffeeBreak.Enabled = WorkTime.isCoffeeBreak();
      labelLunchBreak.Enabled = WorkTime.isLunchBreak();
    }

    private void WorkTimeGui_Click(object sender, EventArgs e)
    {
      bSmall = !bSmall;

      if (bSmall)
        this.Height = 120;
      else
        this.Height = 218;
    }

    private void SetBalloonTip()
    {
      notifyIcon.Icon = (Icon)global::WorkTimeObserver.Properties.Resources.Worktime_Black;
      notifyIcon.BalloonTipTitle = "W.T.O.";
      notifyIcon.BalloonTipText = "worktime: " + WorkTime.getWorkTime();
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
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

    private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
    {
      Application.Exit();      
    }

    private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

  }
}
