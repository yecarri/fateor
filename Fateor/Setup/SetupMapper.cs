using AutoMapper;

namespace Fateor.Setup
{
    public static class SetupMapper
    {
        public static void AddMappers(this IServiceCollection services)
        {

            services.AddSingleton<IMapper>(factory =>
                {
                    var loggerFactory = factory.GetRequiredService<ILoggerFactory>();

                    var config = new MapperConfiguration(x => x.AddProfiles(
                        new Profile[]
                        {
                            new Mappers.ModelMapper(),
                            new Application.Mappers.AutoMapperProfile()
                        }), loggerFactory                                                                        
                    );

                    return new Mapper(config);
                });


            services.AddSingleton<Fateor.Infrastructure.Mappers.IMapper, Fateor.Infrastructure.Mappers.Mapper>(x =>
                new Fateor.Infrastructure.Mappers.Mapper(x.GetRequiredService<AutoMapper.IMapper>())
                );
            
        }
    }
}
