using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleNetCoreWebApiTemplate.DataAccess;
using SampleNetCoreWebApiTemplate.Model.ViewModel;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IDbSession _dbSession;

        public TestController(IDbSession dbSession)
        {
            _dbSession = dbSession;
        }

        [HttpGet]
        [Route("api/[controller]/GetAllUserName")]
        public async Task<List<string>> GetAllUserName()
        {
           return await _dbSession.GetUserNamesAsync();
        }

        [HttpGet]
        [Route("api/[controller]/GetAllUser")]
        public async Task<List<UserViewModel>> GetAllUser()
        {
           return await _dbSession.GetUserList();
        }

    }
}