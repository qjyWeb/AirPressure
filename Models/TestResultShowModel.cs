using SQLite;

namespace AirPressure.Models
{
    [Table("TestResults")]
    public class TestResultShowModel
    {
        public string? SN { get; set; }
        public string? CreateTime { get; set; }
        public string? TestMode { get; set; }
        public string? Result { get; set; }
        public string? LeakageRate { get; set; }
        public string? TestPressure { get; set; }
        public string? TPUL { get; set; }
        public string? TPLL { get; set; }
    }
}
