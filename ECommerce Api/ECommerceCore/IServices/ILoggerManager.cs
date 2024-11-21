namespace ECommerce.Core.IServices
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarn(string message);
        void LogError(string message);
    }
}
