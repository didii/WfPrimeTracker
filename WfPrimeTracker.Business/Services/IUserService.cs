using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WfPrimeTracker.Dtos.UserData;

namespace WfPrimeTracker.Business.Services {
    public interface IUserService {
        Task<UserSaveDataDto> GetUserData();
        Task SaveUserData(UserSaveDataDto data);
    }
}
