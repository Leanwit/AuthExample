namespace WebApi.Test.Helper
{
    using AutoMapper;
    using WebApi.Infrastructure.Mapper;

    public static class Services
    {
        public static IMapper CreateAutoMapperObjectUsingUserProfile()
        {
            var myProfile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            return new Mapper(configuration);
        }
    }
}