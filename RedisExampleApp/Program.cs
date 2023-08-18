using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Model;
using RedisExampleApp.Repository;
using RedisExampleApp.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedisService>(sp => new RedisService("localhost:6379"));

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseInMemoryDatabase("inMemDb");
});

builder.Services.AddScoped<IProductRepository>(sp =>
{
    var appDbContext = sp.GetRequiredService<AppDbContext>();

    var productRepository = new ProductRepository(appDbContext);
    
    var redisService=sp.GetRequiredService<RedisService>();
    return new ProductRepositoryWithCache(productRepository, redisService);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context=scope.ServiceProvider.GetService<AppDbContext>();

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
