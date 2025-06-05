using Microsoft.Extensions.Configuration;

namespace Template9.Common.Extensions.Tests;

public class IConfigurationExtensionsTests
{
    private IConfiguration CreateConfigurationWithTool(string? toolValue)
    {
        var dict = new Dictionary<string, string?>
        {
            { "TOOL", toolValue }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(dict)
            .Build();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsToolExecuting_ReturnsFalse_WhenToolValueIsNullOrWhitespace(string? toolValue)
    {
        var config = CreateConfigurationWithTool(toolValue);
        config.IsToolExecuting().ShouldBeFalse();
    }

    [Theory]
    [InlineData("Docker")]
    [InlineData("Swagger")]
    [InlineData("some-tool")]
    public void IsToolExecuting_ReturnsTrue_WhenToolValueIsPresent(string toolValue)
    {
        var config = CreateConfigurationWithTool(toolValue);
        config.IsToolExecuting().ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ExecutingTool_ReturnsNone_WhenToolValueIsNullOrEmpty(string? toolValue)
    {
        var config = CreateConfigurationWithTool(toolValue);
        config.ExecutingTool().ShouldBe(Tool.None);
    }

    [Theory]
    [InlineData("Docker", Tool.Docker)]
    [InlineData("Swagger", Tool.Swagger)]
    public void ExecutingTool_ReturnsKnownTool_WhenValueMatches(string input, Tool expected)
    {
        var config = CreateConfigurationWithTool(input);
        config.ExecutingTool().ShouldBe(expected);
    }

    [Theory]
    [InlineData("DOCKER")] // test case-insensitive match
    [InlineData("swagger")]
    public void ExecutingTool_MatchesIgnoreCase(string input)
    {
        var config = CreateConfigurationWithTool(input);
        config.ExecutingTool().ShouldNotBe(Tool.Unknown);
    }

    [Fact]
    public void ExecutingTool_ReturnsUnknown_WhenToolValueIsNotRecognized()
    {
        var config = CreateConfigurationWithTool("Foobar");
        config.ExecutingTool().ShouldBe(Tool.Unknown);
    }
}
