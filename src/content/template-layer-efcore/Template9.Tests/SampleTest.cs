namespace Template9.Tests;

// This collection attribute is used to tell the test framework to
// inject the IntegrationTestFixture into the test class.
[Collection("IntegrationTest")]
public class SampleTest
{
    private readonly ProjectDbContext _context;

    public SampleTest(IntegrationTestFixture fixture)
    {
        // Because the DbContext is provided via dependency injection,
        // we do not need to worry about disposing it.
        _context = fixture.GetRequiredService<ProjectDbContext>();
    }

    [Fact]
    public void Test1()
    {
        
    }
}
