using System.Text.Encodings.Web;
using System.Text.Json;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cities = await _cityInfoRepository.GetCitiesAsync();
        // var result = _citiesDataStore.Cities;
        // var result = cities.Select(city => new CityWithoutPointsOfInterestDto() { Id = city.Id, Description = city.Description, Name = city.Name }).ToList();
        var result = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities);
        return Json(result); //Encoding gerekli değil :Newtonsoft 
        // return Json(result,new JsonSerializerOptions(){WriteIndented = true,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false) //Response headers a artı olarak content-legnt ekliyor
    {
        // var city = _citiesDataStore.Cities.FirstOrDefault(dto => dto.Id == id);
        var city =await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
        return city == null ? NotFound() : includePointOfInterest ? Ok(_mapper.Map<CityDto>(city)) : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}