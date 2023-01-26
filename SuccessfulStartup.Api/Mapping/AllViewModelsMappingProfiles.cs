using AutoMapper;

namespace SuccessfulStartup.Api.Mapping
{
    public class AllViewModelsMappingProfiles
    {
        //public static MapperConfiguration _configuration =  new MapperConfiguration(configuration => configuration.AddProfile(new BusinessPlanViewModelMappingProfile())); // manual method
        public static MapperConfiguration _configuration = new MapperConfiguration(configuration => configuration.AddMaps(new[] { "SuccessfulStartup.Api"})); // adds all profiles automatically in this assembly
        
        public static Mapper GetMapper()
        {
            return new Mapper(_configuration);
        }
    }
}
