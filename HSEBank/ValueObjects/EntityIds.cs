namespace HSEBank.ValueObjects
{
    public abstract record EntityId<T>(T Value) where T : IComparable<T>, IEquatable<T>;

    public record AccountId(int Value) : EntityId<int>(Value)
    {
        public override string ToString()
        {
            return $"ACC{Value}";
        }
    }

    public record CategoryId(int Value) : EntityId<int>(Value)
    {
        public override string ToString()
        {
            return $"CAT{Value}";
        }
    }

    public record OperationId(int Value) : EntityId<int>(Value)
    {
        public override string ToString()
        {
            return $"OP{Value}";
        }
    }
}
