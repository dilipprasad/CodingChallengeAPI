namespace CodingChallenge.Models.Response
{
    public class SearchCityByZipCodeResponse:ResponseBase
    {
        public List<CityDetails> CityDetails { get; set; }


        public SearchCityByZipCodeResponse()
        {
            CityDetails=new List<CityDetails>();
        }
    }
}
