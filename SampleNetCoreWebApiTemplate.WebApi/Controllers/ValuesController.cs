using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SampleNetCoreWebApiTemplate.WebApi.Controllers
{
    /// <summary>
    ///  示例
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        /// <summary>
        ///  查询所有数据
        /// </summary>
        /// <returns>字符串集合</returns>
        /// <remarks>
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }    
        /// </remarks>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///  通过ID查询数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>数据</returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        ///  插入数据
        /// </summary>
        /// <param name="value">数据</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        /// <summary>
        ///  更新数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        /// <summary>
        ///  删除数据
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
