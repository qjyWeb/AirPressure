using AirPressure.Models;
using AirPressure.Utils;

namespace AirPressure.Services
{
    public class SqliteRepositoryAdapter : ITestRepository
    {
        private readonly SqliteHelper _helper;
        public SqliteRepositoryAdapter()
        {
            _helper = new SqliteHelper();
        }

        public Task<List<TestResultShowModel>> QueryDataAsync(string startTime, string endTime, string sn, string result)
        {
            return _helper.QueryDataAsync(startTime, endTime, sn, result);
        }

        public Task<List<TestResultShowModel>> GetAllAsync()
        {
            return _helper.GetAllAsync();
        }

        public Task<int> GetTodayTestCountAsync()
        {
            return _helper.GetTodayTestCountAsync();
        }

        public Task<int> GetTodayDefectiveCountAsync()
        {
            return _helper.GetTodayDefectiveCountAsync();
        }

        public Task<int> GetTotalTestCountAsync()
        {
            return _helper.GetTotalTestCountAsync();
        }

        public Task<int> GetTotalDefectiveCountAsync()
        {
            return _helper.GetTotalDefectiveCountAsync();
        }
    }
}
