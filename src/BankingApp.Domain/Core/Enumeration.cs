using System.Reflection;

namespace BankingApp.Domain.Core;

public abstract class Enumeration<TKey, TValue>(TKey key, TValue value) : IComparable
    where TKey : IComparable
    where TValue : IComparable
{
    public TKey Key { get; } = key;

    public TValue Value { get; } = value;

    public static IEnumerable<TValueObject> Enumerate<TValueObject>()
        where TValueObject : Enumeration<TKey, TValue>
    {
        var enumerationType = typeof(TValueObject);

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

        var fields = enumerationType.GetFields(bindingFlags);

        var properties = enumerationType.GetProperties(bindingFlags);

        return properties.Select(property => property.GetValue(null))
            .Union(fields.Select(field => field.GetValue(null)))
            .Cast<TValueObject>();
    }

    public static TValueObject ParseByValue<TValueObject>(TValue value)
        where TValueObject : Enumeration<TKey, TValue> => Convert<TValueObject, TValue>(value, nameof(value), item => item.Value.Equals(value));

    public static bool TryParseByValue<TValueObject>(TValue value, out TValueObject? valueObject)
        where TValueObject : Enumeration<TKey, TValue>
    {
        valueObject = TryConvert<TValueObject>(item => item.Value.Equals(value));
        return valueObject is not null;
    }

    public static TValueObject ParseByKey<TValueObject>(TKey id)
        where TValueObject : Enumeration<TKey, TValue> => Convert<TValueObject, TKey>(id, nameof(id), item => item.Key.Equals(id));

    public static bool TryParseByKey<TValueObject>(TKey key, out TValueObject? valueObject)
        where TValueObject : Enumeration<TKey, TValue>
    {
        valueObject = TryConvert<TValueObject>(item => item.Key.Equals(key));
        return valueObject is not null;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Enumeration<TKey, TValue>)obj);
    }

    public int CompareTo(object? obj)
    {
        if (obj is Enumeration<TKey, TValue> enumeration)
            return Key.CompareTo(enumeration.Key);

        return -1;
    }

    public override int GetHashCode() => HashCode.Combine(Key, Value);

    public static bool operator ==(Enumeration<TKey, TValue>? left, Enumeration<TKey, TValue>? right) =>
        left is not null && right is not null && left.Key.Equals(right.Key) && left.Value.Equals(right.Value);

    public static bool operator !=(Enumeration<TKey, TValue>? left, Enumeration<TKey, TValue>? right) =>
        left is null || right is null || !left.Key.Equals(right.Key) || !left.Value.Equals(right.Value);

    private bool Equals(Enumeration<TKey, TValue> other) =>
        Key.Equals(other.Key) && Value.Equals(other.Value);

    private static TValueObject Convert<TValueObject, T>(T value, string name, Func<TValueObject, bool> function)
        where TValueObject : Enumeration<TKey, TValue>
    {
        var item = TryConvert(function);

        if (item is null)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not a valid {name} for type {typeof(TValueObject)}");

        return item;
    }

    private static TValueObject? TryConvert<TValueObject>(Func<TValueObject, bool> function)
        where TValueObject : Enumeration<TKey, TValue>
    {
        return Enumerate<TValueObject>().FirstOrDefault(function);
    }
}