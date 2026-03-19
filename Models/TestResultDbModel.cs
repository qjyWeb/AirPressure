using SQLite;

namespace AirPressure.Models
{
    [Table("TestResults")]
    public class TestResultDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? SN { get; set; } = GlobalVar.Serial;
        public string? Vehicle { get; set; } = GlobalVar.Vehicle; // 新增治具码字段
        public string? CreateTime { get; set; }
        public string? InstrumentNumber { get; set; }
        public string? TestMode { get; set; }
        public string? Result { get; set; }
        public string? LeakageRate { get; set; }
        public string? DETUL { get; set; }
        public string? DETLL { get; set; }
        public string? DeltaP { get; set; }
        public string? TestPressure { get; set; }
        public string? TPUL { get; set; }
        public string? TPLL { get; set; }
        public string? CH { get; set; }
        public string? CheckSum { get; set; }
    }

}