using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using Newtonsoft.Json;

namespace SampleNetCoreWebApiTemplate.WebApi.MiddleWare
{
    public class TraceLogMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceLogMiddleWare> _logger;

        // 计时器对象在参数字典中对应的key
        private const string KeyName = "PerformanceTimeKey";

        public TraceLogMiddleWare(RequestDelegate next, ILogger<TraceLogMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items.Add(KeyName, Stopwatch.StartNew());

            var originalResponseStream = context.Response.Body;

            using (MemoryStream responseStream = new MemoryStream())
            {

                context.Response.Body = responseStream;

                await _next(context);

                var contextItem = context.Items.FirstOrDefault(a => a.Key.ToString() == KeyName);

                if (contextItem.Value != null)
                {
                    // 删除添加的信息，避免下次使用的时候，保存的还是上次的内容
                    context.Items.Remove(KeyName);

                    var stopWatch = contextItem.Value as Stopwatch;

                    var requestPath = context.Request.Path.ToUriComponent();

                    var paramterStr = string.Empty;

                    if (context.Request.Method == "GET")
                    {
                        paramterStr = context.Request.QueryString.ToString();
                    }

                    if (context.Request.Method == "POST")
                    {
                        context.Request.EnableBuffering();
                        context.Request.Body.Seek(0, SeekOrigin.Begin);
                        StreamReader requestStreamReader = new StreamReader(context.Request.Body);
                        paramterStr = await requestStreamReader.ReadToEndAsync();
                    }

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseStr = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await context.Response.Body.CopyToAsync(originalResponseStream);

                    var clientIp = context.Connection.RemoteIpAddress.ToString();
                    stopWatch.Stop();
                    var traceLog = new TraceLogModel() {
                        Method = context.Request.Method,
                        ClientIp = clientIp,
                        Url = requestPath,
                        Parameter = paramterStr,
                        ResponseContent = responseStr,
                        RequestDate = DateTime.Now,
                        Type = this.GetType().FullName,
                        HttpStatusCode = context.Response.StatusCode,
                        ConsumeTime = (int)stopWatch.ElapsedMilliseconds
                    };
                    _logger.LogInformation(JsonConvert.SerializeObject(traceLog));
                }
            }
        }
    }
}
