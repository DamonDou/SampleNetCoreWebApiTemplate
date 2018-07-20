namespace SampleNetCoreWebApiTemplate.Common
{
    public class CommonValue
    {
        #region 模板必备

        /// <summary>
        ///  接口返回的状态信息
        /// </summary>
        public enum WebApiReusltState
        {
            Success,
            Fail,
            Warning,
            Error
        }

        /// <summary>
        ///  webapi控制器与过滤器传递参数的key
        /// </summary>
        public const string WebApiResultKey = "ApiResultMsgModel"; 

        #endregion
    }
}