using System;
using L0gg3r;
using L0gg3r.Base;

namespace LoggerBuilderTests.MethodTests.WithChildLogger__name_loggerBuilder__MethodTests;

[TestClass]
public class TheWithChildLoggerMethod
{
    [TestMethod]
    public void ShouldCreateALoggerWithAChildLogger()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Logger logger = loggerBuilder.WithChildLogger("ChildLogger", childLoggerBuilder =>
        {
            childLoggerBuilder.WithMinimumLogLevel(LogLevel.Warning);
        }).Build();

        // Assert
        logger.ChildLoggers.Should().ContainKey("ChildLogger");
        logger.ChildLoggers["ChildLogger"].MinimumLogLevel.Should().Be(LogLevel.Warning);
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfNameIsNull()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Action action = () => loggerBuilder.WithChildLogger(null!, _ => { });

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfActionIsNull()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Action action = () => loggerBuilder.WithChildLogger("ChildLogger", null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
