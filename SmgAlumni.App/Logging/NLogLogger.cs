using NLog;

namespace SmgAlumni.App.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;
        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
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