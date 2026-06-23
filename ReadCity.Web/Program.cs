using ReadCity.DAL.Database;
using ReadCity.DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов в контейнере приложения.
builder.Services.AddRazorPages();

// Регистрация DatabaseConnection через строку подключения из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена.");
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

// Регистрация BookService для внедрения зависимостей
builder.Services.AddScoped<BookService>();

var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
