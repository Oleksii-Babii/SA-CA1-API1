var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "BonsaiAPI is running");

app.Run();