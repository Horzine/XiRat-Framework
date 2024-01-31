using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Xi.Tools
{
    public static class XiLogger
    {
        private static int _unityMainThreadId;
        private const string kCallMarkStr = "****** CALL MARK ******";
        private const string kFrameCountNotInMainThread = "[NotMainThread]";

        public static void SetupByMainThread() => _unityMainThreadId = Thread.CurrentThread.ManagedThreadId;

        public static void CallMark([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(kCallMarkStr, null, LogType.Log, filePath, methodName, lineNumber);

        public static void Log(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, null, LogType.Log, filePath, methodName, lineNumber);

        public static void Log(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, obj, LogType.Log, filePath, methodName, lineNumber);

        public static void LogError(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, null, LogType.Error, filePath, methodName, lineNumber);

        public static void LogError(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, obj, LogType.Error, filePath, methodName, lineNumber);

        public static void LogWarning(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, null, LogType.Warning, filePath, methodName, lineNumber);

        public static void LogWarning(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, obj, LogType.Warning, filePath, methodName, lineNumber);

        public static void LogException(Exception exception)
            => Debug.LogException(exception);

        public static void LogException(Exception exception, Object obj)
            => Debug.LogException(exception, obj);

        private static void LogMessage(string message, Object context, LogType logType, string filePath, string methodName, int lineNumber)
        {
            int curThreadId = Thread.CurrentThread.ManagedThreadId;
            string logMsg = $" [T:{curThreadId}] [{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}";
            string frameCount = curThreadId == _unityMainThreadId ? $"[F:{Time.frameCount}]" : kFrameCountNotInMainThread;

            switch (logType)
            {
                case LogType.Error:
                    if (context != null)
                    {
                        Debug.LogError($"{frameCount}{logMsg}", context);
                    }
                    else
                    {
                        Debug.LogError($"{frameCount}{logMsg}");
                    }

                    break;
                case LogType.Warning:
                    if (context != null)
                    {
                        Debug.LogWarning($"{frameCount}{logMsg}", context);
                    }
                    else
                    {
                        Debug.LogWarning($"{frameCount}{logMsg}");
                    }

                    break;
                case LogType.Log:
                default:
                    if (context != null)
                    {
                        Debug.Log($"{frameCount}{logMsg}", context);
                    }
                    else
                    {
                        Debug.Log($"{frameCount}{logMsg}");
                    }

                    break;
            }
        }
    }
}
