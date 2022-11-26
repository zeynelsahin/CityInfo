using System.Text.Encodings.Web;
using System.Text.Json;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityInfo.API.Controllers;

[ApiController]
// [Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/cities")]
public class CitiesController : Controller
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private const int maxCitiesPageSize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery(Name = "filteronname")] string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }
        
        var (cities,paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery,pageNumber,pageSize);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginationMetaData));
        var result = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities);

        return Json(result); //Encoding gerekli değil :Newtonsoft 
        // return Json(result,new JsonSerializerOptions(){WriteIndented = true,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false) //Response headers a artı olarak content-legnt ekliyor
    {
        // var city = _citiesDataStore.Cities.FirstOrDefault(dto => dto.Id == id);
        var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
        return city == null ? NotFound() : includePointOfInterest ? Ok(_mapper.Map<CityDto>(city)) : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}