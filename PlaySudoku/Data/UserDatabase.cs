using System;
using System.Linq;
using PlaySudoku.Models;

namespace PlaySudoku.Data
{
    public class UserDatabase : InMemoryDatabase<User>
    {
        private readonly FileUserStorage _storage;

        public UserDatabase(FileUserStorage storage = null)
        {
            _storage = storage ?? new FileUserStorage();
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            var users = _storage.LoadUsers();
            if (users.Count > 0)
            {
                lock (_lock)
                {
                    _data = users;
                    _currentId = users.Max(u => u.Id) + 1;
                }
            }
        }

        private void SaveToFile()
        {
            lock (_lock)
            {
                _storage.SaveUsers(_data);
            }
        }

        public override void Add(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lock)
            {
                entity.Id = _currentId++;
                base.Add(entity);
                SaveToFile();
            }
        }

        protected override Func<User, bool> GetIdPredicate(int id)
        {
            return u => u.Id == id;
        }

        public User GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Ім'я користувача не може бути порожнім", nameof(username));

            lock (_lock)
            {
                return _data.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public override void Update(User entity)
        {
            base.Update(entity);
            SaveToFile();
        }
    }
}