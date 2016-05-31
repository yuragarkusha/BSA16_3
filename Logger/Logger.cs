using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class Logger
    {
        private ILoggerStrategy _loggerStrategy;

        private static Logger _logger = null;
        public static Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }
            return _logger;
        }

        private Logger()
        {
            _loggerStrategy = new LoggerConsole();
        }

        public void ChangeStrategy(ILoggerStrategy strategy)
        {
            _loggerStrategy = strategy;
        }

        public void Info(string message)
        {
            _loggerStrategy.LoggerMessage("Info: ", message);
        }

        public void Debug(string message)
        {
            _loggerStrategy.LoggerMessage("Debug: ", message);
        }

        public void Warning(string message)
        {
            _loggerStrategy.LoggerMessage("Warning: ", message);
        }

        public void Error(string message)
        {
            _loggerStrategy.LoggerMessage("Error: ", message);
        }
    }

    public interface ILoggerStrategy
    {
        void LoggerMessage(string type, string message);
    }

    public class LoggerConsole : ILoggerStrategy
    {
        public void LoggerMessage(string type, string message)
        {
            Console.WriteLine(type, message);
        }
    }

    public class LoggerFile : ILoggerStrategy, IDisposable
    {
        private readonly StreamWriter _streamWriter;
        private bool _disposed;
        private bool v;

        public LoggerFile(string path)
        {
            _streamWriter = new StreamWriter(path);
            _disposed = false;
        }

        public LoggerFile(string path, bool v) : this(path)
        {
            this.v = v;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _streamWriter.Close();
                _disposed = true;
            }
        }

        public void LoggerMessage(string type, string message)
        {
            _streamWriter.WriteLine(type, message);
        }
    }
}
