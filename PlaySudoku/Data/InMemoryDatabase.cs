using System;
using System.Collections.Generic;
using System.Linq;
using PlaySudoku.Interfaces;

namespace PlaySudoku.Data
{
    public abstract class InMemoryDatabase<T> : IDatabase<T> where T : class
    {
        protected List<T> _data = new List<T>();
        protected int _currentId = 1;
        protected readonly object _lock = new object();

        public virtual void Add(T entity)
        {
            lock (_lock)
            {
                _data.Add(entity);
            }
        }

        public virtual T GetById(int id)
        {
            lock (_lock)
            {
                return _data.FirstOrDefault(GetIdPredicate(id));
            }
        }

        public virtual List<T> GetAll()
        {
            lock (_lock)
            {
                return new List<T>(_data);
            }
        }

        public virtual void Update(T entity)
        {
            
        }

        public bool Exists(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _data.Any(predicate);
            }
        }

        protected abstract Func<T, bool> GetIdPredicate(int id);
    }
}