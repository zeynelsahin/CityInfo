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

    [HttpGet("{pointOfInterestId:int}")]
    public IActionResult GetPointsOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();
        {
            var pointOfInterest = city.PointsInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

            return pointOfInterest == null ? NotFound() : Ok(pointOfInterest);
        }

    }
}