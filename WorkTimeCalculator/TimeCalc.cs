
#if DEBUG
  #define DEBUGIT
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;



namespace WindowsFormsWorkTimeApplication
{
  [Serializable()]
  public class TimeCalcSettings
  {
    public int StartTimeOffsetMinutes;

    public int DailyWorkTime;
    public int DailyWorkLimit;    
     
    public int CoffeeTimeHour; 
    public int CoffeeBreakDurationMinutes;

    public int LunchTimeHour;
    public int LunchBreakDurationMinutes;

    public Boolean EnableWorktimeStartStop;
    public int WorktimeStartHour;
    public int WorktimeStopHour;

    public string logFileName;
    public Boolean logFileAddMonthYear;
    public char logFileSepChar;

    public Boolean checkStartTime;
    
    public TimeCalcSettings()
    {
      StartTimeOffsetMinutes      = 10; 
      
      DailyWorkTime               = 8;
      DailyWorkLimit              = 10;

      CoffeeTimeHour = 9;
      CoffeeBreakDurationMinutes  = 15;
      
      LunchTimeHour               = 12;
      LunchBreakDurationMinutes   = 30;

      EnableWorktimeStartStop     = true;

      WorktimeStartHour           = 6;
      WorktimeStopHour            = 24;
   
      logFileName                 = @"WTO_Logging.csv";
      logFileAddMonthYear         = true;
      logFileSepChar              = ';';

      checkStartTime              = true;
    }

    public static TimeCalcSettings Load(string fileName)
    {
      TimeCalcSettings retVal = null;

      XmlSerializer SerializerObj = new XmlSerializer(typeof(TimeCalcSettings));

      if (File.Exists(fileName))
      {
        FileStream ReadFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        // Load the object saved above by using the Deserialize function
        retVal = new TimeCalcSettings();
        retVal = (TimeCalcSettings)SerializerObj.Deserialize(ReadFileStream);

        // Cleanup
        ReadFileStream.Close();
      }
      return retVal;
    }

    public static void Save(string fileName, TimeCalcSettings settings)
    {
      XmlSerializer SerializerObj = new XmlSerializer(typeof(TimeCalcSettings));
      // Create a new file stream to write the serialized object to a file
      TextWriter WriteFileStream = new StreamWriter(fileName);
      SerializerObj.Serialize(WriteFileStream, settings);

      // Cleanup
      WriteFileStream.Close();
    }
  }

  class TimeCalc
  {
    private DateTime startTime;
    private TimeSpan DiffTime;
    private TimeSpan CorrectionTime;
    private Boolean running;
    public TimeCalcSettings settings;
    private string logFileName;
    public Boolean ignoreLogging;

    public TimeCalc()
    {
      init(DateTime.Now);
    }


