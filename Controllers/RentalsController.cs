using Microsoft.AspNetCore.Mvc;
using CarRental.Api.Data;
using CarRental.Api.Models;

namespace CarRental.Api.Controllers;

public record CreateRentalRequest(int UserId, int CarId, DateTime StartDate, DateTime EndDate, string? Notes);

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Rental>> GetAll()
    {
        return Ok(FakeStore.Rentals);
    }

    [HttpPost]
    public ActionResult<Rental> Create([FromBody] CreateRentalRequest request)
    {
        if (request.EndDate <= request.StartDate)
            return BadRequest(new { message = "La fecha de fin debe ser posterior a la fecha de inicio." });

        var user = FakeStore.Users.FirstOrDefault(u => u.Id == request.UserId);
        if (user is null)
            return NotFound(new { message = "Usuario no encontrado." });

        var car = FakeStore.Cars.FirstOrDefault(c => c.Id == request.CarId);
        if (car is null)
            return NotFound(new { message = "Auto no encontrado." });

        if (car.Status == "Mantenimiento")
            return BadRequest(new { message = "El auto está en mantenimiento y no puede alquilarse." });

        var hasOverlap = FakeStore.Rentals.Any(r =>
            r.CarId == request.CarId &&
            r.Status == "Activo" &&
            request.StartDate < r.EndDate &&
            request.EndDate > r.StartDate);

        if (hasOverlap)
            return Conflict(new { message = "El auto ya está alquilado en ese rango de fechas." });

        var days = Math.Max(1, (int)Math.Ceiling((request.EndDate - request.StartDate).TotalDays));
        var totalPrice = days * car.DailyPrice;

        var nextId = FakeStore.Rentals.Count > 0 ? FakeStore.Rentals.Max(r => r.Id) + 1 : 1;

        var rental = new Rental
        {
            Id = nextId,
            UserId = request.UserId,
            CarId = request.CarId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalPrice = totalPrice,
            Status = "Activo"
        };

        car.Status = "Alquilado";
        FakeStore.Rentals.Add(rental);

        return Ok(rental);
    }
}