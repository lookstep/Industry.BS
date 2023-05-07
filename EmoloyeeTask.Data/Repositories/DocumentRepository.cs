using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Repositories
{
    public class DocumentRepository : DbRepository<Document>
    {
        private readonly AppDbContext _db;
        public DocumentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public override async Task<Document> Add(Document document)
        {
            var result = await _db.Documents
                .AddAsync(document);

            _db.SaveChangesAsync();

            return result.Entity;
        }
        public override async Task Delete(int id)
        {
            var result = await _db.Documents
                      .FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
            {
                _db.Documents.Remove(result);

                await _db.SaveChangesAsync();
            }
        }
        public override Task<IEnumerable<Document>> GetAll()
        {
            return base.GetAll();
        }
        public override Task<Document> Update(Document Entity)
        {
            return base.Update(Entity);
        }
        public override Task<Document> Get(int id)
        {
            return base.Get(id);
        }
        
    }
}
