using Microsoft.EntityFrameworkCore;
using BonsaiAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BonsaiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BonsaiContext")
        ?? throw new InvalidOperationException("Connection string 'BonsaiContext' not found.")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BonsaiContext>();
    db.Database.Migrate();
}

app.MapGet("/", () => "BonsaiAPI is running");

app.Run();