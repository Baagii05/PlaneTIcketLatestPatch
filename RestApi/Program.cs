using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using DataAccess.SetUp;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Flight Management API", Version = "v1" });
});

builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<PassengerRepository>();
builder.Services.AddScoped<SeatRepository>();

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<ISeatAssignmentManager, SeatAssignmentManager>();

var app = builder.Build();


app.UseCors();

DatabaseInitializer.EnsureDatabaseCreated();    

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();