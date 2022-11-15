using System.Reflection.Metadata.Ecma335;
using Bogus;
using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    // public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        var i = 0;
        var list = new List<CityDto>();
        while (i < 20)
        {
            var random = new Random().Next(1, 6);
            var cities = new Faker<CityDto>("tr").RuleFor(dto => dto.Id, i + 1).RuleFor(dto => dto.Name, faker => faker.Address.City())
                .RuleFor(dto => dto.Description, faker => faker.Address.StreetAddress())
                .RuleFor(dto => dto.PointsOfInterest, CreatePointsInterest(random)).Generate();
            i++;
            list.Add(cities);
        }

        Cities = list;
    }

    private List<PointOfInterestDto> CreatePointsInterest(int i)
    {
        var list = new List<PointOfInterestDto>();
        var count = 0;
        while (count <= i)
        {
            var pointOfInterestDto = new Faker<PointOfInterestDto>("tr").RuleFor(dto => dto.Id, count).RuleFor(dto => dto.Name, faker => faker.Address.City())
                .RuleFor(dto => dto.Description, faker => faker.Address.StreetAddress()).Generate();
            count++;
            list.Add(pointOfInterestDto);
        }

        return list;
    }
}