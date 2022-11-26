﻿using System.Collections;
using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<(IEnumerable<City>,PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery,int pageNumber, int pageSize);
    Task<bool> CityExistsAsync(int cityId);
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
    Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
    Task<bool> SaveChangesAsync();
    
}