using AutoMapper;

namespace Fateor.Setup
{
    public static class SetupMapper
    {
        public static void AddMappers(this IServiceCollection services)
        {

            services.AddSingleton<IMapper>(factory =>
                {
                    var config = new MapperConfiguration(x => x.AddProfiles(
                        new Profile[]
                        {
                            new Mappers.ModelMapper(),
                            new Application.Mappers.AutoMapperProfile()
                        }));

                    return new Mapper(config);
                });


            services.AddSingleton<Fateor.Infrastructure.Mappers.IMapper, Fateor.Infrastructure.Mappers.Mapper>(x =>
                new Fateor.Infrastructure.Mappers.Mapper(x.GetRequiredService<AutoMapper.IMapper>())
                );
            
        }
    }
}
