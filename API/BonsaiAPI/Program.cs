using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using BonsaiAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BonsaiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BonsaiContext")
        ?? throw new InvalidOperationException("Connection string 'BonsaiContext' not found.")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BonsaiContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();