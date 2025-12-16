using System;
using System.Security.Cryptography;
using System.Text;
using PlaySudoku.Data;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDatabase _userDb;
        private User _currentUser;

        public AuthService(UserDatabase userDb)
        {
            _userDb = userDb ?? throw new ArgumentNullException(nameof(userDb));
        }

        public User Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("The username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The password cannot be empty", nameof(password));

            if (username.Length < 3)
                throw new ArgumentException("The username must contain at least 3 characters.", nameof(username));

            if (password.Length < 4)
                throw new ArgumentException("The password must contain at least 4 characters.", nameof(password));

            if (_userDb.GetByUsername(username) != null)
                throw new InvalidOperationException("A user with that name already exists!");

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            _userDb.Add(user);
            return user;
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("The username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The password cannot be empty", nameof(password));

            var user = _userDb.GetByUsername(username);
            if (user == null || user.PasswordHash != HashPassword(password))
                throw new InvalidOperationException("Incorrect username or password!");

            _currentUser = user;
            return user;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}