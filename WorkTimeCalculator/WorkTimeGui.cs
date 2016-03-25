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
  
    public WorkTimeGui()
    {
      WorkTime = new TimeCalc(); 
      InitializeComponent();
      GuiUpdateTimer_Tick(null, null);
      bSmall = false;
    }

    private void GuiUpdateTimer_Tick(object sender, EventArgs e)
    {
      label2.Text = WorkTime.getWorkTime();

      if (WorkTime.isDailyWorkTotalLimit())
      {
        label2.ForeColor = Color.Red;
      }
      else if (WorkTime.isDailyWorkDone())
      {
        label2.ForeColor = Color.Green;
      }
      else
      {
        label2.ForeColor = SystemColors.ControlText;
      }

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
  }
}
