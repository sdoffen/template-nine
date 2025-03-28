using AutoMapper;

namespace Template9.Common.Mapper;

public abstract class DomainMapperConfiguration : IAutoMapperConfiguration
{
    public IMapper Mapper { get; protected set; }

    protected DomainMapperConfiguration()
    {
        var config = ConfigureMapper();
        config.AssertConfigurationIsValid();

        Mapper = config.CreateMapper();
    }

    protected abstract MapperConfiguration ConfigureMapper();
}
