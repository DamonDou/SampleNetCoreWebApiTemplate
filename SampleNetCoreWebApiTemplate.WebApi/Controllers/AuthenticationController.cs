using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SampleNetCoreWebApiTemplate.DataAccess;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using SampleNetCoreWebApiTemplate.WebApi.Filter;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private IConfiguration _configuration;

        public AuthenticationController(IDbSession dbSession, ILoggerFactory loggerFactory, IConfiguration configuration ) : base(dbSession, loggerFactory)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("api/[controller]/GetToken")]
        [FormatResult]
        public string GetToken([FromBody]RequstTokenUserModel userModel)
        {

            if (userModel == null || string.IsNullOrEmpty(userModel.Name) || string.IsNullOrEmpty(userModel.Password))
            {
                AddErrorResult("用户名和密码不能为空");
                return string.Empty;
            }

            if (ValidateUser(userModel))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, userModel.Name) };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                   issuer: _configuration["ValidIssuer"],
                   audience: _configuration["ValidAudience"],
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: creds);

                return "Bearer " +new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                AddErrorResult("用户名或密码错误");
            }

            return string.Empty;
        }

        /// <summary>
        ///  验证用户名密码是否有效，需要根据系统逻辑实现。
        /// </summary>
        /// <param name="userModel">用户对象</param>
        /// <returns>
        ///  验证通过返回true,否则返回false
        /// </returns>
        private  bool ValidateUser(RequstTokenUserModel userModel)
        {
            return userModel.Name == "test" && userModel.Password == "test";
        }
    }
}