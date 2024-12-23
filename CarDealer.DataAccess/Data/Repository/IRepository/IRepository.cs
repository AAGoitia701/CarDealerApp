﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        T GetOne(Expression<Func<T, bool>> filter,string? includeProperties = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
