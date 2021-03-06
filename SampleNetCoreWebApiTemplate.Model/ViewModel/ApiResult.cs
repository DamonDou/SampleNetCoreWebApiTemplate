namespace SampleNetCoreWebApiTemplate.Model.ViewModel
{
    public class ApiResult
    {
        /// <summary>
        ///  消息状态：成功,失败(异常),警告,错误
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///  消息信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        ///  数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///  强制提交标识
        /// </summary>
        public int ContinueCount { get; set; }
    }
}