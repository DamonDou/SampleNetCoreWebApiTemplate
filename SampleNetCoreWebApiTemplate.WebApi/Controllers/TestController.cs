using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleNetCoreWebApiTemplate.DataAccess;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using SampleNetCoreWebApiTemplate.WebApi.Filter;

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
        [UnFormatResult]
        public async Task<List<string>> GetAllUserName()
        {
            Logger.LogInformation("GetAllUserName");
            return await DbSession.GetUserNamesAsync();
        }

        [HttpGet]
        [Route("api/[controller]/GetAllUser")]
        [FormatResult]
        [Authorize]
        public async Task<List<UserViewModel>> GetAllUser()
        {
            Logger.LogInformation("GetAllUser");
            return await DbSession.GetUserList();
        }

        [HttpPost]
        [Route("api/[controller]/AddUser")]
        [FormatResult]
        public string AddUser([FromBody] UserViewModel user)
        {

            return "";
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