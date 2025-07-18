using API.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger()
       .ConfigureAPI()
       .AddApplication()
       .AddInfrastructure()
       .AddPersistence();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