    public void log()
    {
      if (!ignoreLogging)
      {
        bool addHeader = false;

        if (!File.Exists(logFileName))
        {
          addHeader = true;
        }

        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFileName, true))
        {
          if (addHeader)
          {
            sw.WriteLine(getLogHeaderString());
          }
          // Add some text to the file.
          sw.WriteLine(getLogString());
          sw.Close();
        }
      }
    }

    private void init(DateTime startTimePara, Boolean ignoreTempFile = false)
    {
      string fileName;

      ignoreLogging = false;
   
      fileName = @"WTO_setting.xml";
      settings = TimeCalcSettings.Load(fileName);

      if (settings == null)
      {
        settings = new TimeCalcSettings();
        TimeCalcSettings.Save(fileName, settings);
      }

      if (settings.logFileName.LastIndexOf('.') < 0)
      {
        settings.logFileName += ".csv";
      }


      if (settings.logFileAddMonthYear)
      {
        logFileName = settings.logFileName.Substring(0, settings.logFileName.LastIndexOf('.'));
        logFileName += "_" + startTimePara.Year.ToString() + "_" + startTimePara.ToString("MM", System.Globalization.CultureInfo.InvariantCulture);
        logFileName += settings.logFileName.Substring(settings.logFileName.LastIndexOf('.'));
      }
      else
      {
        logFileName = settings.logFileName;
      }

      startTime = startTimePara - TimeSpan.FromMinutes(settings.StartTimeOffsetMinutes);
      CorrectionTime = new TimeSpan();


      if (settings.checkStartTime)
      {
        DateTime ob = new DateTime();

        fileName = @"WTO_startTime.xml";

        if (File.Exists(fileName) & !ignoreTempFile)
        {

          XmlSerializer SerializerObj = new XmlSerializer(typeof(DateTime));
          FileStream ReadFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

          // Load the object saved above by using the Deserialize function

          startTime = (DateTime)SerializerObj.Deserialize(ReadFileStream);

          // Cleanup
          ReadFileStream.Close();
        }

        if (ob.Date != startTime.Date)
        {
          XmlSerializer SerializerObj = new XmlSerializer(typeof(DateTime));
          // Create a new file stream to write the serialized object to a file
          TextWriter WriteFileStream = new StreamWriter(fileName);
          SerializerObj.Serialize(WriteFileStream, startTime);

          // Cleanup
          WriteFileStream.Close();
        }
        else
        {
          startTime = ob;
        }
      }
      running = true;
    }

    public void SetStartTime(DateTime setTime)
    {
      init(setTime, true); 
    }

    public DateTime getStartTime
    {
      get { return startTime; }
    }


    public Boolean isRunning
    {
      get { return running; }
    }

    public Boolean isDailyWorkDone()
    {
      #if (DEBUGIT)
        if (DiffTime.Minutes >= settings.DailyWorkTime)
      #else
        if (DiffTime.Hours >= settings.DailyWorkTime)
      #endif
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public Boolean isDailyWorkTotalLimit()
    {
      #if (DEBUGIT)
        if (DiffTime.Minutes >= settings.DailyWorkLimit)
      #else
        if (DiffTime.Hours >= settings.DailyWorkLimit)
      #endif
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public Boolean isCoffeeBreak()
    {  
      if ( ( startTime.Hour < settings.CoffeeTimeHour )
        && ( DateTime.Now.Hour >= settings.CoffeeTimeHour ) )
        return true;
      else
        return false;
    }

     
    public Boolean isLunchBreak()
    {  
      if ( ( startTime.Hour < settings.LunchTimeHour )
        && ( DateTime.Now.Hour >= settings.LunchTimeHour ) )
        return true;
      else
        return false;
    }


    public Boolean update()
    {
      DateTime actualDateTime = DateTime.Now;

      if (settings.EnableWorktimeStartStop & ((actualDateTime.Hour < settings.WorktimeStartHour)
          || (actualDateTime.Hour > settings.WorktimeStopHour)))
      {
        return false;      
      }

      if (startTime.Date != actualDateTime.Date)
      {
        log();
        init(DateTime.Now, true);
      }
      
      DiffTime = actualDateTime - startTime;
      
      if (isCoffeeBreak()
        && (CorrectionTime.Minutes < settings.CoffeeBreakDurationMinutes))
      {
        CorrectionTime += TimeSpan.FromMinutes( settings.CoffeeBreakDurationMinutes);
      }
      
      if (isLunchBreak()
        && (CorrectionTime.Minutes < settings.LunchBreakDurationMinutes))
      {
        CorrectionTime += TimeSpan.FromMinutes( settings.LunchBreakDurationMinutes);
      }

      DiffTime -= CorrectionTime;
        
      return true;  
    }


    public string getWorkTime()
    {
      string str = DiffTime.ToString();
      str = str.Substring(0, str.LastIndexOf(':'));
      return str;
    }

    public string getStartDate()
    {
      return startTime.ToShortDateString();
    }

    public string getCorrectionTime()
    {
      string str = CorrectionTime.ToString();
      str = str.Substring(0, str.LastIndexOf(':'));
      return str;
    }

    public string getLogHeaderString()
    {
      string str;
      str = "Log Timestamp".PadLeft(19)
          + settings.logFileSepChar + "Start Date".PadLeft(11)
          + settings.logFileSepChar + "Start Time".PadLeft(11)
          + settings.logFileSepChar + "Correction Time [minutes]".PadLeft(26)
          + settings.logFileSepChar + "Work Time".PadLeft(11);
      return str;
    }

    public string getLogString()
    {
      string str;
      
      str =  DateTime.Now.ToString().PadLeft(19);
      str += settings.logFileSepChar +startTime.ToShortDateString().PadLeft(11); 
      str += settings.logFileSepChar + startTime.ToShortTimeString().PadLeft(11);
      str += settings.logFileSepChar + CorrectionTime.Minutes.ToString().PadLeft(26);
      str += settings.logFileSepChar + getWorkTime().PadLeft(11);
      return str;
    }
  }
}
