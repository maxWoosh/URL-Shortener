using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллер
builder.Services.AddControllers();

// Настраиваем Swagger/OpenAPI (для тестирования)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

# region Регистрация зависимостей
// Репозиторий (in-memory)
builder.Services.AddScoped<IUrlRepository, InMemoryUrlRepository>();

// Генератор кодов
builder.Services.AddScoped<CodeGenerator>();

// Сервис
builder.Services.AddScoped<IUrlService, UrlService>();
# endregion

var app = builder.Build();

// Конвейер обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


