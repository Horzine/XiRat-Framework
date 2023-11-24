using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Xi.Tools
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
    public class AdvancedLogger : IDisposable
    {
        private readonly string _logFilePath;
        private readonly Queue<LogData> _logQueue = new();
        private bool _isWriting = false;
        private const int kMaxLogQueueSize = 100;
        private const int kMaxLogFileSizeInMB = 5;

        public AdvancedLogger()
        {
            // 创建日志文件路径
            _logFilePath = LogUtils.GetLogFilePath("log.txt");

            // 绑定 Application 的生命周期事件
            Application.logMessageReceivedThreaded += HandleLog;
            Application.quitting += OnApplicationQuit;

            // 开启一个后台线程来写入日志文件
            ThreadPool.QueueUserWorkItem(state =>
            {
                while (true)
                {
                    if (_logQueue.Count > 0 && !_isWriting)
                    {
                        _isWriting = true;
                        var logData = _logQueue.Dequeue();
                        WriteLogToFile(logData);
                        _isWriting = false;
                    }

                    Thread.Sleep(20); // 控制写入频率，避免过于频繁
                }
            });
        }

        private void WriteLogToFile(LogData logData)
        {
            try
            {
                using (var writer = File.AppendText(_logFilePath))
                {
                    writer.WriteLine($"{logData.LogTime} [{logData.Level}] {logData.Message}");
                }

                if (new FileInfo(_logFilePath).Length > kMaxLogFileSizeInMB * 1024 * 1024)
                {
                    // 如果日志文件大小超过限制，备份日志文件并清空
                    string backupFilePath = Path.Combine(Application.dataPath, "log_backup.txt");
                    File.Copy(_logFilePath, backupFilePath, true);
                    File.WriteAllText(_logFilePath, string.Empty);
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
                    break;
                case LogType.Error:
                case LogType.Exception:
                    level = LogLevel.Error;
                    break;
            }

            if (_logQueue.Count < kMaxLogQueueSize)
            {
                _logQueue.Enqueue(new LogData(logString, level));
            }
        }

        private void OnApplicationQuit()
        {
            // 解绑 Application 的生命周期事件
            Application.logMessageReceivedThreaded -= HandleLog;
            Application.quitting -= OnApplicationQuit;

            // 等待后台线程写入完毕
            while (_isWriting)
            {
                Thread.Sleep(10);
            }

            // 释放资源
            Dispose();
        }

        public void Dispose() => _logQueue.Clear();   // 清空日志队列
    }

    public struct LogData
    {
        public string Message;
        public LogLevel Level;
        public DateTime LogTime;

        public LogData(string message, LogLevel level)
        {
            Message = message;
            Level = level;
            LogTime = DateTime.Now;
        }
    }

    public static class LogUtils
    {
        private const string kFolderName = "RuntimeLogs";
        public static string GetLogFilePath(string fileName)
        {
            string logFolderPath = GetLogFolderPath();
            return Path.Combine(logFolderPath, fileName);
        }
        private static string GetLogFolderPath()
        {
            string logFolderPath = string.Empty;

#if UNITY_EDITOR
            logFolderPath = Path.Combine(Application.dataPath, $"../{kFolderName}");
#elif UNITY_STANDALONE_WIN
            logFolderPath = Path.Combine(Application.dataPath, $"../{kFolderName}");
#else
            logFolderPath = Path.Combine(Application.persistentDataPath, kFolderName);
#endif

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            return logFolderPath;
        }
    }
}
