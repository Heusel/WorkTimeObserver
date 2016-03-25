//#define DEBUGIT

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
    public int CoffeeBreakDurationMinutes;
    public int LunchBreakDurationMinutes;
    public int StartTimeOffsetMinutes;
    public int CoffeeTimeHour;
    public int LunchTimeHour;
    public int DailyWorkTime;
    public int DailyWorkLimit;
    public string logFileName;
    public Boolean logFileAddMonthYear;
    public Boolean checkStartTime;
  
    public TimeCalcSettings()
    {
      CoffeeBreakDurationMinutes =  15;
      CoffeeTimeHour      = 9;
      
      LunchBreakDurationMinutes = 30;
      LunchTimeHour      = 12;
      StartTimeOffsetMinutes = 10; 
      DailyWorkTime = 8;
      DailyWorkLimit     = 10;
      logFileName = @"WTO_Logging.txt";
      logFileAddMonthYear = true;
      checkStartTime = true;
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

    public TimeCalc()
    {
      init();
    }

    ~TimeCalc()
    {
      log();
    }

    private void log()
    {
      string filename;

      if (settings.logFileAddMonthYear)
      {
        filename = settings.logFileName.Substring(0, settings.logFileName.LastIndexOf('.'));
        filename += "_" + startTime.Year.ToString() + "_" + startTime.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
        filename += settings.logFileName.Substring(settings.logFileName.LastIndexOf('.'));
      }
      else
      {
        filename = settings.logFileName;
      }

      using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true))
      {
        // Add some text to the file.
        sw.WriteLine(getLogString());
      }
    }

    private void init()
    {
      string fileName;


      startTime = DateTime.Now;
   
      fileName = @"WTO_setting.xml";
      settings = TimeCalcSettings.Load(fileName);

      if (settings == null)
      {
        settings = new TimeCalcSettings();
        TimeCalcSettings.Save(fileName, settings);
      }


      startTime -= TimeSpan.FromMinutes(settings.StartTimeOffsetMinutes);

      if (settings.checkStartTime)
      {
        DateTime ob = new DateTime();

        fileName = @"WTO_startTime.tmp";

        if (File.Exists(fileName))
        {
          Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

          // Now create a binary formatter (it is up to you to ensure that code uses the same formatter for serialization
          // and deserialization
          System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

          // Deserialize the object and use it. Note: Constructors will not be called
          ob = (DateTime)formatter.Deserialize(stream);
          stream.Close();
        }

        if (ob.Date != startTime.Date)
        {
          Stream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
          System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

          formatter.Serialize(stream, startTime);
          stream.Close();
        }
        else
        {
          startTime = ob;
        }
      }


      
      CorrectionTime = new TimeSpan(0, 0, 0);

      running = true;
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


    private void update()
    {
      if (startTime.Date != DateTime.Now.Date)
      {
        log();
        init();
      }


      DiffTime = DateTime.Now - startTime;


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
    }


    public string getWorkTime()
    {
      update();
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

    public string getLogString()
    {
      string str;
      str = startTime.ToShortDateString();
      str += " " + DateTime.Now.ToShortTimeString();
      str += "; startTime: " + startTime.ToShortTimeString();
      str += "; CorrectionTime[minutes]: " + CorrectionTime.Minutes;
      str += "; WorkTime: " + getWorkTime();
      return str;
    }
  }
}
