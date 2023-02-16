using Microsoft.EntityFrameworkCore;
using TestDeviceServices.Source;

string apiToken = "tokentoken";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TestDeviceServices.AppContext>(option => option.UseInMemoryDatabase("db"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseWhen(context => context.Request.Path != "/api/device/regist" && context.Request.Path != "/api/event", appBuilder => {
    appBuilder.UseDeviceValidate();
});

app.Run();
