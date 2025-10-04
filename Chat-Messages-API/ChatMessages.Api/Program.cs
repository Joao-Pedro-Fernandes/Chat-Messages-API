using ChatMessages.Application.Hubs;
using ChatMessages.Application.Services;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;
using ChatMessages.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // 👈 necessário para enviar cookies
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<ChatMessageContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33))
    )
);

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, QueryUserIdProvider>();

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapHub<ChatHub>("/chat-messages/api/chat-hub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
