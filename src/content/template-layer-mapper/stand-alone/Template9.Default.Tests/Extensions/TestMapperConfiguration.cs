using AutoMapper;

namespace Template9.Default.Tests.Extensions;

public class TestMapperConfiguration : DomainMapperConfiguration
{
    protected override MapperConfiguration ConfigureMapper()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TestMapperProfile>();
        });
    }
}