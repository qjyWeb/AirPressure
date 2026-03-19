using AirPressure.Models;
using SQLite;

namespace AirPressure.Utils
{
    public class SqliteHelper
    {
        private readonly SQLiteAsyncConnection _db;

        public SqliteHelper(string dbName = "TestData.db")
        {
            // 数据库路径设为程序运行目录
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);
            _db = new SQLiteAsyncConnection(dbPath);

            // 自动创建表（如果不存在）
            _db.CreateTableAsync<TestResultDbModel>().Wait();
        }

        /// <summary>
        /// 插入一条测试数据
        /// </summary>
        public async Task<int> InsertAsync(TestResultDbModel model)
        {
            return await _db.InsertAsync(model);
        }

        /// <summary>
        /// 核心查询方法：支持按日期、SN码、测试结果筛选（CreateTime和时间参数均为字符串类型）
        /// </summary>
        /// <param name="startTime">开始时间 (字符串格式 yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="endTime">结束时间 (字符串格式 yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="sn">SN码 (模糊查询，为空则不筛选)</param>
        /// <param name="result">测试结果 (OK/NG/All，为空或"All"则不筛选)</param>
        /// <returns>按时间倒序排列的列表</returns>
        public async Task<List<TestResultShowModel>> QueryDataAsync(
            string startTime = null,
            string endTime = null,
            string sn = "",
            string result = "All")
        {
            // 使用原始 SQL 查询，绕过 LINQ 翻译限制
            // SQLite 直接支持字符串的 >= / <= / LIKE 比较，且格式 "yyyy-MM-dd HH:mm:ss" 是字典序安全的
            var sql = "SELECT * FROM TestResults WHERE 1=1";
            var parameters = new List<object>();

            if (!string.IsNullOrEmpty(startTime))
            {
                sql += " AND CreateTime >= ?";
                parameters.Add(startTime);
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                sql += " AND CreateTime <= ?";
                parameters.Add(endTime);
            }

            if (!string.IsNullOrWhiteSpace(sn))
            {
                sql += " AND SN LIKE '%' || ? || '%'";
                parameters.Add(sn);
            }

            if (!string.IsNullOrWhiteSpace(result) && result != "All" && result != "全部")
            {
                sql += " AND Result = ?";
                parameters.Add(result);
            }

            sql += " ORDER BY CreateTime DESC";

            return await _db.QueryAsync<TestResultShowModel>(sql, parameters.ToArray());
        }


        /// <summary>
        /// 获取最新50条数据（按时间倒序）
        /// </summary>
        public async Task<List<TestResultShowModel>> GetAllAsync()
        {
            return await _db.Table<TestResultShowModel>()
                            .OrderByDescending(x => x.CreateTime)
                            .Take(50)           // 只取前50条
                            .ToListAsync();
        }

        /// <summary>
        /// 获取总测试数量（所有记录）
        /// </summary>
        public async Task<int> GetTotalTestCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM TestResults";
            return await _db.ExecuteScalarAsync<int>(sql);
        }

        /// <summary>
        /// 获取总不良品数量（Result = 'FAIL'）
        /// </summary>
        public async Task<int> GetTotalDefectiveCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM TestResults WHERE Result = 'FAIL'";
            return await _db.ExecuteScalarAsync<int>(sql);
        }

        /// <summary>
        /// 获取今日测试数量（基于 CreateTime 的日期部分）
        /// </summary>
        public async Task<int> GetTodayTestCountAsync()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var sql = "SELECT COUNT(*) FROM TestResults WHERE substr(CreateTime, 1, 10) = ?";
            return await _db.ExecuteScalarAsync<int>(sql, today);
        }

        /// <summary>
        /// 获取今日不良品数量（Result = 'NG'）
        /// </summary>
        public async Task<int> GetTodayDefectiveCountAsync()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var sql = "SELECT COUNT(*) FROM TestResults WHERE substr(CreateTime, 1, 10) = ? AND Result = 'FAIL'";
            return await _db.ExecuteScalarAsync<int>(sql, today);
        }

        /// <summary>
        /// 获取今日良率（OK 数量 / 总数量，百分比，返回 double，如 98.5 表示 98.5%）
        /// 如果今日无数据，返回 0.0
        /// </summary>
        public async Task<double> GetTodayYieldRateAsync()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            // 同时查询今日总数量和 OK 数量
            var sql = @"
                SELECT 
                    COUNT(*) AS TotalCount,
                    SUM(CASE WHEN Result = 'OK' THEN 1 ELSE 0 END) AS OKCount
                FROM TestResults 
                WHERE substr(CreateTime, 1, 10) = ?";

            var result = await _db.QueryAsync<dynamic>(sql, today);

            if (result.Count == 0 || result[0].TotalCount == 0)
            {
                return 0.0;
            }

            int total = result[0].TotalCount;
            int okCount = result[0].OKCount;

            return Math.Round((double)okCount / total * 100, 2); // 保留两位小数
        }
    }
}