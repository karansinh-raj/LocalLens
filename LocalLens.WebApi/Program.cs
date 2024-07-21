using LocalLens.WebApi.DependencyInjection;
using OpenAI_API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

var chatGptKey = builder.Configuration["OpenAIChatGpt:Key"];

var chat = new OpenAIAPI(chatGptKey);

builder.Services.AddSingleton(chat);

var app = builder.Build();
app.UseSwaggerDocumentation();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
