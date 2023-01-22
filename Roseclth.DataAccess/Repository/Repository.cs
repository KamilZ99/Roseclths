using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Roseclth.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (includeProperties != null)
            {
                var properties = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach ( var property in properties)
                {
                    query = query.Include(property);
                }
            }

            return query;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            if (includeProperties != null)
            {
                var properties = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach ( var property in properties)
                {
                    query = query.Include(property);
                }
            }

            return query;
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            if (includeProperties != null)
            {
                var properties = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach ( var property in properties)
                {
                    query = query.Include(property);
                }
            }
            
            return query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
