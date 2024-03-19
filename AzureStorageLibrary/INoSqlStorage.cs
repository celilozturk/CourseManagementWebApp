using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(string rowKey, string partitionKey);
        Task<TEntity> GetAsync(string rowKey, string partitionKey);  
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Query(Expression<Func<TEntity,bool>> query);
    }
}
