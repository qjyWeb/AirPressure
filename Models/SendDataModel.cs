namespace AirPressure.Models
{
    /// <summary>
    /// 发送用的数据格式
    /// </summary>
    public class SendDataModel
    {
        public string station = GlobalVar.StationName;
        public string cmd = "End";
        public string? value { get; set; }
    }
}
