using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

namespace LibraryManagementSystem.BL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private LibraryManagementSystemDBEntities context;
        private DbSet<T> dbSet;

        public Repository() //ctor 
        {
            if (context == null)
            {
                context = new LibraryManagementSystemDBEntities();
                dbSet = context.Set<T>();
            }
        }

        public int Add(T entity)
        {
            dbSet.Add(entity);
            return Save();
        }

        public int Delete(int id)
        {
            dbSet.Remove(FindById(id));
            return Save();
        }

        public T FindById(int id)
        {
            return dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList(); // Bütün qeydleri qaytarır
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).ToList(); // Verilən ifadəyə uyğun bütün qeydleri qaytarır
        }

        public IQueryable<T> GetAllByInclude(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public IQueryable<T> GetAllByInclude(string table)
        {
            return dbSet.Include(table);
        }

        public int Update(T entity)
        {
            dbSet.AddOrUpdate(entity);
            return Save(); // Dəyişiklikləri əlavə edir və saxlayır
        }
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
