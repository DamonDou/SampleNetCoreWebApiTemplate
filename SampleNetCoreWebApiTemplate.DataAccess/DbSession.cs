using System.Collections.Generic;
using System.Threading.Tasks;
using SampleNetCoreWebApiTemplate.Model.ViewModel;
using SampleNetCoreWebApiTemplate.DataAccess.Extend;

namespace SampleNetCoreWebApiTemplate.DataAccess
{
    public class DbSession : IDbSession
    {
        private ColorDbContext _colorDbContext;

        public DbSession(ColorDbContext colorDbContext)
        {
            _colorDbContext = colorDbContext;
        }

        public async Task<List<string>> GetUserNamesAsync()
        {
            var result = new List<string>();

            result = await _colorDbContext.SqlQueryAsync<string>("SELECT USERNAME FROM COLORDB.dbo.DCSECURITYUSER");

            return result;
        }

        public async Task<List<UserViewModel>> GetUserList()
        {
            return await _colorDbContext.SqlQueryAsync<UserViewModel>("SELECT ID,USERACCOUNT,USERNAME,ROLEID FROM COLORDB.dbo.DCSECURITYUSER");

        }
    }
}