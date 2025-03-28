﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretBusiness.Abstract
{
    public interface IGenericRepository<T> where T : class, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(int id);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);

    }
}
