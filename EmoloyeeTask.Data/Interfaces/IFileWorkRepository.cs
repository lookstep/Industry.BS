using Microsoft.AspNetCore.Http;

namespace EmoloyeeTask.Data.Interfaces
{
    public interface IFileWorkRepository<T>
    {
        public Task<T> AddWithFile(T entity, IFormFile file);
        public Task<T> UpdateFile(T Entity, IFormFile file);
        public void DeleteFile(T Entity, string filePath);
    }
}
