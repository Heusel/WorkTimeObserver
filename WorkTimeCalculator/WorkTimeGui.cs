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
     
      SetBalloonTip();
      this.Text = this.ProductName;
      this.Icon = (Icon)global::WorkingTimeObserver.Properties.Resources.Worktime_Black;
      bSmall = false;
      GuiUpdateTimer_Tick(null, null);
      WorkTimeGui_Deactivate(null, null);
      GuiUpdateTimer.Enabled = true;
      SetGuiPos();
    }

    private void GuiUpdateTimer_Tick(object sender, EventArgs e)
    {
      if (WorkTime.update())
      {
        this.Enabled = true;
      }
      else
      {
        this.Enabled = false;
      }

      labelWorkingTime.Text = WorkTime.getWorkTime();

      if (WorkTime.isDailyWorkTotalLimit())
      {
        if (showNotification != ShowNotification.Alarmtime)
        {
          labelWorkingTime.ForeColor = Color.Red;
          notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
          notifyIcon.Icon = (Icon)global::WorkingTimeObserver.Properties.Resources.Worktime_Red;

          notifyIcon.BalloonTipText = "Working time is too high! " + WorkTime.getWorkTime();
          notifyIcon.Visible = true;
          notifyIcon.ShowBalloonTip(30);

          showNotification = ShowNotification.Alarmtime;
        }
      }
      else if (WorkTime.isDailyWorkDone() )
      {
        if ((showNotification != ShowNotification.worktime))
        {
          labelWorkingTime.ForeColor = Color.Green;
          notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
          notifyIcon.Icon = (Icon)global::WorkingTimeObserver.Properties.Resources.Worktime_Green;
          notifyIcon.BalloonTipText = "Daily working time limit reached! " + WorkTime.getWorkTime();
          notifyIcon.Visible = true;
          notifyIcon.ShowBalloonTip(30);

          showNotification = ShowNotification.worktime;
        }
      }
      else
      {
        if ((showNotification != ShowNotification.not))
        {
          labelWorkingTime.ForeColor = SystemColors.ControlText;
          notifyIcon.Icon = (Icon)global::WorkingTimeObserver.Properties.Resources.Worktime_Black;
        }
      }

     
      notifyIcon.BalloonTipText = "Working Time: " + WorkTime.getWorkTime();
      notifyIcon.Text           = notifyIcon.BalloonTipText;     

      labelDate.Text       = WorkTime.getStartDate();
      labelStartTime.Text  = WorkTime.getStartTime.ToShortTimeString();
      labelCorrection.Text = WorkTime.getCorrectionTime();

      labelCoffeeBreak.Enabled = WorkTime.isCoffeeBreak();
      labelLunchBreak.Enabled  = WorkTime.isLunchBreak();
    }

    private void SetGuiPos()
    {         
      Point newLocation  = new  Point(0,0);
      Rectangle myScreen = Screen.GetWorkingArea(this);
      Size myForm        = this.Size;

      newLocation.X = myScreen.Width  - myForm.Width;
      newLocation.Y = myScreen.Height - myForm.Height;

      this.SetDesktopLocation(newLocation.X, newLocation.Y);
    }

    private void MakeVisible()
    {
      SetGuiPos();
      this.Show();     
      this.Activate();
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
      notifyIcon.Icon            = (Icon)global::WorkingTimeObserver.Properties.Resources.Worktime_Black;
      notifyIcon.BalloonTipTitle = "W.T.O.";
      notifyIcon.BalloonTipText  = "Working time: " + WorkTime.getWorkTime();
      notifyIcon.BalloonTipIcon  = ToolTipIcon.Info;
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
      if (WorkTime.enableLogging == true)
      {
        DialogResult result = MessageBox.Show("Do you want to save the current working time into log file?",
                                               "Exit " + this.ProductName,
                                               MessageBoxButtons.YesNo);

        if (result == DialogResult.No)
        {
          WorkTime.enableLogging = false;
        }
      }
      Application.Exit();      
    }

    private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
    {
      this.MakeVisible();
    }

    private void toolStripMenuItem_About_Click(object sender, EventArgs e)
    {
      string copyright = string.Empty;
      string version   = string.Empty;
      string company   = string.Empty;
      
      Assembly currentAssem = typeof(WorkTimeGui).Assembly;
      
      // get copyright attribute
      object[] attribs = currentAssem.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);

      if(attribs.Length > 0)
      {
        copyright = ((AssemblyCopyrightAttribute)attribs[0]).Copyright;
      }

      // get company attribute
      attribs = currentAssem.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);

      if (attribs.Length > 0)
      {
        company = ((AssemblyCompanyAttribute)attribs[0]).Company;
      }

      // get the verion an generate special version string
      AssemblyName thisAssemName = currentAssem.GetName();
      Version ver = thisAssemName.Version;

      version = this.ProductName +  " V" + ver.Major + "." + ver.Minor + "." + ver.Build;

      MessageBox.Show(version + "\n"+ company +"\n" + copyright,
          "About" ,
          MessageBoxButtons.OK,
          MessageBoxIcon.Information,
          MessageBoxDefaultButton.Button1);
    }
    
    private void notifyIcon_Click(object sender, EventArgs e)
    {
      MouseEventArgs me = (MouseEventArgs)e;
      if(me.Button == MouseButtons.Left)
        this.MakeVisible();
    }
    
    private void timeNowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CheckAndSetStartTime(DateTime.Now);
    }
    
    private void toolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
    {
      toolStripTextBox1.Text = WorkTime.getStartTime.ToShortTimeString();
    }

    private void CheckAndSetStartTime(DateTime newStartTime)
    {
      DialogResult result = MessageBox.Show("Correcting start time (" + newStartTime.ToShortTimeString() + ") with offset (" + WorkTime.settings.StartTimeOffsetMinutes + " minutes)?", "New start time", MessageBoxButtons.YesNoCancel);

      if (result == DialogResult.Yes)
      {
        WorkTime.SetStartTime(newStartTime, true);
        GuiUpdateTimer_Tick(null, null);
      }
      
      if (result == DialogResult.No)
      {
        WorkTime.SetStartTime(newStartTime, false);
        GuiUpdateTimer_Tick(null, null);
      }
    }

    private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        try
        {
          DateTime dt = DateTime.ParseExact(toolStripTextBox1.Text.Trim(), "H:mm", System.Globalization.CultureInfo.InvariantCulture);

          contextMenuStrip_Notification.Close();

          CheckAndSetStartTime(dt);
        }
        catch (Exception ParseException)
        {
          //ignore exception so we only leave menu with correct time setting  
        }
      }
    }
  }
}
