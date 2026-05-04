using System;
using System.Collections.Generic;

public static class CollectionUtils
{
    public static List<T> Distinct<T>(List<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var result = new List<T>();
        var seen = new HashSet<T>();
        foreach (var item in source)
        {
            if (seen.Add(item))
                result.Add(item);
        }
        return result;
    }

    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
        List<TValue> source,
        Func<TValue, TKey> keySelector) where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        var dict = new Dictionary<TKey, List<TValue>>();
        foreach (var item in source)
        {
            var key = keySelector(item);
            if (!dict.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                dict[key] = list;
            }
            list.Add(item);
        }
        return dict;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        if (conflictResolver == null) throw new ArgumentNullException(nameof(conflictResolver));

        var result = new Dictionary<TKey, TValue>(first);
        foreach (var kv in second)
        {
            if (result.TryGetValue(kv.Key, out var existingValue))
                result[kv.Key] = conflictResolver(existingValue, kv.Value);
            else
                result[kv.Key] = kv.Value;
        }
        return result;
    }

    public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (source.Count == 0)
            throw new InvalidOperationException("Collection must be not empty.");

        T maxElement = source[0];
        TKey maxKey = selector(maxElement);

        for (int i = 1; i < source.Count; i++)
        {
            T current = source[i];
            TKey key = selector(current);
            if (key.CompareTo(maxKey) > 0)
            {
                maxElement = current;
                maxKey = key;
            }
        }
        return maxElement;
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 1, 2, 2, 3, 1, 4, 5, 5 };
        var uniqueNumbers = CollectionUtils.Distinct(numbers);
        Console.WriteLine("Distinct (numbers): " + string.Join(", ", uniqueNumbers));

        List<string> words = new List<string> { "cat", "dog", "cat", "hedgehog", "dog", "lion" };
        var uniqueWords = CollectionUtils.Distinct(words);
        Console.WriteLine("Distinct (words): " + string.Join(", ", uniqueWords));

        List<string> animals = new List<string> { "cat", "dog", "hedgehog", "lion", "tiger", "mouse" };
        var groupedByLength = CollectionUtils.GroupBy(animals, s => s.Length);
        Console.WriteLine("\nGroupBy (by length):");
        foreach (var kv in groupedByLength)
        {
            Console.WriteLine($"  Length {kv.Key}: {string.Join(", ", kv.Value)}");
        }

        var dict1 = new Dictionary<string, int> { { "apple", 3 }, { "pine", 2 }, { "plum", 5 } };
        var dict2 = new Dictionary<string, int> { { "apple", 1 }, { "cherry", 4 }, { "pine", 1 } };
        var merged = CollectionUtils.Merge(dict1, dict2, (a, b) => a + b);
        Console.WriteLine("\nMerge of dictionaries (sum):");
        foreach (var kv in merged)
            Console.WriteLine($"  {kv.Key}: {kv.Value}");

        var products = new List<Product> {
            new Product { Id = 1, Name = "Laptop", Price = 1200m },
            new Product { Id = 2, Name = "Mouse", Price = 25m },
            new Product { Id = 3, Name = "Keyboard", Price = 80m }
        };
        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"\nMaxBy (highest price): {mostExpensive.Name} - {mostExpensive.Price}");

        try
        {
            var emptyList = new List<int>();
            CollectionUtils.MaxBy(emptyList, x => x);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nExpected exception: {ex.Message}");
        }
    }
}