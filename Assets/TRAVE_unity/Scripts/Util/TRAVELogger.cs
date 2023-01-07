using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TRAVE_unity
{
    
    public class TRAVELogger
    {
        private static readonly TRAVELogger instance = new TRAVELogger();

        public string latestLogString = "";
        public bool printMessage;

        public enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
            None
        }

        private Dictionary< LogLevel, string > _logLevel2Color = new Dictionary< LogLevel, string>()
        {
            { LogLevel.Debug, "cyan" },
            { LogLevel.Info, "lime" },
            { LogLevel.Warn, "yellow" },
            { LogLevel.Error, "orange" },
            { LogLevel.Fatal, "red" },
            { LogLevel.None, "white" }
        };

        //以下公開プロパティ

        //クラスインスタンスアクセス用
        public static TRAVELogger GetInstance
        {
            get {return instance;}
        }

        //ログ出力
        public void writeLog(string logMessage, LogLevel logLevel = LogLevel.Debug)
        {
            if(printMessage)
            {
                //出力文字列の生成
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