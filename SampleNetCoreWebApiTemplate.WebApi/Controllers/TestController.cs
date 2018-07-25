using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.DataAccess;
using SampleNetCoreWebApiTemplate.Model.ViewModel;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public TestController(IDbSession dbSession, ILoggerFactory loggerFactory) : base(dbSession, loggerFactory)
        {
        }

        [HttpGet]
        [Route("api/[controller]/GetAllUserName")]
        public async Task<List<string>> GetAllUserName()
        {
            Logger.LogInformation("GetAllUserName");
            return await DbSession.GetUserNamesAsync();
        }

        [HttpGet]
        [Route("api/[controller]/GetAllUser")]
        public async Task<List<UserViewModel>> GetAllUser()
        {
            Logger.LogInformation("GetAllUser");
            return await DbSession.GetUserList();
        }

        [HttpGet]
        [Route("api/[controller]/GetException")]
        public async Task<List<UserViewModel>> GetException()
        {
            Logger.LogInformation("GetException");
            throw (new Exception("test"));
            return await DbSession.GetUserList();
        }
    }
}