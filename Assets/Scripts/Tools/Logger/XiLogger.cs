using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Xi.Tools
{
    public static class XiLogger
    {
        public static void Log(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.Log($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}");

        public static void Log(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.Log($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}", obj);

        public static void LogError(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}");

        public static void LogError(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}", obj);

        public static void LogWarning(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogWarning($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}");

        public static void LogWarning(string message, Object obj, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1)
            => Debug.LogWarning($"[{Path.GetFileNameWithoutExtension(filePath)}] <{methodName}> ({lineNumber}) ===> {message}", obj);

        public static void LogException(Exception exception)
            => Debug.LogException(exception);

        public static void LogException(Exception exception, Object obj)
            => Debug.LogException(exception, obj);
    }
}
