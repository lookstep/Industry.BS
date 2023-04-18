
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual async Task Delete(int id)
        {
            var findedEntity = await Get(id);
            _db.Set<T>().Remove(findedEntity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<T> Get(int id)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
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
    }
}
