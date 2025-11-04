namespace HSEBank.ValueObjects
{
    public readonly struct Money(decimal value) : IEquatable<Money>, IComparable<Money>
    {
        public decimal Value { get; } = value;

        public static Money operator +(Money left, Money right)
        {
            return new(left.Value + right.Value);
        }

        public static Money operator -(Money left, Money right)
        {
            return new(left.Value - right.Value);
        }

        public static bool operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(Money left, Money right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <(Money left, Money right)
        {
            return left.Value < right.Value;
        }

        public bool IsPositive => Value > 0;
        public bool IsNegative => Value < 0;
        public bool IsZero => Value == 0;

        public Money Abs()
        {
            return new(Math.Abs(Value));
        }

        public bool Equals(Money other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Money other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(Money other)
        {
            return Value.CompareTo(other.Value);
        }

        public static implicit operator decimal(Money value)
        {
            return value.Value;
        }

        public static implicit operator Money(decimal value)
        {
            return new(value);
        }
    }

    public readonly struct PositiveDecimal
    {
        public decimal Value { get; }

        public PositiveDecimal(decimal value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Значение должно быть положительным", nameof(value));
            }

            Value = value;
        }
        public Money ToMoney()
        {
            return new(Value);
        }

        public Money ToNegativeMoney()
        {
            return new Money(-Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator decimal(PositiveDecimal value)
        {
            return value.Value;
        }

        public static explicit operator PositiveDecimal(decimal value)
        {
            return new(value);
        }

        public static explicit operator Money(PositiveDecimal value)
        {
            return value.ToMoney();
        }
    }

    public readonly struct AccountNumber
    {
        public string Value { get; }

        public AccountNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ACC"))
            {
                throw new ArgumentException("Номер счета должен начинаться с ACC", nameof(value));
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(AccountNumber number)
        {
            return number.Value;
        }
    }

    public readonly struct CategoryCode
    {
        public string Value { get; }

        public CategoryCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 4)
            {
                throw new ArgumentException("Код категории должен состоять из 4 символов", nameof(value));
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(CategoryCode code)
        {
            return code.Value;
        }
    }

    public readonly struct TransactionId
    {
        public string Value { get; }

        public TransactionId(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("TXN"))
            {
                throw new ArgumentException("ID транзакции должен начинаться с TXN", nameof(value));
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(TransactionId id)
        {
            return id.Value;
        }
    }
}
