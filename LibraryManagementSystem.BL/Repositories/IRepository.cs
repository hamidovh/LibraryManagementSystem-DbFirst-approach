using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LibraryManagementSystem.BL.Repositories
{
    public interface IRepository<T> where T : class // T yazilan yere Class-lar gelecek, <> Generic adlanir
    {
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAllByInclude(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllByInclude(string table);
        IQueryable<T> GetAllByIncludes(params Expression<Func<T, object>>[] includeProperties);
        T FindById(int id);
        T Get(Expression<Func<T, bool>> expression);
        int Add(T entity);
        int Elave(T entity);
        void Attach(T entity);
        int Redakte(T entity, int id);
        int Update(T entity);
        int Delete(int id);
        IQueryable<T> GetPaged<TKey>(int page, int pageSize, Expression<Func<T, TKey>> orderBy);
    }
}
