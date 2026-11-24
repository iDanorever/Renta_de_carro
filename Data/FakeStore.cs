using CarRental.Api.Models;

namespace CarRental.Api.Data;

public static class FakeStore
{
    public static List<User> Users { get; set; } = new()
    {
        new User { Id = 1, Username = "admin", Email = "admin@rental.com", Password = "123", Role = "Admin" },
        new User { Id = 2, Username = "cliente1", Email = "cliente@rental.com", Password = "123", Role = "Customer" }
    };

    public static List<Car> Cars { get; set; } = new()
    {
        new Car { Id = 1, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC-123", Category = "Sedán", Year = 2024, DailyPrice = 45, Status = "Disponible" },
        new Car { Id = 2, Brand = "Hyundai", Model = "Tucson", LicensePlate = "XYZ-789", Category = "SUV", Year = 2025, DailyPrice = 65, Status = "Disponible" },
        new Car { Id = 3, Brand = "Kia", Model = "Sportage", LicensePlate = "LMN-456", Category = "SUV", Year = 2023, DailyPrice = 60, Status = "Alquilado" }
    };

    public static List<Rental> Rentals { get; set; } = new();
}