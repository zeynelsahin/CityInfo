using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId}/pointsofinterest")]
public class PointsOfInterestController : Controller
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly CitiesDataStore _citiesDataStore;
    private readonly LocalMailService _localMailService;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, CitiesDataStore citiesDataStore,LocalMailService localMailService)
    {
        
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(_citiesDataStore));
        _localMailService = localMailService;
        // HttpContext.RequestServices.GetService();
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        try
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                _logger.LogInformation("City with id {CityId} wasn't found when accessing points of interest", cityId);
                return NotFound();
            }

            return Ok(city.PointsInterest);
        }
        catch (Exception exception)
        {
            _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", exception);
            return StatusCode(500, "A problem happend while handling your request");
        }
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public IActionResult GetPointsOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();
        {
            var pointOfInterest = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

            return pointOfInterest == null ? NotFound() : Ok(pointOfInterest);
        }
    }

    [HttpPost]
    public IActionResult CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var maxPointOfInterestId = city.NumberOfPointsOfInterest;
        var finalPointOfInterest = new PointOfInterestDto
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterestForCreationDto.Name,
            Description = pointOfInterestForCreationDto.Description
        };
        city.PointsInterest.Add(finalPointOfInterest);
        return CreatedAtRoute("GetPointOfInterest", new
        {
            cityId = cityId,
            pointOfInterestId = finalPointOfInterest.Id
        }, finalPointOfInterest);
    }

    [HttpPut("{pointOfInterestId:int}")]
    public IActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return Ok();
        var pointOfInterestFromStore = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestId == null)
        {
            return NotFound();
        }

        pointOfInterestFromStore.Name = pointOfInterestForUpdateDto.Name;
        pointOfInterestFromStore.Description = pointOfInterestForUpdateDto.Description;
        return NoContent();
    }

    [HttpPatch("{pointOfInterestId:int}")]
    public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        {
            Name = pointOfInterestFromStore.Name,
            Description = pointOfInterestFromStore.Description
        };

        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
        return NoContent();
    }

    [HttpDelete($"{{pointOfInterestId}}")]
    public IActionResult DeleteOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        { 
            return NotFound();
        }

        city.PointsInterest.Remove(pointOfInterestFromStore);
        _localMailService.Send(subject:"Point of interest deleted.",message:$"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");
        return NoContent();
    }
}