using AutoMapper;

namespace CodingChallenge.Business.MapperSetup
{

    public class ObjectMapper
    {

        public static Lazy<IConfigurationProvider> config = new Lazy<IConfigurationProvider>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CodingChallenge.Business.MapperSetup.MapperProfile>();//Use the profile created as a seperate class with the mappings

            });

            return config;
        });

        public static IConfigurationProvider Configuration
        {
            get { return config.Value; }
        }

        public static Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            return new Mapper(Configuration);
            
        });

        public static IMapper Mapper
        {
            get { return mapper.Value; }
        }

    }
}
