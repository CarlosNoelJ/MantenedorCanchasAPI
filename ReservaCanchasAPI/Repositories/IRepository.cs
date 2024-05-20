namespace ReservaCanchasAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetById(string id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(string id);
    }
}
