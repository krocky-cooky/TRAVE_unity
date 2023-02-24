using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TRAVE_unity
{
    /// <summary>
    /// Debug printing class only for TRAVE.
    /// </summary>
    public class TRAVELogger
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static readonly TRAVELogger instance = new TRAVELogger();

        public string latestLogString = "";
        public bool printMessage;

        /// <summary>
        /// Log level of messages.
        /// </summary>
        public enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
            None
        }

        /// <summary>
        /// Correspondance between Loglevel and color of the printed string.
        /// </summary>
        private Dictionary< LogLevel, string > _logLevel2Color = new Dictionary< LogLevel, string>()
        {
            { LogLevel.Debug, "cyan" },
            { LogLevel.Info, "lime" },
            { LogLevel.Warn, "yellow" },
            { LogLevel.Error, "orange" },
            { LogLevel.Fatal, "red" },
            { LogLevel.None, "white" }
        };

        /// <summary>
        /// Get singleton instance of TRAVELogger.
        /// Instances have to be initialized with this property.
        /// </summary>
        /// <value>Singleton instance.</value>
        public static TRAVELogger GetInstance
        {
            get {return instance;}
        }

        /// <summary>
        /// Printing log to inspector.
        /// By specifying loglevel, color of the printed string can be changed.
        /// </summary>
        /// <param name="logMessage">String value of log.</param>
        /// <param name="logLevel">Loglevel of request.</param>
        public void WriteLog(string logMessage, LogLevel logLevel = LogLevel.Debug)
        {
            if(printMessage)
            {
                string color = _logLevel2Color[logLevel];
                string logString = $"[<color=cyan>TRAVE</color>::<color={color}>{logLevel.ToString()}</color>] <color={color}>{logMessage}</color>";
                latestLogString = logString;
                if(logLevel == LogLevel.Warn)
                    Debug.LogWarning(logString);
                else if(logLevel == LogLevel.Error)
                    Debug.LogError(logString);
                else
                    Debug.Log(logString);
            }
        }
    }
}