using EmployeeProject.Core.Models;
using PlantManagement.Core.Interfaces;
using PlantManagement.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind the ConnectionStrings section to the AppSettings.ConnectionString property
builder.Configuration.GetSection("ConnectionStrings").Bind(AppSettings.ConnectionString);
 
 
// Register your repository with the appropriate interface
builder.Services.AddScoped<IUserInterface, UserRepository>();
builder.Services.AddScoped<IAddPlantRepository, AddPlantRepository>();
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}








// Enable CORS
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
