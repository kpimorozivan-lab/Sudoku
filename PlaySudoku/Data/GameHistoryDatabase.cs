using System;
using System.Collections.Generic;
using System.Linq;
using PlaySudoku.Models;

namespace PlaySudoku.Data
{
    public class GameHistoryDatabase : InMemoryDatabase<GameHistory>
    {
        public override void Add(GameHistory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lock)
            {
                entity.Id = _currentId++;
                base.Add(entity);
            }
        }

        protected override Func<GameHistory, bool> GetIdPredicate(int id)
        {
            return g => g.Id == id;
        }

        public List<GameHistory> GetByUserId(int userId)
        {
            lock (_lock)
            {
                return _data.Where(g => g.UserId == userId)
                    .OrderByDescending(g => g.PlayedAt)
                    .ToList();
            }
        }
    }
}