using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : Controller
{
    private readonly CitiesDataStore _citiesDataStore;

    public CitiesController(CitiesDataStore citiesDataStore)
    {
        _citiesDataStore = citiesDataStore;
    }

    [HttpGet]
    public ActionResult<List<CityDto>> GetCities()
    {
        var result = _citiesDataStore.Cities;
        return Ok(result);
    }

    [HttpGet("getById/{id:int}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(dto => dto.Id == id);
        return city != null ? Ok(city) : NotFound();
    }
}