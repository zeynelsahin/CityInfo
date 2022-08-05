using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId}/pointsofinterest")]
public class PointsOfInterestController : Controller
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        return Ok(city.PointsInterest);
    }

    [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterest")]
    public IActionResult GetPointsOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();
        {
            var pointOfInterest = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

            return pointOfInterest == null ? NotFound() : Ok(pointOfInterest);
        }
    }

    [HttpPost]
    public ActionResult<PointOfInterestForCreationDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return Ok();
        var pointOfInterestFromStore = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        if (pointOfInterestId==null)
        {
            return NotFound();
        }

        pointOfInterestFromStore.Name = pointOfInterestForUpdateDto.Name;
        pointOfInterestFromStore.Description = pointOfInterestForUpdateDto.Description;

        return NoContent(); 
    }
}