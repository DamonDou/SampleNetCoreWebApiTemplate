using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.DataAccess;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IDbSession DbSession;
        protected readonly ILogger Logger;

        public BaseController(IDbSession dbSession,ILoggerFactory loggerFactory)
        {
            DbSession = dbSession;
            Logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }



        // TODO: 目前没有好的方法实现注入，暂时采用构造函数的方式注入
         //DbSession = this.HttpContext.RequestServices.GetService(typeof(IDbSession)) as IDbSession;
         //var loggerFactory = this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
         //Logger = loggerFactory.CreateLogger(this.GetType().FullName);
    }
}