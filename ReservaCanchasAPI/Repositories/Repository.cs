using Microsoft.EntityFrameworkCore;
using ReservaCanchasAPI.Data;

namespace ReservaCanchasAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ReservaCanchasContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ReservaCanchasContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var entity = await GetById(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> GetById(string id)
            => await _dbSet.FindAsync(id);

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
