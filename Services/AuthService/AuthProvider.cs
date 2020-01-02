using System;
using System.Collections.Generic;
using System.Linq;
using Dorywcza.Data;
using Dorywcza.Models.Auth;
using Dorywcza.Services.AuthService.Helpers;

namespace Dorywcza.Services.AuthService
{
    public class AuthProvider : IAuthProvider
    {
        private readonly ApplicationDbContext _context;

        public AuthProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public User AuthenticateUser(string username, string password)
        {
            // checking if username or password are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            // fetching username from database
            var user = _context.Users.SingleOrDefault(a => a.Username == username);

            // checking if username exists
            if (user == null) return null;

            // checking if password is correct with Salted Password Hashing Helper
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            // authentication successful
            return user;
        }

        public User PostUser(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new AppException("Hasło jest wymagane");

            if (_context.Users.Any(a => a.Username == user.Username)) throw new AppException("Użytkownik \"" + user.Username + "\" już istnieje");

            #region Create Salted Password Hashing
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            #endregion
            
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(a => a.UserId == id);
        }

        public void PutUser(User userParam, string password = null)
        {
            var user = _context.Users.FirstOrDefault(a => a.UserId == userParam.UserId);

            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username) user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(userParam.FirstName)) user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName)) user.LastName = userParam.LastName;

            #region Update Salted Password Hasing
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            #endregion
            
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(a => a.UserId == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        #region Salted Password Hashing Helper Methods to proctect from dictionary attacks
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Wartość nie może być pusta", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("VWartość nie może być pusta", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Niepoprawna wielkość bajtowa password hash (spodziewane 64 bajty)", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Niepoprawna wielkość bajtowa password salt (spodziewane 128 bajtów)", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        #endregion
    }
}
