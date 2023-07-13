using Microsoft.Extensions.Caching.Memory;
using RecipesCostManagement.Contracts.RepositoryContracts;
using RecipesCostManagement.Contracts.ServiceContracts;
using RecipesCostManagement.Implementations;
using RecipesCostManagement.InfrastructureData.RepositoryImplementations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IRecipeManagementService, RecipeManagementService>();
builder.Services.AddScoped<IRecipeCostForMinutRepository, RecipeCostForMinutRepository>();
builder.Services.AddScoped<IRecipeIngredientsRepository, RecipeIngredientsRepository>();
builder.Services.AddScoped<IRecipeManagementCacheRepository, RecipeManagementCacheRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Services.AddMemoryCache(memoryCacheOptions =>
{
    memoryCacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(3);
    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(6),
        SlidingExpiration = TimeSpan.FromMinutes(1.5)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
