using EdTech.Interfaces;
using EdTech.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<EdTech.Interfaces.IConnectionProvider, EdTech.Utils.ConnectionProvider>();
builder.Services.AddSingleton<EdTech.Interfaces.IBCryptHelper, BCryptHelper>();
builder.Services.AddScoped<ITools, Tools>();
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

ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder = app.MapControllers();

app.Run();
