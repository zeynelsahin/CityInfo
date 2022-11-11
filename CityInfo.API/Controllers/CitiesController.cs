using System.Text.Encodings.Web;
using System.Text.Json;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : Controller
{
    private readonly ICityInfoRepository _cityInfoRepository;

    public CitiesController(ICityInfoRepository cityInfoRepository)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cities = await _cityInfoRepository.GetCitiesAsync();
        // var result = _citiesDataStore.Cities;
        var result = cities.Select(city => new CityWithoutPointsOfInterestDto() { Id = city.Id, Description = city.Description, Name = city.Name }).ToList();
        return Json(result);//Encoding gerekli değil :Newtonsoft 
        // return Json(result,new JsonSerializerOptions(){WriteIndented = true,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [HttpGet("getById/{id:int}")]
    public IActionResult GetCity(int id)//Response headers a artı olarak content-legnt ekliyor
    {
        // var city = _citiesDataStore.Cities.FirstOrDefault(dto => dto.Id == id);
        var city = _cityInfoRepository.GetCityAsync(id, false);
        return Ok(city);
    }
}