using System;
using System.Collections.Generic;
using System.Linq;
using PlaySudoku.Data;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly GameHistoryDatabase _historyDb;
        private readonly UserDatabase _userDb;

        public StatisticsService(GameHistoryDatabase historyDb, UserDatabase userDb)
        {
            _historyDb = historyDb ?? throw new ArgumentNullException(nameof(historyDb));
            _userDb = userDb ?? throw new ArgumentNullException(nameof(userDb));
        }

        public List<GameHistory> GetUserHistory(int userId)
        {
            return _historyDb.GetByUserId(userId);
        }

        public int GetUserRating(int userId)
        {
            var user = _userDb.GetById(userId);
            return user?.Rating ?? 0;
        }

        public List<User> GetLeaderboard(int top = 10)
        {
            if (top <= 0)
                throw new ArgumentException("The number of leaders must be greater than 0.", nameof(top));

            return _userDb.GetAll()
                .OrderByDescending(u => u.Rating)
                .Take(top)
                .ToList();
        }
    }
}