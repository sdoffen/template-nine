namespace Template9.Test;

// This collection attribute is used to tell the test framework to
// inject the IntegrationTestFixture into the test class.
[Collection("IntegrationTest")]
public class SampleTest
{
    private readonly ProjectContext _projectContext;

    public SampleTest(IntegrationTestFixture fixture)
    {
        // Because the DbContext is provided via dependency injection,
        // we do not need to worry about disposing it.
        _projectContext = fixture.GetRequiredService<ProjectContext>();
    }

    [Fact]
    public void Test1()
    {
        
    }
}
