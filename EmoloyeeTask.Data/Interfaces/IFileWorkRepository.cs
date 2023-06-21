using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Interfaces
{
    public interface IFileWorkRepository<T>
    {
        public Task<T> AddWithFile(T entity, IFormFile file);
        public Task<T> UpdateFile(T Entity, IFormFile file);
        public void DeleteFile(T Entity, string filePath);
    }
}
