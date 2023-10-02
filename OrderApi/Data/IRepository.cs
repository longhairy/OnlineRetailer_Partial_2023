using System.Collections.Generic;

namespace OrderApi.Data
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task EditAsync(T entity);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task RemoveAsync(int id);
    }
}
