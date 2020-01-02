using System.Collections.Generic;
using Dorywcza.Models.Auth;

namespace Dorywcza.Services.AuthService
{
    public interface IAuthProvider
    {
        User AuthenticateUser(string username, string password);
        IEnumerable<User> GetUsers();
        User GetUser(int id);
        User PostUser(User user, string password);
        void PutUser(User user, string password = null);
        void DeleteUser(int id);
    }
}
