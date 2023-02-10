using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Data.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AllViewModelsMappingProfiles).Assembly);
builder.Services.AddCors(); // necessary because requests come from different URL
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>options.EnableAnnotations());
builder.Services.AddScoped<ViewModelConverter>();
DataLayerConfiguration.AddDataScope(builder.Services); // adds other services defined in the data project

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
