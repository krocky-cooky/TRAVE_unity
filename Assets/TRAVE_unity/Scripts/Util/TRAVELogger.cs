using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TRAVE_unity
{
    
    public class TRAVELogger
    {
        private static readonly TRAVELogger instance = new TRAVELogger();

        public string latestLogString = "";

        public enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
            None
        }

        //以下公開プロパティ

        //クラスインスタンスアクセス用
        public static TRAVELogger GetInstance
        {
            get {return instance;}
        }

        //ログ出力
        public void writeLog(string logMessage, LogLevel logLevel)
        {
            //出力文字列の生成
            string logString = $"[TRAVE::{logLevel.ToString()}] {logMessage}";
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