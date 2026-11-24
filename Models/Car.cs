using System;

namespace CarRental.Api.Models;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string Category { get; set; } = "Sedán"; 
    public int Year { get; set; }
    public decimal DailyPrice { get; set; }
    public string Status { get; set; } = "Disponible";
}