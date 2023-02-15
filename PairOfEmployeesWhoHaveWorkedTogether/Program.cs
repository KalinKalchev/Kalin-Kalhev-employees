using PairOfEmployeesWhoHaveWorkedTogether.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IPairOfEmployeesWhoHaveWorkedTogetherService, PairOfEmployeesWhoHaveWorkedTogetherService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins(new string[]
    {
        "http://localhost:8080",
    });
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
