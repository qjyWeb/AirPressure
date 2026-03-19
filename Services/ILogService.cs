namespace AirPressure.Services
{
    public interface ILogService
    {
        event Action<string, Color>? OnLog;
        void Log(string message, Color color);
        Task SaveLogAsync(IEnumerable<string> lines);
    }
}
