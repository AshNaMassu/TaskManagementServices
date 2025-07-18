using API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger()
       .ConfigureAPI()
       .AddApplication()
       .AddInfrastructure()
       .AddPersistence();

var app = builder.Build();

app.ApplyMigration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
