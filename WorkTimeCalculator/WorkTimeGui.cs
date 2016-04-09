using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace WindowsFormsWorkTimeApplication
{
  public partial class WorkTimeGui : Form
  {
    TimeCalc WorkTime;
    Boolean bSmall;
    enum ShowNotification {not,worktime, Alarmtime} ;
    ShowNotification showNotification;

    
    
    public WorkTimeGui()
    { 
      showNotification = ShowNotification.not;
      WorkTime = new TimeCalc();
        
      InitializeComponent();
      SetGuiPos();
      SetBalloonTip();
      bSmall = false;
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
        //notifyIcon1_DoubleClick(null,null);
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
        ShowNotificationMsg( ShowNotification.Alarmtime, "Working Time of " + WorkTime.getWorkTime() + " is too high!!");
      }
      else if (WorkTime.isDailyWorkDone())
      {
        labelWorkingTime.ForeColor = Color.Green;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
        notifyIcon.Icon = (Icon)global::WorkTimeObserver.Properties.Resources.Worktime_Green;
        ShowNotificationMsg(ShowNotification.worktime, "Work of the day is done: " + WorkTime.getWorkTime());
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

    private void SetGuiPos()
    {         
      Point newLocation = new  Point(0,0);
      Rectangle myScreen = Screen.GetWorkingArea(this);
      Size myForm = this.Size;
      newLocation.X = myScreen.Width - myForm.Width;
      newLocation.Y = myScreen.Height - myForm.Height;

      this.SetDesktopLocation(newLocation.X, newLocation.Y);
    }

    private void MakeVisible()
    {
      SetGuiPos();
      this.Show();    
    }

    private void WorkTimeGui_Click(object sender, EventArgs e)
    {
      bSmall = !bSmall;

      if (bSmall)
        this.Height = 120;
      else
        this.Height = 218;


      SetGuiPos();
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
      this.MakeVisible();
    }

    private void WorkTimeGui_Deactivate(object sender, EventArgs e)
    {
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
      this.MakeVisible();
    }

    private void toolStripMenuItem_About_Click(object sender, EventArgs e)
    {
      string copyright = string.Empty;
      string version = string.Empty;
      string company = string.Empty;
      
      Assembly currentAssem = typeof(WorkTimeGui).Assembly;
      object[] attribs = currentAssem.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);

      if(attribs.Length > 0)
      {
        copyright = ((AssemblyCopyrightAttribute)attribs[0]).Copyright;
      }

      attribs = currentAssem.GetCustomAttributes(typeof(AssemblyVersionAttribute), true);

      if (attribs.Length > 0)
      {
        version = ((AssemblyVersionAttribute)attribs[0]).Version;
      }

      attribs = currentAssem.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);

      if (attribs.Length > 0)
      {
        company = ((AssemblyCompanyAttribute)attribs[0]).Company;
      }

      AssemblyName thisAssemName = currentAssem.GetName();

      Version ver = thisAssemName.Version;

      version ="Version V" + ver.Major.ToString() + "." + ver.Minor + "\n\nBuild " + ver.Build + "\nRevision " + ver.Revision.ToString();

      MessageBox.Show(version + "\n\n"+ company +"\n" + copyright,
          "About " + this.ProductName,
          MessageBoxButtons.OK,
          MessageBoxIcon.Information,
          MessageBoxDefaultButton.Button1);
    }
  }
}
