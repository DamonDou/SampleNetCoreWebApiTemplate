using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleNetCoreWebApiTemplate.WebApi.MiddleWare
{
    public class CustomerExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomerExceptionMiddleWare> _logger;

        public CustomerExceptionMiddleWare(RequestDelegate next, ILogger<CustomerExceptionMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                _logger.LogError(ex, message);

                if (context.Response.HasStarted)
                {
                    throw ex;
                }
                else
                {
                    context.Response.Clear();
                    var apiResult = new ApiResult() { State = Common.CommonValue.WebApiReusltState.Error.ToString() ,Msg = message };
                    var content = JsonConvert.SerializeObject(apiResult);
                    context.Response.StatusCode = 500;
                    context.Response.ContentLength = content.Length;
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(content,System.Text.Encoding.UTF8);
                }
            }
        }
    }
}
