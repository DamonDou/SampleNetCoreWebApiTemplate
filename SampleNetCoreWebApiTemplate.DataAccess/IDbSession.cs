using System.Collections.Generic;
using System.Threading.Tasks;
using SampleNetCoreWebApiTemplate.Model.ViewModel;

namespace SampleNetCoreWebApiTemplate.DataAccess
{
    public interface IDbSession
    {
          Task<List<string>> GetUserNamesAsync();
          Task<List<UserViewModel>> GetUserList();
    }
}