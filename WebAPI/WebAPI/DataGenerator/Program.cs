using System;
using System.Linq;
using Bogus;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

class Program
{
    static void Main()
    {
        using var context = new WebStoreContext(new DbContextOptionsBuilder<WebStoreContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Products;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
            .Options);

        SeedData(context);
    }

    static void SeedData(WebStoreContext context)
    {
        if (context.Products.Any() || context.Users.Any())
        {
            Console.WriteLine("Database already contains data.");
            return;
        }

        var productGroups = GenerateProductGroups(5);
        context.ProductGroups.AddRange(productGroups);
        context.SaveChanges();

        var products = GenerateProducts(20, productGroups);
        context.Products.AddRange(products);
        context.SaveChanges();

        var users = GenerateUsers(10);
        context.Users.AddRange(users);
        context.SaveChanges();

        Console.WriteLine("Data generated and saved.");
    }

    static List<ProductGroup> GenerateProductGroups(int count)
    {
        var groupFaker = new Faker<ProductGroup>()
            .RuleFor(g => g.Name, f => f.Commerce.Categories(1)[0]);

        return groupFaker.Generate(count);
    }

    static List<Product> GenerateProducts(int count, List<ProductGroup> productGroups)
    {
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Double(5, 500))
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.GroupID, f => f.PickRandom(productGroups).ID);

        return productFaker.Generate(count);
    }

    static List<User> GenerateUsers(int count)
    {
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Login, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Type, f => f.PickRandom(new[] { "Admin", "Casual" }))
            .RuleFor(u => u.IsActive, f => f.Random.Bool());

        return userFaker.Generate(count);
    }
}
