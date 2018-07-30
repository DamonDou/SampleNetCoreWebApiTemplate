using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.DataAccess;
using SampleNetCoreWebApiTemplate.Model.ViewModel;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IDbSession DbSession;
        protected readonly ILogger Logger;

        public BaseController(IDbSession dbSession, ILoggerFactory loggerFactory)
        {
            DbSession = dbSession;
            Logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        protected void AddConfirmResult(string msg, int continueCount)
        {
            var resultModel = new ApiResult() { Msg = msg, State = Common.CommonValue.WebApiReusltState.Warning.ToString(), ContinueCount = continueCount };

            AddResult(resultModel);
        }

        protected void AddWarningResult(string msg)
        {
            var resultModel = new ApiResult() { Msg = msg, State = Common.CommonValue.WebApiReusltState.Warning.ToString() };

            AddResult(resultModel);
        }

        protected void AddErrorResult(string msg)
        {
            var resultModel = new ApiResult() { Msg = msg, State = Common.CommonValue.WebApiReusltState.Error.ToString() };

            AddResult(resultModel);
        }

        private void AddResult(ApiResult model)
        {
            if (ControllerContext.ActionDescriptor.Properties.Keys.Any(a => a.ToString() == Common.CommonValue.WebApiResultKey))
            {
                ControllerContext.ActionDescriptor.Properties[Common.CommonValue.WebApiResultKey] = model;
            }
            else
            {
                ControllerContext.ActionDescriptor.Properties.Add(Common.CommonValue.WebApiResultKey, model);
            }            
        }

        // TODO: 目前没有好的方法实现注入，暂时采用构造函数的方式注入
        //DbSession = this.HttpContext.RequestServices.GetService(typeof(IDbSession)) as IDbSession;
        //var loggerFactory = this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
        //Logger = loggerFactory.CreateLogger(this.GetType().FullName);
    }
}