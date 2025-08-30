using BussinessLogic.Interfaces;
using BussinessLogic.Services;
using DataAccess.Repositories;
using DataAccess.SetUp;
using Microsoft.Data.Sqlite;

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

// Register a scoped SqliteConnection for each request
builder.Services.AddScoped<SqliteConnection>(sp =>
{
    var conn = new SqliteConnection("Data Source=flights.db");
    conn.Open();
    return conn;
});

// Register repositories with the connection
builder.Services.AddScoped<FlightRepository>(sp =>
    new FlightRepository(sp.GetRequiredService<SqliteConnection>()));
builder.Services.AddScoped<PassengerRepository>(sp =>
    new PassengerRepository(sp.GetRequiredService<SqliteConnection>()));
builder.Services.AddScoped<SeatRepository>(sp =>
    new SeatRepository(sp.GetRequiredService<SqliteConnection>()));

// Register services as before
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<ISeatAssignmentManager, SeatAssignmentManager>();

var app = builder.Build();

app.UseCors();

// Ensure database schema and seed data using the DI-managed connection
using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<SqliteConnection>();
    DatabaseInitializer.EnsureDatabaseCreated(connection);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();