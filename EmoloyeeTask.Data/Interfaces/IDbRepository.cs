namespace EmoloyeeTask.Data.Interfaces
{
    public interface IDbRepository<T> where T : class, IEntity
    {
        Task<T> Add(T NewEntity);    
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Update(T Entity);
        Task Delete(int id);
    }
}
