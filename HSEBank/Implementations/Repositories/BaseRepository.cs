using HSEBank.Contracts;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Repositories
{
    public abstract class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
       where TEntity : class
       where TId : EntityId<int>
    {
        protected readonly Dictionary<TId, TEntity> Entities = [];

        public virtual void Add(TEntity entity)
        {
            Entities[GetId(entity)] = entity;
        }

        public virtual void Remove(TId id)
        {
            _ = Entities.Remove(id);
        }

        public virtual TEntity? Get(TId id)
        {
            return Entities.TryGetValue(id, out TEntity? entity) ? entity : null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Entities.Values;
        }

        public virtual void Update(TEntity entity)
        {
            Entities[GetId(entity)] = entity;
        }

        protected abstract TId GetId(TEntity entity);
    }
}
