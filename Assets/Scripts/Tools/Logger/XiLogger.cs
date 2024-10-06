using System;
using System.Collections.Generic;
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
        private const string kFrameCountNotInMainThread = "[NotMain]";
        private const string kNoSetupMainThread = "[Null]";
        private static readonly Dictionary<string, string> _fileNameCache = new();
        private static Thread _mainThread;
        private static bool IsMainThread => _mainThread == Thread.CurrentThread;

        public static void SetupMainThread(Thread mainThread) => _mainThread = mainThread;

        public static void CallMark([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.Log(HandleMessage(kCallMarkStr, filePath, methodName, lineNumber));

        public static void Log(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.Log(HandleMessage(message, filePath, methodName, lineNumber));

        public static void Log(string message, Object context, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.Log(HandleMessage(message, filePath, methodName, lineNumber), IsMainThread ? context : null);

        public static void LogError(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogError(HandleMessage(message, filePath, methodName, lineNumber));

        public static void LogError(string message, Object context, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogError(HandleMessage(message, filePath, methodName, lineNumber), IsMainThread ? context : null);

        public static void LogWarning(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogWarning(HandleMessage(message, filePath, methodName, lineNumber));

        public static void LogWarning(string message, Object context, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogWarning(HandleMessage(message, filePath, methodName, lineNumber), IsMainThread ? context : null);

        public static void LogException(Exception exception)
            => Debug.LogException(exception);

        public static void LogException(Exception exception, Object obj)
            => Debug.LogException(exception, obj);

        private static string HandleMessage(string message, string filePath, string methodName, int lineNumber)
        {
            int curThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_fileNameCache.TryGetValue(filePath, out string fileName))
            {
                fileName = Path.GetFileNameWithoutExtension(filePath);
                fileName = fileName[(fileName.LastIndexOf('\\') + 1)..];
                _fileNameCache.Add(filePath, fileName);
            }

            string logMsg = $" [T:{curThreadId}] [{fileName}] <{methodName}> ({lineNumber}) ===> {message}";
            string frameCount = _mainThread == null
                ? kNoSetupMainThread
                : IsMainThread
                ? $"[F:{Time.frameCount}]"
                : kFrameCountNotInMainThread;

            return $"{frameCount}{logMsg}";
        }
    }
}
