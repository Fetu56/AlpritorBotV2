using System;
using System.IO;

namespace AlpritorBotV2.LogModule
{
    public static class SimpleLogger
    {
        public const string LogDirectory = "Log";
        public const string LogFile = "Log.txt";
        private static string PathToLog { get { return LogDirectory + '\\' + LogFile; } }
        public static void WriteToLog(string message, string actor="None")
        {
            CheckIsFilesCreated();
            File.AppendAllText(PathToLog, $"[{DateTime.Now}] ({actor}): {message}\n");
        }
        private static void CheckIsFilesCreated()
        {
            if(!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
            if(!File.Exists(PathToLog))
            {
                File.Create(PathToLog).Close();
            }
        }
    }
}
