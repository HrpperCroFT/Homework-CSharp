using System;
using System.Collections.Generic;

public interface IEntity
{
    int Id { get; }
}

public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _items = new Dictionary<int, T>();

    public int Count => _items.Count;

    public void Add(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new InvalidOperationException($"Element with Id {item.Id} already exists.");
        _items[item.Id] = item;
    }

    public bool Remove(int id) => _items.Remove(id);

    public T? GetById(int id) => _items.TryGetValue(id, out var item) ? item : default;

    public IReadOnlyList<T> GetAll()
    {
        return new List<T>(_items.Values);
    }

    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in _items.Values)
            if (predicate(item))
                result.Add(item);
        return result;
    }
}
public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        var productRepo = new Repository<Product>();

        var p1 = new Product { Id = 1, Name = "Laptop", Price = 1200m };
        var p2 = new Product { Id = 2, Name = "Mouse", Price = 25m };
        var p3 = new Product { Id = 3, Name = "Keyboard", Price = 80m };

        productRepo.Add(p1);
        productRepo.Add(p2);
        productRepo.Add(p3);

        Console.WriteLine($"Total count: {productRepo.Count}");
        Console.WriteLine("Products:");
        foreach (var p in productRepo.GetAll())
            Console.WriteLine($"  {p.Id}: {p.Name} - {p.Price}");

        var found = productRepo.GetById(2);
        Console.WriteLine($"\nSearching Id=2: {(found != null ? found.Name : "not found")}");

        var expensive = productRepo.Find(p => p.Price > 100);
        Console.WriteLine("\nProducts with cost more than 100:");
        foreach (var p in expensive)
            Console.WriteLine($"  {p.Name} - {p.Price}");

        try
        {
            var duplicate = new Product { Id = 1, Name = "Videocard", Price = 300m };
            productRepo.Add(duplicate);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nError : {ex.Message}");
        }

        bool removed = productRepo.Remove(3);
        Console.WriteLine($"\nRemoving Id=3: {(removed ? "success" : "not found")}");
        Console.WriteLine($"Products remain: {productRepo.Count}");

        var userRepo = new Repository<User>();
        userRepo.Add(new User { Id = 10, Name = "John", Age = 25 });
        userRepo.Add(new User { Id = 11, Name = "Ivan", Age = 30 });
        Console.WriteLine($"\nUsers: {userRepo.Count}");
    }
}