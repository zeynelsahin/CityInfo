using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : Controller
{
    [HttpGet("deneme")]
    public IActionResult Index()
    {
        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
        return Ok(CitiesDataStore.Current.Cities);
    }

    [HttpGet("getById/{id:int}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(dto => dto.Id == id);
        return city != null ? Ok(city) : NotFound();
    }
}