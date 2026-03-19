namespace AirPressure.Models
{
    /// <summary>
    /// 收到消息后的返回数据格式
    /// </summary>
    public class ResponseDataModel
    {
        public string? station = GlobalVar.StationName;
        public string? cmd  { get; set; }
        public string? success { get; set; }
        public string? message { get; set; }
    }
}
