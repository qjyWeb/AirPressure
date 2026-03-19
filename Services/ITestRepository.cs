using AirPressure.Models;

namespace AirPressure.Services
{
    public interface ITestRepository
    {
        Task<List<TestResultShowModel>> QueryDataAsync(string startTime, string endTime, string sn, string result);
        Task<List<TestResultShowModel>> GetAllAsync();
        Task<int> GetTodayTestCountAsync();
        Task<int> GetTodayDefectiveCountAsync();
        Task<int> GetTotalTestCountAsync();
        Task<int> GetTotalDefectiveCountAsync();
    }
}
