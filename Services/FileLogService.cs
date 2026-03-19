using System.Text;

namespace AirPressure.Services
{
    public class FileLogService : ILogService
    {
        public event Action<string, Color>? OnLog;

        public void Log(string message, Color color)
        {
            OnLog?.Invoke(message, color);
        }

        public Task SaveLogAsync(IEnumerable<string> lines)
        {
            try
            {
                string fileName = $"{DateTime.Now:yyyyMMdd}.txt";
                string directory = Path.Combine(GlobalVar.ExePath, "log");
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, fileName);

                var list = new List<string> { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                list.AddRange(lines);
                list.Add("===============================================");
                return File.AppendAllLinesAsync(path, list, Encoding.UTF8);
            }
            catch
            {
                return Task.CompletedTask;
            }
        }
    }
}
