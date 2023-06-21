using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmoloyeeTask.Data.Interfaces
{
    public abstract class DbRepository<T> : IDbRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _db;
        public DbRepository(AppDbContext db)
        {
            _db = db;
        }

        public virtual async Task<T> Add(T NewEntity)
        {
            var result = await _db.Set<T>().AddAsync(NewEntity);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public virtual Task<T> AddWithFile(T entity, IFormFile file)
        {
            return Add(entity);
        }

        public virtual async Task Delete(int id)
        {
            var findedEntity = await Get(id);
            _db.Set<T>().Remove(findedEntity);
            await _db.SaveChangesAsync();
        }

        public virtual void DeleteFile(T Entity, string filePath)
        {
            File.Delete(filePath);
        }

        public virtual async Task<T> Get(int id)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public virtual async Task Save()
        {
            _db.SaveChangesAsync();
        }

        public virtual async Task<T> Update(T Entity)
        {
            var updatedEntity = await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == Entity.Id);
            if (updatedEntity == null)
            {
                updatedEntity = Entity;
                await _db.SaveChangesAsync();

                return updatedEntity;
            }
            return null!;
        }

        public virtual Task<T> UpdateFile(T Entity, IFormFile file)
        {
            return Update(Entity);
        }
    }
}
