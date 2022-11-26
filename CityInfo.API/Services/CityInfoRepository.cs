using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        var result = await _context.Cities.ToListAsync();
        return result;
    }

    public async Task<(IEnumerable<City>,PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery,int pageNumber, int pageSize)
    {
        var collection = _context.Cities as IQueryable<City>;
        
        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(c => c.Name.Contains(searchQuery)||(c.Description!=null && c.Description.Contains(searchQuery)));
        }

        var totalItemCount = await collection.CountAsync();
        var paginationMetaData = new PaginationMetaData(totalItemCount,pageNumber,pageSize);
        var collectionToReturn =await collection.OrderBy(c => c.Name).Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
        return (collectionToReturn, paginationMetaData);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        if (includePointsOfInterest)
        {
            return await _context.Cities.Include(c => c.PointsOfInterest).Where(d => d.Id == cityId).FirstOrDefaultAsync();
        }

        return await _context.Cities.Where(d => d.Id == cityId).FirstOrDefaultAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        city?.PointsOfInterest.Add(pointOfInterest);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }

    public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}