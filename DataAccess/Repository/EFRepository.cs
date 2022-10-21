using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Entities.Models;


namespace DataAccess
{
    /// <summary>
    /// Repository là thành phần chính tương tác đến CSDL
    /// EFRepository là repository cơ bản nhất, bản thân nó chứa các thành phần cơ bản từ IRepository với một vài thao tác tương tác ban đầu
    /// </summary>
    /// <typeparam name="T"> Model ánh xạ từ các bảng trong CSDL </typeparam>
    
    public class EFRepository<T> : IRepository<T> where T : class
    {
       
        protected Context DbContext;
        protected DbSet<T> DbSet;

        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new Exception("EFRepository::initialize::dbContext::Canot null");
            }
            this.DbContext = (Context)dbContext;
            this.DbSet = this.DbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Where(predicate);
        }
      
        public void Delete(T entity)
        {        
            this.DbContext.Entry(entity).State = EntityState.Deleted;
            Save();
        }

        public void Add(T entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Added;
            Save();
        }

        public void AddWithoutSave(T entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public int Save()
        {
            return DbContext.SaveChanges();
        }
    }
}
