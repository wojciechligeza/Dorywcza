using System.Collections.Generic;
using System.Threading.Tasks;
using Dorywcza.Models.Auth;

namespace Dorywcza.Services.AuthService
{
    public interface IAuthProvider
    {
        Task<User> AuthenticateUser(string username, string password);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> RegisterUser(User user, string password);
        Task PutUser(User user, string password = null);
        Task DeleteUser(int id);
    }
}
