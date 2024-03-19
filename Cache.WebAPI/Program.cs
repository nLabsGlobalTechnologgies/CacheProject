using Cache.WebAPI.Context;
using Cache.WebAPI.Models;
using EntityFrameworkCorePagination.Nuget.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("MyDb");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

var scoped = builder.Services.BuildServiceProvider();
var context = scoped.GetRequiredService<AppDbContext>();

var memoryCache = scoped.GetRequiredService<IMemoryCache>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("GetAllWithPagination", async(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
{
    //List<Product>? products = await context.Products!.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync(cancellationToken);
    //decimal count = await context.Products!.CountAsync(cancellationToken);
    //decimal totalPageNumbers = Math.Ceiling(count / pageSize);
    //decimal totalPageNumbers = (count + pageSize) > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 0;
    //bool isFirstPage = pageNumber == 1 ? true : false;
    //bool isLastPage = pageNumber == totalPageNumbers ? true : false;
    //var response = new
    //{
    //    Data = products,
    //    Count = count,
    //    TotalPageNumbers = totalPageNumbers,
    //    IsFirsPage = isFirstPage,
    //    IsLastPage = isLastPage,
    //    PageNumber = pageNumber,
    //    PageSize = pageSize
    //};
    //return response;

    var pageProducts = await context.Products!.ToPagedListAsync(pageNumber, pageSize, cancellationToken);

    return pageProducts;
});

app.MapGet("SeedData", () =>
{
    var db = ConnectionMultiplexer.Connect("localhost:6379");
    var redisCache = db.GetDatabase();
    redisCache.KeyDelete("products");

    memoryCache.Remove("products");

    var products = new List<Product>();
    for (int i = 0; i < 100000; i++)
    {
        Product product = new()
        {
            Name = "Product" + i
        };
        products.Add(product);
    }

    context.Products!.AddRange(products);
    context.SaveChanges();

    return new { Message = "Product SeedData created successfull" };
});


app.MapGet("GetAllProductsCache", async(CancellationToken cancellationToken) =>
{
    List<Product>? products;
    memoryCache.TryGetValue("products", out products);
    if (products is null)
    {
        products = await context.Products!.ToListAsync(cancellationToken);
        memoryCache.Set("products", products, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        });
    }

    return products.Count();
});

app.MapGet("GetAllProductsRedis", async (CancellationToken cancellationToken) =>
{
    var db = ConnectionMultiplexer.Connect("localhost:6379");
    var redisCache = db.GetDatabase();

    List<Product>? products = null;
    var redisValue = redisCache.StringGet("products");

    if (!redisValue.IsNullOrEmpty)
    {
        products = JsonSerializer.Deserialize<List<Product>?>(redisValue);
    }

    
    if (products is null)
    {
        products = await context.Products!.ToListAsync(cancellationToken);
        redisCache.StringSet("products", JsonSerializer.Serialize(products), TimeSpan.FromMinutes(20));
    }

    return products.Count();
});



app.Run();
