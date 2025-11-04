using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Repositories
{
    public class OperationRepository : BaseRepository<Operation, OperationId>
    {
        protected override OperationId GetId(Operation entity)
        {
            return entity.Id;
        }
    }
}
