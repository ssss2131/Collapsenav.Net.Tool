using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Collapsenav.Net.Tool.Data
{
    public interface IModifyRepository<TKey, T> : IQueryRepository<TKey, T> where T : class, IBaseEntity<TKey>
    {
        Task<int> AddAsync(IEnumerable<T> entityList);
        Task<int> DeleteAsync(Expression<Func<T, bool>> exp, bool isTrue = false);
        Task<int> DeleteAsync(IEnumerable<TKey> id, bool isTrue = false);
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);
    }
}