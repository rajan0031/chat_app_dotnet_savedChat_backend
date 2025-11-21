using ChatApp.Hubs;

var builder = WebApplication.CreateBuilder(args);



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

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("AngularClient");


app.MapHub<ChatHub>("/chat");

app.Run();
