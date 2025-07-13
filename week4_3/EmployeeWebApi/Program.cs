using EmployeeWebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    // Add the custom exception filter globally to all controllers
    options.Filters.Add<CustomExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Employee Web API", 
        Version = "v1",
        Description = "A sample API for Employee Management with Custom Filters"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
        // Set Swagger UI at apps root
        c.RoutePrefix = string.Empty;
    });
}

// Remove HTTPS redirection for now to fix the issue
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add a simple redirect for debugging
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();