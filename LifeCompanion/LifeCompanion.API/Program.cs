using LifeCompanion.Domain.Options;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GeminiOptions>(GeminiOptions.TextOnly,
    builder.Configuration.GetSection("Gemini:TextOnly"));
builder.Services.Configure<GeminiOptions>(GeminiOptions.TextAndImage,
    builder.Configuration.GetSection("Gemini:TextAndImage"));

builder.Services.AddHttpClient("textOnlyGemini", (serviceProvider, httpClient) => {
    var textOnlyGemini = serviceProvider.GetRequiredService<IOptionsMonitor<GeminiOptions>>().Get(GeminiOptions.TextOnly);
    //httpClient.DefaultRequestHeaders.Add("accept", textOnlyGemini.ContentType);
    httpClient.BaseAddress = new Uri(builder.Configuration["Gemini:BaseAddress"]) ;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
