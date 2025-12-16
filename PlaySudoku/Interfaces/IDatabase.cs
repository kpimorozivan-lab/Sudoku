using System;
using System.Collections.Generic;

namespace PlaySudoku.Interfaces
{
    public interface IDatabase<T> where T : class
    {
        void Add(T entity);
        T GetById(int id);
        List<T> GetAll();
        void Update(T entity);
        bool Exists(Func<T, bool> predicate);
    }
}