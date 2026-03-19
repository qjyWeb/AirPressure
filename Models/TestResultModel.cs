using Newtonsoft.Json;

namespace AirPressure.Models
{
    public class TestResultModel
    {
        public string User { get; set; } = "Admin";
        public string SN => GlobalVar.Serial;
        public string Model => GlobalVar.ModelName;
        public string? Result { get; set; } = "PASS"; // PASS或 FAIL

        // 气密性测试特有的详细数据字典
        public Dictionary<string, string> DetailedData { get; private set; }

        public TestResultModel()
        {
            DetailedData = new Dictionary<string, string>()
            {
                ["气密测试测试压力"] = "0",
                ["气密测试测试泄漏值"] = "0",
                ["气密测试测试压力单位"] = "kPa",
                ["气密测试测试泄漏值单位"] = "ml/min",
                ["气密测试测试压力Max"] = "0",
                ["气密测试测试压力Min"] = "0"
            };
        }

        /// <summary>
        /// 转换成 MES 要求的 JSON 字符串数组格式
        /// </summary>
        public string ToMesJsonArrayString()
        {
            string detailJson = JsonConvert.SerializeObject(this.DetailedData);

            string[] args = {
                "A1",                       // 0
                "A1",                       // 1
                this.User,                  // 2
                "a",                        // 3
                "A1",                       // 4
                this.SN,                    // 5
                JsonConvert.SerializeObject(new string[]{""}), // 6
                GlobalVar.Status,           // 7
                this.Result,                // 8
                GlobalVar.Flag,             // 9
                GlobalVar.RunTimeMinutes,   // 10
                GlobalVar.RunTimeSeconds,   // 11
                GlobalVar.StartTime,        // 12
                GlobalVar.EndTime,          // 13
                this.Result,                // 14
                detailJson,                 // 15: 这里的详细数据
                "",                       // 16
                "1",                        // 17
                this.Model                  // 18
            };

            return JsonConvert.SerializeObject(args);
        }
    }
}