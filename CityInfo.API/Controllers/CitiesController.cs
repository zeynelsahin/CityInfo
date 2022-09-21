using System.Text.Encodings.Web;
using System.Text.Json;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        return Json(result);//Encoding gerekli değil :Newtonsoft 
        // return Json(result,new JsonSerializerOptions(){WriteIndented = true,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [HttpGet("getById/{id:int}")]
    public IActionResult GetCity(int id)//Response headers a artı olarak content-legnt ekliyor
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(dto => dto.Id == id);
        return city != null ? Ok(city) : NotFound();
    }
}