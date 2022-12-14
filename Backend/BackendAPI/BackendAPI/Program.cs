using BackendAPI.Repositories;
using BackendAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["Database:ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<BotDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IMagic8BallRepository, Magic8BallRepository>();
builder.Services.AddScoped<IMagic8BallService, Magic8BallService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventVoteRepository, EventVoteRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICommandGroupRepository, CommandGroupRepository>();
builder.Services.AddScoped<ISubCommandRepository, SubCommandRepository>();
builder.Services.AddScoped<ICommandParameterRepository, CommandParameterRepository>();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddSingleton<Random>();

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
