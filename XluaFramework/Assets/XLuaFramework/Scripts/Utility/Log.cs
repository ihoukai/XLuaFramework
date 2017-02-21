
using System;
using UnityEngine;

namespace XLuaFramework
{
    public class Log
    {
        public const int DEBUG = 1;         
        public const int INFO = 2;          
        public const int WARN = 3;
        public const int ERROR = 4;
        public const int NONE = 5;

        public static void Debug(string msg)
        {
            if (AppConst.LogLevel <= Log.DEBUG) {
                UnityEngine.Debug.Log(msg);
            }
        }

        public static void Info(string msg)
        {
            if (AppConst.LogLevel <= Log.INFO)
            {
                UnityEngine.Debug.Log(msg);
            }
        }

        public static void Warn(string msg)
        {
            if (AppConst.LogLevel <= Log.WARN)
            {
                UnityEngine.Debug.LogWarning(msg);
            }
        }

        public static void Error(string msg)
        {
            if (AppConst.LogLevel <= Log.ERROR)
            {
                UnityEngine.Debug.LogError(msg);
            }
        }

        public static void Error(Exception e)
        {
            if (AppConst.LogLevel <= Log.ERROR)
            {
                UnityEngine.Debug.LogError(e);
            }
        }
    }
}