using ChatApp.Data;      // for ChatDbContext
using ChatApp.Hubs;      // for ChatHub
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers (optional, for REST APIs if needed)
builder.Services.AddControllers();

// Add CORS with credentials allowed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register EF Core with SQL Server
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();
app.UseCors("AngularClient");

app.MapControllers();              // REST endpoints if you add controllers
app.MapHub<ChatHub>("/chat");      // SignalR hub endpoint

app.Run();
