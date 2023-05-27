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
    }
}
