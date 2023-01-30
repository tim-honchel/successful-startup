using AutoMapper; // for MapperConfiguration and Mapper

namespace SuccessfulStartup.Data.Mapping
{
    public class AllMappingProfiles // creates a single mapping configuration from all mapping profiles
    {
        //public static MapperConfiguration _configuration =  new MapperConfiguration(configuration => configuration.AddProfile(new BusinessPlanMappingProfile())); // manual method
        public static MapperConfiguration _configuration = new MapperConfiguration(configuration => configuration.AddMaps(new[] { "SuccessfulStartup.Data" })); // adds all profiles automatically in this assembly
        
        public static Mapper GetMapper()
        {
            return new Mapper(_configuration);
        }
    }
}
