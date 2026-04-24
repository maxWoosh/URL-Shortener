using API.Middleware;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Логирование
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Контроллер
builder.Services.AddControllers();

// Swagger/OpenAPI (для тестирования)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

# region Регистрация зависимостей
// Репозиторий (in-memory) (синглтоном, чтобы данные хранились между запросами)
builder.Services.AddSingleton<IUrlRepository, InMemoryUrlRepository>();

// Генератор кодов
builder.Services.AddScoped<CodeGenerator>();

// Сервис
builder.Services.AddScoped<IUrlService, UrlService>();
# endregion

# region Middleware
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
# endregion


