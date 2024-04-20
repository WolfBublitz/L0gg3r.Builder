using System;
using L0gg3r;
using L0gg3r.Base;

namespace LoggerBuilderTests.MethodTests.WithMinimunLogLevelMethodTests;

[TestClass]
public class TheWithMinimunLogLevelMethod
{
    [TestMethod]
    public void ShouldCreateALoggerWithAGivenName()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Logger logger = loggerBuilder.WithMinimumLogLevel(LogLevel.Warning).Build();

        // Assert
        logger.MinimumLogLevel.Should().Be(LogLevel.Warning);
    }
}
