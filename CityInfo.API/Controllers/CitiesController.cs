using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : Controller
{
    [HttpGet]
    public ActionResult<List<CityDto>> GetCities()
    {
        var result = CitiesDataStore.Current.Cities;
        return Ok(result);
    }

    [HttpGet("getById/{id:int}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(dto => dto.Id == id);
        return city != null ? Ok(city) : NotFound();
    }
}