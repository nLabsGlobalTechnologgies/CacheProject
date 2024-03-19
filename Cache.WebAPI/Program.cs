using Cache.WebAPI.Context;
using Cache.WebAPI.Models;
using EntityFrameworkCorePagination.Nuget.Pagination;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("MyDb");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var scoped = builder.Services.BuildServiceProvider();
var context = scoped.GetRequiredService<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("GetAllProducts", async(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
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
    var products = new List<Product>();
    for (int i = 0; i < 1000; i++)
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

app.Run();
