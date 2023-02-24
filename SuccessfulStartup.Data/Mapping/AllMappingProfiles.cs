using AutoMapper; // for MapperConfiguration and Mapper

namespace SuccessfulStartup.Data.Mapping
{
    public static class AllMappingProfiles // creates a single mapping configuration from all mapping profiles
    {
        //public readonly static MapperConfiguration _configuration =  new MapperConfiguration(configuration => configuration.AddProfile(new BusinessPlanMappingProfile())); // manual method
        public readonly static MapperConfiguration _configuration = new(configuration => configuration.AddMaps("SuccessfulStartup.Data")); // adds all profiles automatically in this assembly
        
        public static Mapper GetMapper()
        {
            return new Mapper(_configuration);
        }
    }
}
