using BackendAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["Database:ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<BotDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IMagic8BallRepository, Magic8BallRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
