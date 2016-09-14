using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lex.Db;

namespace GaugeDbWrapper.Wrapper
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly DbTable<TEntity> dbSet;
        private readonly DbInstance dbInstance;
        public GenericRepository(DbInstance context)
        {
            dbInstance = context;
            dbSet = context.Table<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get()
        {
            List<TEntity> query = dbSet.LoadAll().ToList();
            return query;
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.LoadByKey(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Save(entity);
        }
        public virtual void Insert(IEnumerable<TEntity> entries)
        {
            dbSet.Save(entries);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Save(entityToUpdate);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.LoadByKey(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            dbSet.Delete(entityToDelete);
        }

        public virtual void Delete(IEnumerable<TEntity> entitiesToDelete)
        {
            dbSet.Delete(entitiesToDelete);
        }

        public int GetMaxKey(string propertyName = "Id")
        {
            var property = typeof(TEntity).GetRuntimeProperties().FirstOrDefault(c => c.Name == propertyName);
            var idList = Get().Select(data => property.GetValue(data, null)).Select(Convert.ToInt32).ToList();
            return !idList.Any() ? 0 : idList.Max();
        }
    }
}
