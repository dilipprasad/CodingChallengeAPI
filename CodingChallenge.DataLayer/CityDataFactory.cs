﻿using CodingChallenge.Models;

namespace CodingChallenge.DataLayer
{
    public class CityDataFactory : ICityDataFactory
    {
        private IDataLayer _dataLayer { get; }

        public CityDataFactory(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }


        public async Task<CityDetails> GetZipCodeByCity(string zipCode)
        {
            var cityDetailsDTO=await _dataLayer.GetCityDetails(zipCode);

            return Task<CityDetails>.FromResult(cityDetailsDTO.Select(x => new CityDetails
            {
                City = x.City
                ZipCode = x.ZipCode
            });
        }
    }
}
