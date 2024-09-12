using Api.Application.Common;

namespace Api
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        public void LogError(string message, Exception? ex = null)
        {
            _logger.LogError(message, ex);
        }

        public void LogCritical(string message, Exception? ex = null)
        {
            _logger.LogCritical(message, ex);
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
