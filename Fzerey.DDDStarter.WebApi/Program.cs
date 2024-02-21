


using Fzerey.DDDStarter.Infrastructure.Context;
using Fzerey.DDDStarter.WebApi;
using Fzerey.DDDStarter.WebApi.Middlewares;
using Fzerey.DDDStarter.WebApi.Models.Exception;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().MinimumLevel.Information()
    .CreateLogger();
if (builder.Environment.EnvironmentName == Environments.Development)
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", true, true);
    
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", true, true);
}


builder.Host.UseSerilog(Log.Logger);
builder.Services.RegisterApiModule(builder.Configuration);

var app = builder.Build();

app.Configuration["RandomKey"] = new Random().Next(1, 100).ToString();
if (app.Environment.EnvironmentName == Environments.Development)
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"./v1/swagger.json", "API V1");
});

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapControllers();

app.UseHttpsRedirection();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();

