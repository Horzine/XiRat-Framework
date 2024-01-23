using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Xi.Tools
{
    public class AdvancedLoggerTool : IDisposable
    {
        private enum LogLevel
        {
            Info,
            Warning,
            Error,
            Exception,
        }
        private readonly string _logFolderPath;
        private readonly string _logFilePath;
        private readonly string _warningErrorLogFilePath;
        private readonly Queue<LogData> _logQueue = new();
        private readonly Queue<LogData> _warningErrorLogQueue = new();
        private bool _isWriting = false;
        private bool _isWritingWarningError = false;
        private const int kMaxLogFileSizeInMB = 25;
        private const string kLogFileName = "log_all.txt";
        private const string kDetailFileName = "warning_error_exception.txt";

        public AdvancedLoggerTool()
        {
            _logFolderPath = LoggerUtils.GetLogFolderPath();
            _logFilePath = LoggerUtils.GetLogFilePath(kLogFileName);
            _warningErrorLogFilePath = LoggerUtils.GetLogFilePath(kDetailFileName);

            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }

            Application.logMessageReceivedThreaded += HandleLog;
            Application.quitting += OnApplicationQuit;

            ThreadPool.QueueUserWorkItem(state =>
            {
                while (true)
                {
                    if (_logQueue.Count > 0 && !_isWriting)
                    {
                        _isWriting = true;
                        var logData = _logQueue.Dequeue();
                        WriteLogToFile(logData, _logFilePath);
                        _isWriting = false;
                    }

                    if (_warningErrorLogQueue.Count > 0 && !_isWritingWarningError)
                    {
                        _isWritingWarningError = true;
                        var logData = _warningErrorLogQueue.Dequeue();
                        WriteLogToFile(logData, _warningErrorLogFilePath);
                        _isWritingWarningError = false;
                    }

                    Thread.Sleep(20);
                }
            });
        }

        private void WriteLogToFile(LogData logData, string filePath)
        {
            try
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine($"{logData.LogTime:HH:mm:ss.fff} [{logData.Level}] {logData.Message}");

                    if (!string.IsNullOrEmpty(logData.StackTrace))
                    {
                        writer.WriteLine(logData.StackTrace);
                    }
                }

                if (new FileInfo(filePath).Length > kMaxLogFileSizeInMB * 1024 * 1024)
                {
                    string archiveFilePath = filePath.Replace(".txt", $"_archive_{LoggerUtils.GetTimeNowName()}.txt");
                    File.Copy(filePath, archiveFilePath, true);
                    File.WriteAllText(filePath, string.Empty);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to write log to file: " + e.Message);
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            var level = LogLevel.Info;

            switch (type)
            {
                case LogType.Warning:
                    level = LogLevel.Warning;
                    _warningErrorLogQueue.Enqueue(new LogData(logString, level, stackTrace));
                    break;
                case LogType.Error:
                    level = LogLevel.Error;
                    _warningErrorLogQueue.Enqueue(new LogData(logString, level, stackTrace));
                    break;
                case LogType.Exception:
                    level = LogLevel.Exception;
                    _warningErrorLogQueue.Enqueue(new LogData(logString, level, stackTrace));
                    break;
                default:
                    break;
            }
            _logQueue.Enqueue(new LogData(logString, level));
        }

        private void OnApplicationQuit()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
            Application.quitting -= OnApplicationQuit;

            while (_isWriting || _isWritingWarningError)
            {
                Thread.Sleep(10);
            }

            Dispose();
        }

        public void Dispose()
        {
            _logQueue.Clear();
            _warningErrorLogQueue.Clear();
        }

        private struct LogData
        {
            public string Message;
            public LogLevel Level;
            public DateTime LogTime;
            public string StackTrace;

            public LogData(string message, LogLevel level, string stackTrace = "")
            {
                Message = message;
                Level = level;
                LogTime = DateTime.Now;
                StackTrace = stackTrace;
            }
        }

        private static class LoggerUtils
        {
            private const string kFolderName = "RuntimeLogs";

            public static string GetLogFilePath(string fileName) => Path.Combine(GetLogFolderPath(), fileName);

            public static string GetLogFolderPath()
            {
                string logFolderPath = string.Empty;

#if UNITY_EDITOR
                logFolderPath = Path.Combine(Application.dataPath, $"../{kFolderName}/{GetTimeNowName()}");
#elif UNITY_STANDALONE_WIN
                logFolderPath = Path.Combine(Application.dataPath, $"../{kFolderName}/{GetTimeNowName()}");
#else
                logFolderPath = Path.Combine(Application.persistentDataPath, kFolderName, GetTimeNowName());
#endif
                return logFolderPath;
            }

            public static string GetTimeNowName() => DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }
    }
}
