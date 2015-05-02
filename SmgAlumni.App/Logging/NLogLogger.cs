using NLog;

namespace SmgAlumni.App.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;
        public NLogLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }
        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}