using SuccessfulStartup.Api.Controllers;
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AllViewModelsMappingProfiles).Assembly);
//builder.Services.AddCors(); // may not be necessary,
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ViewModelConverter>();
builder.Services.AddScoped<WriteOnlyApi>(); // temporary workaround
DataLayerConfiguration.AddDataScope(builder.Services); // adds services defined in the data project

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors(); // may not be necessary

app.UseAuthorization();

app.MapControllers();

app.Run();
