using AutoMapper;


namespace SuccessfulStartup.Data.Mapping
{
    public class AllMappingProfiles
    {
        //public static MapperConfiguration _configuration =  new MapperConfiguration(configuration => configuration.AddProfile(new BusinessPlanMappingProfile())); // manual method
        public static MapperConfiguration _configuration = new MapperConfiguration(configuration => configuration.AddMaps(new[] { "SuccessfulStartup.Data" })); // adds all profiles automatically in this assembly
        
        public static Mapper GetMapper()
        {
            return new Mapper(_configuration);
        }
    }
}
