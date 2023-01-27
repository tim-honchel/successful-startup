using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Data; // TODO: necessary for authentication, but would like to find an alternative

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AllViewModelsMappingProfiles).Assembly);
builder.Services.AddCors(); // necessary because requests come from different URL
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ViewModelConverter>();
DataLayerConfiguration.AddDataScope(builder.Services); // adds services defined in the data project

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

app.MapControllers();

app.Run();
