using HSEBank.ValueObjects;

namespace HSEBank.Contracts
{
    public interface IRepository<TEntity, TId>
        where TEntity : class
        where TId : EntityId<int>
    {
        void Add(TEntity entity);
        void Remove(TId id);
        TEntity? Get(TId id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity entity);
    }
}
