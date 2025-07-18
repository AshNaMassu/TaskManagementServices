using API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger()
       .ConfigureOptions()
       .ConfigureAPI()
       .AddApplication()
       .AddInfrastructure()
       .AddPersistence();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
