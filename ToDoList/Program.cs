using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToDoList.Database;

var builder = WebApplication.CreateBuilder(args);
var sitePolicy = "_site-policy";

// Add services to the container.

builder.Services.AddControllers();

//Add database's service 
builder.Services.AddDbContext<ToDoListContext>(options=>
options.UseSqlite("DataSource=ToDoList.db"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(sitePolicy, built =>
    {
        built.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(sitePolicy);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/favicon.ico", () => Results.NotFound());

app.Run();
