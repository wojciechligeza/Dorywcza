﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dorywcza.Data;
using Dorywcza.Models;
using Dorywcza.Models.Auth;
using Dorywcza.Services.AuthService.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Dorywcza.Services.AuthService
{
    public class AuthProvider : IAuthProvider
    {
        private readonly ApplicationDbContext _context;

        public AuthProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            // checking if username or password are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            // fetching username from database
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Username == username);

            // checking if username exists
            if (user == null) return null;

            // checking if password is correct with Salted Password Hashing Helper
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            // authentication successful
            return user;
        }

        public async Task<User> RegisterUser(User user, string password)
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
            await _context.SaveChangesAsync();

            // adding User as Employer
            Employer employer = new Employer()
            {
                CompanyName = user.Username,
                Description = "",
                UserId = _context.Users.Max(a => a.UserId)
            };
             _context.Employers.Add(employer);
             await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.UserId == id);
        }

        public async Task PutUser(User userParam, string password = null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == userParam.UserId);

            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
                    user.Username = userParam.Username;

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
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
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
            if (storedHash.Length != 64) throw new ArgumentException("Niepoprawna wielkość bajtowa password hash (spodziewane 64 bajty)", "storedHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Niepoprawna wielkość bajtowa password salt (spodziewane 128 bajtów)", "storedSalt");

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
