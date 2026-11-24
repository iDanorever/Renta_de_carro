using System.Net.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new { message = "CarRental API funcionando en el puerto 5101" }));

var apiTask = app.RunAsync("http://localhost:5101");

await RunConsoleLoginAndTableAsync("http://localhost:5101");

await apiTask;

static async Task RunConsoleLoginAndTableAsync(string baseUrl)
{
    using var http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    await Task.Delay(800);

    Console.WriteLine("\n=== CAR RENTAL - LOGIN (C#) ===\n");

    Console.Write("Correo: ");
    var email = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Contraseña: ");
    var password = Console.ReadLine()?.Trim() ?? "";

    var loginResponse = await http.PostAsJsonAsync("/api/auth/login", new { email, password });

    if (!loginResponse.IsSuccessStatusCode)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nCredenciales incorrectas.");
        Console.ResetColor();
        return;
    }

    var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nBienvenido, {loginData?.User?.Username ?? email}!\n");
    Console.ResetColor();

    var cars = await http.GetFromJsonAsync<List<CarItem>>("/api/cars") ?? new();

    Console.WriteLine("=== TABLA DE AUTOS ===\n");
    Console.WriteLine($"{"ID",-4} | {"MARCA",-12} | {"MODELO",-14} | {"PLACA",-10} | {"CATEGORÍA",-12} | {"AÑO",-6} | {"PRECIO/DÍA",-11} | {"ESTADO",-12}");
    Console.WriteLine(new string('-', 102));

    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Id,-4} | {car.Brand,-12} | {car.Model,-14} | {car.LicensePlate,-10} | {car.Category,-12} | {car.Year,-6} | ${car.DailyPrice,-10:0.00} | {car.Status,-12}");
    }
}

record LoginResponse(
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("user")] UserInfo? User
);

record UserInfo(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("email")] string Email
);

record CarItem(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("brand")] string Brand,
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("licensePlate")] string LicensePlate,
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("dailyPrice")] decimal DailyPrice,
    [property: JsonPropertyName("status")] string Status
);