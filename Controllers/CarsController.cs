using Microsoft.AspNetCore.Mvc;
using CarRental.Api.Data;
using CarRental.Api.Models;

namespace CarRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetAll()
    {
        return Ok(FakeStore.Cars);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Car> GetById(int id)
    {
        var car = FakeStore.Cars.FirstOrDefault(c => c.Id == id);
        if (car is null) return NotFound(new { message = "Auto no encontrado." });
        return Ok(car);
    }

    [HttpPost]
    public ActionResult<Car> Create(Car newCar)
    {
        var nextId = FakeStore.Cars.Count > 0 ? FakeStore.Cars.Max(c => c.Id) + 1 : 1;
        newCar.Id = nextId;
        if (string.IsNullOrWhiteSpace(newCar.Status))
            newCar.Status = "Disponible";

        FakeStore.Cars.Add(newCar);
        return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, newCar);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Car updatedCar)
    {
        var car = FakeStore.Cars.FirstOrDefault(c => c.Id == id);
        if (car is null) return NotFound(new { message = "Auto no encontrado." });

        car.Brand = updatedCar.Brand;
        car.Model = updatedCar.Model;
        car.LicensePlate = updatedCar.LicensePlate;
        car.Category = updatedCar.Category;
        car.Year = updatedCar.Year;
        car.DailyPrice = updatedCar.DailyPrice;
        car.Status = updatedCar.Status;

        return Ok(car);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var car = FakeStore.Cars.FirstOrDefault(c => c.Id == id);
        if (car is null) return NotFound(new { message = "Auto no encontrado." });

        FakeStore.Cars.Remove(car);
        return NoContent();
    }
}