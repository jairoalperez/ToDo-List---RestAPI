using Microsoft.EntityFrameworkCore;
using ToDoList_RestAPI.Data;
using ToDoList_RestAPI.Helpers;
using DotNetEnv;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Process connection string
var rawConnectionString = builder.Configuration.GetConnectionString("ToDoListDb")
                            ?? throw new InvalidOperationException(Messages.Database.NoConnectionString);
var connectionString = ReplaceConnectionString.BuildConnectionString(rawConnectionString);

// Configuration of EFC with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();