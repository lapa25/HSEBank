using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Factories
{
    public abstract class BaseFactory<TId> where TId : EntityId<int>
    {
        protected static int NextId = 1;

        protected static TId GenerateId()
        {
            TId id = (TId)Activator.CreateInstance(typeof(TId), NextId++)!;
            return id;
        }
    }
}
