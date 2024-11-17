using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options=>{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnections"));
});

var app = builder.Build();



app.MapControllers();

app.Run();