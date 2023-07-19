using Url_Shortener.API.Extensions;
using Url_Shortener.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbConnection(builder.Configuration);
builder.Services.RegisterServices();
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

app.AddRedirect();

/*Task HandleRedirect(HttpContext context)
{
    context.Response.Redirect("https://google.com");
    return Task.CompletedTask;
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
