namespace Api.Application.Common
{
    public interface ILogService
    {
        void LogInfo(string message);
        void LogError(string message, Exception? ex = null);
        void LogCritical(string message, Exception? ex = null);
    }
}
