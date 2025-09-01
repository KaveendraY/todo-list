    using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string (appsettings or env var: ConnectionStrings__DefaultConnection)
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
           ?? builder.Configuration["ConnectionStrings__DefaultConnection"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn));

// Allow Angular dev
builder.Services.AddCors(options => options.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Apply migrations with retry (useful in Docker when DB may not be ready)
await ApplyMigrationsWithRetry(app.Services, attempts: 10, delayMs: 2000);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

// ---- Helpers ----
static async Task ApplyMigrationsWithRetry(IServiceProvider sp, int attempts = 5, int delayMs = 2000)
{
    using var scope = sp.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migrator");
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    for (var i = 1; i <= attempts; i++)
    {
        try
        {
            await db.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully.");
            return;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Migration attempt {Attempt} failed. Retrying in {Delay}ms...", i, delayMs);
            if (i == attempts) throw;
            await Task.Delay(delayMs);
        }
    }
}
