using AutoMapper; // for MapperConfiguration, Mapper

namespace SuccessfulStartup.Api.Mapping
{
    public class AllViewModelsMappingProfiles // assembles single mapping congifuration from all mapping profiles
    {
        //public static MapperConfiguration _configuration =  new MapperConfiguration(configuration => configuration.AddProfile(new BusinessPlanViewModelMappingProfile())); // manual method
        public static MapperConfiguration _configuration = new MapperConfiguration(configuration => configuration.AddMaps(new[] { "SuccessfulStartup.Api"})); // adds all profiles automatically in this assembly
        
        public static Mapper GetMapper()
        {
            return new Mapper(_configuration);
        }
    }
}
