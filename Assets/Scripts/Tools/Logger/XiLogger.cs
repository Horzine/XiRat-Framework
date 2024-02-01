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
        private const string kCallMarkStr = "****** CALL MARK ******";
        private const string kFrameCountNotInMainThread = "[NotMainThread]";
        private static Thread _mainThread;
        private static bool IsMainThread => _mainThread == Thread.CurrentThread;

        public static void SetupMainThread(Thread mainThread) => _mainThread = mainThread;

        public static void CallMark([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(kCallMarkStr, LogType.Log, filePath, methodName, lineNumber, null);

        public static void Log(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Log, filePath, methodName, lineNumber, null);

        public static void Log(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Log, filePath, methodName, lineNumber, obj);

        public static void LogError(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Error, filePath, methodName, lineNumber, null);

        public static void LogError(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Error, filePath, methodName, lineNumber, obj);

        public static void LogWarning(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Warning, filePath, methodName, lineNumber, null);

        public static void LogWarning(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => LogMessage(message, LogType.Warning, filePath, methodName, lineNumber, obj);

        public static void LogException(Exception exception)
            => Debug.LogException(exception);

        public static void LogException(Exception exception, Object obj)
            => Debug.LogException(exception, obj);

        private static void LogMessage(string message, LogType logType, string filePath, string methodName, int lineNumber, Object context = null)
        {
            int curThreadId = Thread.CurrentThread.ManagedThreadId;
            string logMsg = $" [T:{curThreadId}] [{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}";
            string frameCount = IsMainThread ? $"[F:{Time.frameCount}]" : kFrameCountNotInMainThread;

            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError($"{frameCount}{logMsg}", IsMainThread ? context : null);
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"{frameCount}{logMsg}", IsMainThread ? context : null);
                    break;
                case LogType.Log:
                default:
                    Debug.Log($"{frameCount}{logMsg}", IsMainThread ? context : null);
                    break;
            }
        }
    }
}
