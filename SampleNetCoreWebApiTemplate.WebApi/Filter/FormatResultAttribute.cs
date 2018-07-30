using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SampleNetCoreWebApiTemplate.Common;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using System;
using System.Linq;
using System.Net;

namespace SampleNetCoreWebApiTemplate.WebApi.Filter
{
    public class FormatResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            FormateResult(context);
        }

        /// <summary>
        ///  格式化结果
        /// </summary>
        /// <param name="context">上下文</param>
        private  void FormateResult(ActionExecutedContext context)
        {
            // 如果有不需要格式化的特性，将不进行格式
            if (context.Filters.Any(a => a.GetType().Equals(typeof(UnFormatResultAttribute))))
            {
                return;
            }

            // 如果发生异常，不进行处理，将异常抛出到全局异常处理中
            if (context.Exception != null)
            {
                throw context.Exception;
            }

            // 替换掉NoContent状态，使用自定义的消息格式
            var statusCode = context.HttpContext.Response.StatusCode == (int)HttpStatusCode.NoContent
                ? (int)HttpStatusCode.OK
                : context.HttpContext.Response.StatusCode;

            var formateResult = new ApiResult();
            var msg = new Object();

            if (context.ActionDescriptor.Properties.TryGetValue(CommonValue.WebApiResultKey, out msg))
            {
                // 需要删除，否则下次请求时，上次请求的信息还会保留
                context.ActionDescriptor.Properties.Remove(CommonValue.WebApiResultKey);
                formateResult = msg as ApiResult;
                if (formateResult == null)
                {
                    formateResult = new ApiResult { State = CommonValue.WebApiReusltState.Success.ToString() };
                }
            }
            else if (context.ModelState.ValidationState == ModelValidationState.Invalid && statusCode != (int)HttpStatusCode.OK)
            {
                formateResult = new ApiResult { State = CommonValue.WebApiReusltState.Error.ToString() };
            }
            else
            {
                formateResult = new ApiResult { State = CommonValue.WebApiReusltState.Success.ToString() };
            }

            var originalResult = context.Result as ObjectResult;

            if (originalResult != null)
            {
                formateResult.Data = originalResult.Value;
            }
            else
            {
                formateResult.Data = null;
            }

            context.Result = new ObjectResult(formateResult);
        }
    }
}
