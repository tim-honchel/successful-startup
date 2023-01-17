using AutoMapper;


namespace SuccessfulStartup.Data.Mapping
{
    public class AllMappingProfiles
    {
        public static Mapper GetMapper()
        {
            return new Mapper(new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new BusinessPlanMappingProfile());
            }
            ));
        }
    }
}
