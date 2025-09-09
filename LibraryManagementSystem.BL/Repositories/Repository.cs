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
        public readonly LibraryManagementSystemDBEntities context;
        protected readonly DbSet<T> dbSet;

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

        public void Attach(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Unchanged;
        }

        public int Delete(int id)
        {
            dbSet.Remove(FindById(id));
            return Save();
        }

        public int Elave(T entity)
        {
            context.Entry(entity).State = EntityState.Added;

            // Əlaqəli entity-lərin vəziyyətini dəyişmə:
            foreach (var entry in context.ChangeTracker.Entries()
                                         .Where(e => e.Entity != entity))
            {
                if (entry.State == EntityState.Added)
                    entry.State = EntityState.Unchanged;
            }
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

        public IQueryable<T> GetAllByIncludes(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public int Redakte(T entity, int id)
        {
            var existing = dbSet.Find(id);
            if (existing == null)
                throw new Exception("Obyekt tapılmadı!");

            // Scalar property-ləri kopyalayır:
            context.Entry(existing).CurrentValues.SetValues(entity);

            // Navigation-lar controller-də idarə olunur (Add/Clear):
            return Save();
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
