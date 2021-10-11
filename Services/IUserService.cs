using System.Collections.Generic;
using System.Threading.Tasks;
using twelve_factor_app.Models;

namespace twelve_factor_app.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> getUsersFromRemote(int count = 1000);
        IEnumerable<User> makeUsersSendToRemote(int countToSend = 1);
    }
}
