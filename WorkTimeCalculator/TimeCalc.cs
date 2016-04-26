
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

    public int DailyWorkingTime;
    public int DailyWorkingTimeLimit;    
     
    public int CoffeeTimeHour; 
    public int CoffeeBreakDurationMinutes;

    public int LunchTimeHour;
    public int LunchBreakDurationMinutes;

    public Boolean EnableWorkingTimeStartStop;
    public int WorkingTimeStartHour;
    public int WorkingTimeStopHour;

    public Boolean EnableLogFile;
    public string LogFileName;
    public Boolean LogFileAddMonthYear;
    public char LogFileSepChar;

    
    public TimeCalcSettings()
    {
      StartTimeOffsetMinutes      = 10; 
      
      DailyWorkingTime            = 8;
      DailyWorkingTimeLimit       = 10;

      CoffeeTimeHour = 9;
      CoffeeBreakDurationMinutes  = 15;
      
      LunchTimeHour               = 12;
      LunchBreakDurationMinutes   = 30;

      EnableWorkingTimeStartStop  = true;
      WorkingTimeStartHour        = 6;
      WorkingTimeStopHour         = 21;
   
      EnableLogFile               = false;
      LogFileName                 = @"WTO_Logging.csv";
      LogFileAddMonthYear         = true;
      LogFileSepChar              = '\t';
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
    public Boolean enableLogging;

    public TimeCalc()
    {
      init();
    }


    public void log()
    {
      if (enableLogging)
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
          sw.WriteLine(getLogString());
          sw.Close();
        }
      }
    }
    private void init()
    {
      init(DateTime.Now, false, true);    
    }

    private void init(DateTime startTimePara, Boolean ignoreTempFile, Boolean useStartTimeOffset)
    {
      string fileName;

   
      fileName = @"WTO_setting.xml";
      settings = TimeCalcSettings.Load(fileName);

      if (settings == null)
      {
        settings = new TimeCalcSettings();
        TimeCalcSettings.Save(fileName, settings);
      }
      
      enableLogging = settings.EnableLogFile;
   
      if (settings.LogFileName.LastIndexOf('.') < 0)
      {
        settings.LogFileName += ".csv";
      }


      if (settings.LogFileAddMonthYear)
      {
        logFileName =  settings.LogFileName.Substring(0, settings.LogFileName.LastIndexOf('.'));
        logFileName += "_" + startTimePara.Year.ToString() + "_" + startTimePara.ToString("MM", System.Globalization.CultureInfo.InvariantCulture);
        logFileName += settings.LogFileName.Substring(settings.LogFileName.LastIndexOf('.'));
      }
      else
      {
        logFileName = settings.LogFileName;
      }

      startTime = startTimePara;

      if (useStartTimeOffset)
      {
        startTime -= TimeSpan.FromMinutes(settings.StartTimeOffsetMinutes);
      }

      CorrectionTime = new TimeSpan();


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
      running = true;
    }

    public void SetStartTime(DateTime setTime, Boolean useStartTimeOffset)
    {
      init(setTime, true, useStartTimeOffset); 
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
        if (DiffTime.Minutes >= settings.DailyWorkingTime)
      #else
        if (DiffTime.Hours >= settings.DailyWorkingTime)
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
        if (DiffTime.Minutes >= settings.DailyWorkingTimeLimit)
      #else
        if (DiffTime.Hours >= settings.DailyWorkingTimeLimit)
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
      if ( (startTime.Hour < settings.CoffeeTimeHour)
        && (DateTime.Now.Hour >= settings.CoffeeTimeHour) )
        return true;
      else
        return false;
    }

     
    public Boolean isLunchBreak()
    {  
      if ( (startTime.Hour < settings.LunchTimeHour)
        && (DateTime.Now.Hour >= settings.LunchTimeHour) )
        return true;
      else
        return false;
    }


    public Boolean update()
    {
      DateTime actualDateTime = DateTime.Now;

      if (settings.EnableWorkingTimeStartStop
        && ((actualDateTime.Hour < settings.WorkingTimeStartHour)
        || (actualDateTime.Hour >= settings.WorkingTimeStopHour)))
      {
        return false;      
      }

      if ( startTime.Date != actualDateTime.Date )
      {
        log();
        init(DateTime.Now, true, false);
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

      str =  "Log Timestamp".PadLeft(19);
      str += settings.LogFileSepChar + "Start Date".PadLeft(10);
      str += settings.LogFileSepChar + "Start Time".PadLeft(10);
      str += settings.LogFileSepChar + "Correction Time [minutes]".PadLeft(25);
      str += settings.LogFileSepChar + "Working Time".PadLeft(12);
      
      return str;
    }

    public string getLogString()
    {
      string str;
      
      str =  DateTime.Now.ToString().PadLeft(19);
      str += settings.LogFileSepChar + startTime.ToShortDateString().PadLeft(10); 
      str += settings.LogFileSepChar + startTime.ToShortTimeString().PadLeft(10);
      str += settings.LogFileSepChar + CorrectionTime.Minutes.ToString().PadLeft(25);
      str += settings.LogFileSepChar + getWorkTime().PadLeft(12);
      
      return str;
    }
  }
}
