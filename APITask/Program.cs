using API.Core.Interfaces;
using API.Core.Models;
using API.Infrastructure.Data;
using API.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB Connection
builder.Services.AddDbContext<APIDbContext>(options =>
options
 //.UseLazyLoadingProxies() // must use virtual for all data accessed
.UseSqlServer(builder.Configuration.GetConnectionString("API_DBConnection")));

// add identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<APIDbContext>();

//DI life time : addscoped -> jack of all trades, singelton -> single non changable, transient -> multiple
//builder.Services.AddScoped<ICategoryServices, CategoryServices>();

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
